using AzureAdd.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AzureAdd.Data
{
    public class AzureAddDbContext : IdentityDbContext
    {
        public AzureAddDbContext(DbContextOptions<AzureAddDbContext> options) : base(options)
        {
        }

        public DbSet<VillaPenthhouse> VillasPenthhouses { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<TypePlace> TypePlaces { get; set; } = null!;
        public DbSet<Amenity> Amenities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<VillaPenthhouse>(entity =>
            {

                entity.HasKey(h => h.IdVilla);

                entity
                .HasOne(e => e.Location)
                .WithMany(e => e.VillasPenthhouses)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

                entity
             .HasOne(e => e.TypePlace)
             .WithMany(e => e.VillasPenthhouses)
             .HasForeignKey(e => e.IdPlace)
             .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(h => h.Manager)
                .WithMany()
                .HasForeignKey(h => h.IDManager)
                .OnDelete(DeleteBehavior.Restrict);

                //  entity
                //.HasOne(e => e.Rooms)
                //.WithMany(e => e.Hotels)
                //.HasForeignKey(e => e.TownId)
                //.OnDelete(DeleteBehavior.Restrict);


            });

            builder.Entity<UserVilla>(entity =>
            {
                entity.HasKey(ur => new { ur.UseriId, ur.VillaId });

                entity
             .HasOne(e => e.Villa)
             .WithMany(e => e.UserVillas)
             .HasForeignKey(e => e.VillaId)
             .OnDelete(DeleteBehavior.Restrict);

                entity
             .HasOne(e => e.User)
             .WithMany()
             .HasForeignKey(e => e.UseriId)
             .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<Booking>(entityres =>
            {

                entityres.HasKey(r => r.IdBooking);

                entityres.HasOne(g => g.Guest)
                        .WithMany()
                        .HasForeignKey(g => g.GuestId);

                //entityres.HasOne(r => r.Room)
                //      .WithMany(r => r.Reservations)
                //      .HasForeignKey(r => r.RoomId);

                entityres.HasOne(h => h.VillaPenthhouse)
                      .WithMany(h => h.AllBookings)
                      .HasForeignKey(h => h.VillaId);


            });

             var defaultUser = new IdentityUser
            {
                Id = "7699db7d-964f-4782-8209-d76562e0fece",
                UserName = "admin@horizons.com",
                NormalizedUserName = "ADMIN@HORIZONS.COM",
                Email = "admin@horizons.com",
                NormalizedEmail = "ADMIN@HORIZONS.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
           new IdentityUser { UserName = "admin@horizons.com" },
           "Admin123!")
            };
            builder.Entity<IdentityUser>().HasData(defaultUser);

            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
        }
}
