using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using NRAdmanWebApplicationNet10.Models;
using NRAdmanWebApplicationNet10.Services;
using NRAdmanWebApplicationNet10.ViewModel;
using Serilog.Core;

namespace NRAdmanWebApplicationNet10.Controllers
{
    public class UserController(
        ApplicationDbContext applicationDbContext,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        //IEmailSender emailSender,
        //IConfiguration configuration,
        ILogger<UserController> logger,
        IAntiBruteForceService antiBruteForce) : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2Fa(bool rememberMe)
        {
            // Ensure we have a user pending 2FA
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            ViewBag.RememberMe = rememberMe;
            return View("LoginWith2fa");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2Fa(string twoFactorCode, bool rememberMe, string returnUrl = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(twoFactorCode))
                {
                    ModelState.AddModelError(string.Empty, "Enter authentication code.");
                    ViewBag.RememberMe = rememberMe;
                    return View("LoginWith2fa");
                }

                // Clean code
                var code = twoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);
                // get the user who is in the 2fa flow
                var twoFaUser = await signInManager.GetTwoFactorAuthenticationUserAsync();
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                // Check if IP is blocked
                if (await antiBruteForce.IsBlockedAsync(ip))
                {
                    logger.LogWarning("Blocked login attempt from IP {Ip}", ip);
                    throw new Exception("Too many failed attempts from your IP. Try again later.");
                }

                var result =
                    await signInManager.TwoFactorAuthenticatorSignInAsync(code, rememberMe, rememberClient: false);

                if (result.Succeeded)
                {
                    // log success
                    applicationDbContext.LoginAttempts.Add(new LoginAttempt
                    {
                        UserId = twoFaUser?.Id,
                        IpAddress = ip,
                        AttemptType = "TwoFactor",
                        Success = true,
                        Details = "Success"
                    });
                    await applicationDbContext.SaveChangesAsync();

                    return RedirectToAction("Index", "Dashboard");
                }

                // log failure
                var failureDetail = result.IsLockedOut ? "LockedOut" : "Invalid2FACode";
                applicationDbContext.LoginAttempts.Add(new LoginAttempt
                {
                    UserId = twoFaUser?.Id,
                    IpAddress = ip,
                    AttemptType = "TwoFactor",
                    Success = false,
                    Details = failureDetail
                });
                await applicationDbContext.SaveChangesAsync();

                if (result.IsLockedOut)
                {
                    throw new Exception("Your account is locked out");
                }

                ModelState.AddModelError(string.Empty, "Invalid authentication code.");
                ViewBag.RememberMe = rememberMe;
                return View("LoginWith2fa");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                logger.LogError(ex, "2FA login failed");
                return View("LoginWith2fa");
            }
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                    // Check IP block before attempting sign-in
                    if (await antiBruteForce.IsBlockedAsync(ip))
                    {
                        logger.LogWarning("Blocked login attempt from IP {Ip}", ip);
                        throw new Exception("Too many failed attempts from your IP. Try again later.");
                    }

                    var result = await signInManager.PasswordSignInAsync(model.Username,
                            model.Password, model.RememberMe, lockoutOnFailure: true);

                    var foundUser = await userManager.FindByNameAsync(model.Username);

                    if (result.Succeeded)
                    {
                        // Log successful password login
                        applicationDbContext.LoginAttempts.Add(new LoginAttempt
                        {
                            UserId = foundUser?.Id,
                            IpAddress = ip,
                            AttemptType = "Password",
                            Success = true,
                            Details = "Success"
                        });
                        await applicationDbContext.SaveChangesAsync();

                        // reset any failure counters for this IP on successful login
                        await antiBruteForce.ResetFailuresAsync(ip);

                        if (foundUser is { UserName: not null })
                        {
                            var roles = await userManager.GetRolesAsync(foundUser);
                            foreach (var role in roles)
                            {
                                Console.WriteLine(role);
                            }
                        }

                        return RedirectToAction("Index", "Dashboard");
                    }

                    // Log failed attempts with reason
                    string reason;
                    if (result.IsLockedOut)
                    {
                        reason = "LockedOut";
                    }
                    else if (result.IsNotAllowed)
                    {
                        reason = "NotAllowed";
                    }
                    else if (result.RequiresTwoFactor)
                    {
                        reason = "RequiresTwoFactor";
                    }
                    else
                    {
                        reason = "InvalidCredentials";
                    }

                    applicationDbContext.LoginAttempts.Add(new LoginAttempt
                    {
                        UserId = foundUser?.Id,
                        IpAddress = ip,
                        AttemptType = "Password",
                        Success = false,
                        Details = reason
                    });
                    await applicationDbContext.SaveChangesAsync();

                    // Record failure to anti-brute-force
                    await antiBruteForce.RecordFailureAsync(ip);

                    if (result.IsLockedOut)
                    {
                        throw new Exception("Your account is locked out");
                    }

                    if (result.IsNotAllowed)
                    {
                        throw new Exception("Your account is not allowed. Check your email for confirmation");
                    }

                    if (result.RequiresTwoFactor)
                    {
                        // Redirect to 2FA token entry page
                        // Preserve RememberMe in TempData or query
                        return RedirectToAction(nameof(LoginWith2Fa), new { rememberMe = model.RememberMe });
                    }

                    throw new Exception("Username or password not match");
                }

                throw new Exception("Error model");
            }
            catch (Exception exception)
            {
                TempData["error"] = exception.Message;
                logger.LogError(exception, "Login failed for user {Username}", model.Username);
                return View("Login");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "User");
        }
    }
}
