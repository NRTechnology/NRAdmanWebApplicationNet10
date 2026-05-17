using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NRAdmanWebApplicationNet10.Services;
using NRAdmanWebApplicationNet10.ViewModels;

namespace NRAdmanWebApplicationNet10.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class SettingsUserController(
        ApplicationDbContext applicationDbContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<SettingsUserController> logger) : Controller
    {
        public IActionResult Index()
        {
            return View("SettingsUserList");
        }

        [HttpGet]
        public IActionResult GetJsonResult()
        {
            var data = userManager.Users.Select(u => new
            {
                id = u.Id,
                userName = u.UserName,
                email = u.Email,
                phoneNumber = u.PhoneNumber,
                emailConfirmed = u.EmailConfirmed,
                phoneNumberConfirmed = u.PhoneNumberConfirmed,
                twoFactorEnabled = u.TwoFactorEnabled,
                lockoutEnabled = u.LockoutEnabled,
                lockoutEnd = u.LockoutEnd,
                isLocked = u.LockoutEnd.HasValue && u.LockoutEnd > DateTime.UtcNow
            }).ToList();

            return Json(data);
        }

        [HttpGet]
        public IActionResult IsUserNameUnique(string userName, string excludeId = "")
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return Json(new { isUnique = false });
            }

            var user = userManager.Users.FirstOrDefault(u => u.UserName == userName);
            if (user != null && !string.IsNullOrEmpty(excludeId) && user.Id == excludeId)
            {
                return Json(new { isUnique = true });
            }

            return Json(new { isUnique = user == null });
        }

        [HttpGet]
        public IActionResult IsEmailUnique(string email, string excludeId = "")
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Json(new { isUnique = false });
            }

            var user = userManager.Users.FirstOrDefault(u => u.Email == email);
            if (user != null && !string.IsNullOrEmpty(excludeId) && user.Id == excludeId)
            {
                return Json(new { isUnique = true });
            }

            return Json(new { isUnique = user == null });
        }

        [HttpGet]
        public IActionResult CreateModal()
        {
            var viewModel = new ApplicationUserViewModel();
            var roles = roleManager.Roles.Select(r => r.Name).ToList();
            ViewBag.AvailableRoles = roles;
            return PartialView("../Shared/_Modals/_ModalCreateUser", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditModal(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var viewModel = new ApplicationUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,
                Roles = userRoles.ToList()
            };

            var roles = roleManager.Roles.Select(r => r.Name).ToList();
            ViewBag.AvailableRoles = roles;

            return PartialView("../Shared/_Modals/_ModalEditUser", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(new { success = false, message = "Validasi gagal.", errors = errors.Select(e => e.ErrorMessage) });
            }

            // Server-side unique check for Username
            var existingUser = userManager.Users.FirstOrDefault(u => u.UserName == model.UserName);
            if (existingUser != null)
            {
                return Json(new { success = false, message = "Username sudah ada." });
            }

            // Server-side unique check for Email
            if (!string.IsNullOrEmpty(model.Email))
            {
                var existingEmail = userManager.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingEmail != null)
                {
                    return Json(new { success = false, message = "Email sudah digunakan." });
                }
            }

            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    EmailConfirmed = model.EmailConfirmed,
                    PhoneNumberConfirmed = model.PhoneNumberConfirmed,
                    TwoFactorEnabled = model.TwoFactorEnabled,
                    LockoutEnabled = model.LockoutEnabled
                };

                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return Json(new { success = false, message = "Gagal membuat user.", errors = errors });
                }

                // Assign roles if provided
                if (model.Roles != null && model.Roles.Any())
                {
                    var roleResult = await userManager.AddToRolesAsync(user, model.Roles);
                    if (!roleResult.Succeeded)
                    {
                        logger.LogWarning("Gagal menambahkan roles untuk user {UserId}", user.Id);
                    }
                }

                logger.LogInformation("User {UserName} berhasil dibuat oleh {AdminUser}", model.UserName, User.Identity?.Name);
                return Json(new { success = true, message = "User berhasil ditambahkan." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gagal membuat user {UserName}", model.UserName);
                return Json(new { success = false, message = "Gagal menyimpan data user. Silakan coba lagi." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, ApplicationUserViewModel model)
        {
            if (string.IsNullOrEmpty(id) || id != model.Id)
            {
                return Json(new { success = false, message = "ID tidak sesuai." });
            }

            // Remove password validation if password is empty (optional on edit)
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                var errorList = errors.Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validasi gagal.", errors = errorList });
            }

            try
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return Json(new { success = false, message = "Data user tidak ditemukan." });
                }

                // Check if username is being changed and if it's already taken
                if (user.UserName != model.UserName)
                {
                    var existingUser = userManager.Users.FirstOrDefault(u => u.UserName == model.UserName);
                    if (existingUser != null)
                    {
                        return Json(new { success = false, message = "Username sudah ada." });
                    }
                }

                // Check if email is being changed and if it's already taken
                if (user.Email != model.Email && !string.IsNullOrEmpty(model.Email))
                {
                    var existingEmail = userManager.Users.FirstOrDefault(u => u.Email == model.Email);
                    if (existingEmail != null)
                    {
                        return Json(new { success = false, message = "Email sudah digunakan." });
                    }
                }

                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.EmailConfirmed = model.EmailConfirmed;
                user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                user.TwoFactorEnabled = model.TwoFactorEnabled;
                user.LockoutEnabled = model.LockoutEnabled;

                // Update password if provided
                if (!string.IsNullOrEmpty(model.Password))
                {
                    // Validate password meets requirements
                    var passwordValidator = new PasswordValidator<ApplicationUser>();
                    var passwordValidationResult = await passwordValidator.ValidateAsync(
                        userManager, user, model.Password);

                    if (!passwordValidationResult.Succeeded)
                    {
                        var passwordErrors = passwordValidationResult.Errors.Select(e => e.Description).ToList();
                        return Json(new { success = false, message = "Password tidak memenuhi persyaratan keamanan.", errors = passwordErrors });
                    }

                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResult = await userManager.ResetPasswordAsync(user, token, model.Password);
                    if (!passwordResult.Succeeded)
                    {
                        var passwordErrors = passwordResult.Errors.Select(e => e.Description).ToList();
                        return Json(new { success = false, message = "Gagal mengubah password.", errors = passwordErrors });
                    }
                }

                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    var errors = updateResult.Errors.Select(e => e.Description).ToList();
                    return Json(new { success = false, message = "Gagal memperbarui user.", errors = errors });
                }

                // Update roles if provided
                if (model.Roles != null)
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    var removeResult = await userManager.RemoveFromRolesAsync(user, userRoles);
                    if (removeResult.Succeeded)
                    {
                        await userManager.AddToRolesAsync(user, model.Roles);
                    }
                }

                logger.LogInformation("User {UserName} berhasil diperbarui oleh {AdminUser}", model.UserName, User.Identity?.Name);
                return Json(new { success = true, message = "User berhasil diperbarui." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gagal memperbarui user {UserId}", id);
                return Json(new { success = false, message = "Gagal memperbarui data user. Silakan coba lagi." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return Json(new { success = false, message = "Data user tidak ditemukan." });
                }

                // Prevent deleting the current user
                if (user.Id == User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value)
                {
                    return Json(new { success = false, message = "Anda tidak dapat menghapus user yang sedang login." });
                }

                var result = await userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return Json(new { success = false, message = "Gagal menghapus user.", errors = errors });
                }

                logger.LogInformation("User {UserName} berhasil dihapus oleh {AdminUser}", user.UserName, User.Identity?.Name);
                return Json(new { success = true, message = "User berhasil dihapus." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gagal menghapus user {UserId}", id);
                return Json(new { success = false, message = "Gagal menghapus data user. Silakan coba lagi." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> LockUnlock(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return Json(new { success = false, message = "Data user tidak ditemukan." });
                }

                if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
                {
                    // User is locked, unlock them
                    await userManager.SetLockoutEndDateAsync(user, null);
                    logger.LogInformation("User {UserName} dibuka kuncinya oleh {AdminUser}", user.UserName, User.Identity?.Name);
                    return Json(new { success = true, message = "User berhasil dibuka kuncinya.", isLocked = false });
                }
                else
                {
                    // Lock the user
                    await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(1));
                    logger.LogInformation("User {UserName} dikunci oleh {AdminUser}", user.UserName, User.Identity?.Name);
                    return Json(new { success = true, message = "User berhasil dikunci.", isLocked = true });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gagal mengubah status lock user {UserId}", id);
                return Json(new { success = false, message = "Gagal mengubah status lock user. Silakan coba lagi." });
            }
        }
    }
}
