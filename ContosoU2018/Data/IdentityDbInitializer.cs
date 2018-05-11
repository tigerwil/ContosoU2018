using ContosoU2018.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2018.Data
{
    public class IdentityDbInitializer
    {
        //mwilliams:  Part 9 - IDENTITY FRAMEWORK
        #region snippet_Initialize
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // For sample purposes we are seeding 2 users both with the same password.
                // The password is set with the following command:
                // dotnet user-secrets set SeedUserPW <pw> or Right-Click Project -> Manage User Secrets
                // This will create the secrets.json file in the following directory:
                //  %APPDATA%\microsoft\UserSecrets\<userSecretsId>\secrets.json
                // C:\Users\mwilliams\AppData\Roaming\Microsoft\UserSecrets\aspnet-ContosoU2018_Marc-1971EABE-6C1C-4DED-A710-DB6B49BCFC9D\secrets.json

                //admin (this is the admin account - full priveleges)
                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@contoso.com");
                await EnsureRole(serviceProvider, adminID, "admin");

                //student (generic student account - does not belong to real student)
                var suid = await EnsureUser(serviceProvider, testUserPw, "student@contoso.com");
                await EnsureRole(serviceProvider, suid, "student");

                //instructor (generic instructor account - does not belong to real instructor)
                var iuid = await EnsureUser(serviceProvider, testUserPw, "instructor@contoso.com");
                await EnsureRole(serviceProvider, iuid, "instructor");
            }
        }

    
        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = UserName };
                await userManager.CreateAsync(user, testUserPw);

            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByIdAsync(uid);

            //admin 
            if (role == "admin")
            {
                await userManager.SetLockoutEnabledAsync(user, false);
            }

            //admin

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
        #endregion

    }
}
