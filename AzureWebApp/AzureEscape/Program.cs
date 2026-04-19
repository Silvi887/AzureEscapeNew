using AzureAdd.Data;
using AzureAdd.DataModels;
using AzureServises.Core;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AzureEscape
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<AzureAddDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedAccount = false;

                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;

            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AzureAddDbContext>();



            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IVilla,VillaService>();
            builder.Services.AddScoped<ITownService, TownService>();
            builder.Services.AddScoped<IAvailableDates, AvailableDates>();


            var app = builder.Build();
            //seed roles

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                ApplicationUserSeeder.Seed(services);

                 ApplicationUserSeeder.SeedRoles(services);
            }


            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

           
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

    }
}
