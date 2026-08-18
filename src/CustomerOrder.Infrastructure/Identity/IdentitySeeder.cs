using CustomerOrder.Infrastructure.Persistence.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace CustomerOrder.Infrastructure.Identity
{
    public static class IdentitySeeder
    {
        public const string AdminRole = "admin";
        public const string UserRole = "user";

        public static void Seed(AppDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            using (var roleStore = new RoleStore<IdentityRole>(context))
            using (var roleManager = new RoleManager<IdentityRole>(roleStore))
            using (var userStore = new UserStore<ApplicationUser>(context))
            using (var userManager = new UserManager<ApplicationUser>(userStore))
            {
                EnsureRole(roleManager, AdminRole);
                EnsureRole(roleManager, UserRole);

                EnsureUser(userManager, "admin", "Admin123", AdminRole);
                EnsureUser(userManager, "user", "User123", UserRole);
            }
        }

        private static void EnsureRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (roleManager.RoleExists(roleName))
            {
                return;
            }

            Guard(roleManager.Create(new IdentityRole(roleName)), "Create role " + roleName);
        }

        private static void EnsureUser(
            UserManager<ApplicationUser> userManager,
            string userName,
            string password,
            string roleName)
        {
            var user = userManager.FindByName(userName);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userName,
                    Email = userName + "@customerorder.local"
                };

                Guard(userManager.Create(user, password), "Create user " + userName);
            }

            if (!userManager.IsInRole(user.Id, roleName))
            {
                Guard(userManager.AddToRole(user.Id, roleName), "Add " + userName + " to " + roleName);
            }
        }

        /// <summary>
        /// IdentityResult never throws - without this every failure is silent.
        /// </summary>
        private static void Guard(IdentityResult result, string operation)
        {
            if (result.Succeeded)
            {
                return;
            }

            throw new InvalidOperationException(
                operation + " failed: " + string.Join(" | ", result.Errors));
        }
    }
}

