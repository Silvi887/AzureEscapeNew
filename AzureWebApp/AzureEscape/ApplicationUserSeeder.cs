using AzureAdd.Data;
using AzureAdd.DataModels;
using Microsoft.AspNetCore.Identity;

namespace AzureEscape
{
    public static class ApplicationUserSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var db = serviceProvider.GetRequiredService<AzureAddDbContext>();

            string email = "admin@horizons.com";
            string password = "Admin123!";

            var user = userManager.FindByEmailAsync(email)
                                   .GetAwaiter()
                                   .GetResult();

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(user, password)
                                        .GetAwaiter()
                                        .GetResult();

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ", result.Errors.Select(e => e.Description))
                    );
                }

            
            }
            //User2
            string email1 = "silviya.kab@gmail.com";
            string password1 = "55555777";

            var user1 = userManager.FindByEmailAsync(email1)
                                   .GetAwaiter()
                                   .GetResult();

            if (user1 == null)
            {
                user1 = new ApplicationUser
                {
                    UserName = email1,
                    Email = email1,
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(user1, password1)
                                        .GetAwaiter()
                                        .GetResult();

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ", result.Errors.Select(e => e.Description))
                    );
                }


            }
            //User3
            string email3 = "jane@gmail.com";
            string password3 = "111111";

            var user3 = userManager.FindByEmailAsync(email3)
                                   .GetAwaiter()
                                   .GetResult();

            if (user3 == null)
            {
                user3 = new ApplicationUser
                {
                    UserName = email3,
                    Email = email3,
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(user3, password3)
                                        .GetAwaiter()
                                        .GetResult();

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(", ", result.Errors.Select(e => e.Description))
                    );
                }


            }


            if (db.VillasPenthhouses.Any(v => v.IdVilla >= 1 && v.IdVilla <= 16 && v.IDManager == null))
            {

                var allvillas = db.VillasPenthhouses.Where(v => v.IdVilla > 1 && v.IdVilla <= 16).ToList();

                foreach (var v in allvillas)
                {
                    v.IDManager = user.Id;

                }
            }
            db.SaveChanges();
        }

        public static void SeedRoles(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminEmail = "admin@horizons.com";
            string emailuser = "silviya.kab@gmail.com";
            string emailuser2 = "jane@gmail.com";

            // Ensure Admin role exists
            var roleExists = roleManager.RoleExistsAsync("Admin")
                                        .GetAwaiter()
                                        .GetResult();

            if (!roleExists)
            {
                roleManager.CreateAsync(new IdentityRole("Admin"))
                           .GetAwaiter()
                           .GetResult();
            }

            // Get user
            var user = userManager.FindByEmailAsync(adminEmail)
                                  .GetAwaiter()
                                  .GetResult();

            if (user != null)
            {
                var isInRole = userManager.IsInRoleAsync(user, "Admin")
                                          .GetAwaiter()
                                          .GetResult();

                if (!isInRole)
                {
                    userManager.AddToRoleAsync(user, "Admin")
                               .GetAwaiter()
                               .GetResult();
                }
            }
            //User
            var user1 = userManager.FindByEmailAsync(emailuser)
                               .GetAwaiter()
                               .GetResult();

            if (user1 != null)
            {
                var isInRole = userManager.IsInRoleAsync(user1, "User")
                                          .GetAwaiter()
                                          .GetResult();

                if (!isInRole)
                {
                    userManager.AddToRoleAsync(user1, "User")
                               .GetAwaiter()
                               .GetResult();
                }
            }
            //
            var user2 = userManager.FindByEmailAsync(emailuser2)
                              .GetAwaiter()
                              .GetResult();

            if (user2 != null)
            {
                var isInRole = userManager.IsInRoleAsync(user2, "User")
                                          .GetAwaiter()
                                          .GetResult();

                if (!isInRole)
                {
                    userManager.AddToRoleAsync(user2, "User")
                               .GetAwaiter()
                               .GetResult();
                }
            }
        }

     
    }
}
