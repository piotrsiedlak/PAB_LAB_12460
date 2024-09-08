namespace LabRazorAsp
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    namespace LabRazorAsp.Data
    {
        public static class SeedData
        {
            public static async Task Initialize(IServiceProvider serviceProvider)
            {
                using (var context = new ApplicationDbContext(
                    serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
                {
                    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    if (!await context.Roles.AnyAsync())
                    {
                        await roleManager.CreateAsync(new IdentityRole("Admin"));
                        await roleManager.CreateAsync(new IdentityRole("User"));
                    }

                    if (!await context.Users.AnyAsync())
                    {
                        var adminUser = new ApplicationUser
                        {
                            UserName = "admin@admin.com",
                            Email = "admin@admin.com",
                            EmailConfirmed = true
                        };

                        await userManager.CreateAsync(adminUser, "Admin123!");
                        await userManager.AddToRoleAsync(adminUser, "Admin");

                        var normalUser = new ApplicationUser
                        {
                            UserName = "user@user.com",
                            Email = "user@user.com",
                            EmailConfirmed = true
                        };

                        await userManager.CreateAsync(normalUser, "User123!");
                        await userManager.AddToRoleAsync(normalUser, "User");
                    }

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
