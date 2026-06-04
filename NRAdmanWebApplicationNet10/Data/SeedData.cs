using NRAdmanWebApplicationNet10.Services;
using NRAdmanWebApplicationNet10.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace NRAdmanWebApplicationNet10.Data
{
    public class SeedData
    {         
        private static async Task SetInitData(IServiceProvider serviceProvider)
        {
            await using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            var defaultRouter = new NetworkRouter()
            {
                Name = "Default Router",
                IpAddress = "15.0.91.6",
                RouterType = EnumRouterType.Mikrotik,
                Username = "admin",
                Password = "eve"
            };
            var defaultRouterExist = await context.NetworkRouters.FirstOrDefaultAsync(r => r.Name == "Default Router");
            if (defaultRouterExist == null)
            {
                context.NetworkRouters.Add(defaultRouter);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SetUsername(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames =
            [
                "Administrator", "Operator", "Finance", "Technician", "Auditor", "Customer"
            ];

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Here you could create a super user who will maintain the web app
            var adminUser = new ApplicationUser()
            {
                UserName = "administrator",
                NormalizedUserName = "ADMINISTRATOR",
                Email = "administrator@gmail.com",
                NormalizedEmail = "ADMINISTRATOR@GMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = string.Empty
            };

            
            var keuanganUser = new ApplicationUser()
            {
                UserName = "keuangan",
                NormalizedUserName = "KEUANGAN",
                Email = "keuangan@gmail.com",
                NormalizedEmail = "KEUANGAN@GMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = string.Empty
            };

            var adminUserExist = await userManager.FindByEmailAsync("administrator@gmail.com");
            if (adminUserExist == null)
            {
                var createAdminUser = await userManager.CreateAsync(adminUser, "Administrator123!");
                if (createAdminUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }

            var keuanganUserExist = await userManager.FindByEmailAsync("keuangan@gmail.com");
            if (keuanganUserExist == null)
            {
                var createKeuanganUser = await userManager.CreateAsync(keuanganUser, "Keuangan123#");
                if (createKeuanganUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await userManager.AddToRoleAsync(keuanganUser, "Finance");
                }
            }

        }

        public static async Task Initialize(IServiceProvider serviceProvider)
        {            
            await SetUsername(serviceProvider);
            await SetInitData(serviceProvider);
        }
    }
}