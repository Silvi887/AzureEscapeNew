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
    }
}
