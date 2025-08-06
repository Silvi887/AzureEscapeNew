using AzureAdd.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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
        public DbSet<UserVilla> UserVilla { get; set; } = null!;
        public DbSet<FeedBack> FeedBacks { get; set; } = null!;
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
                entity.HasKey(ur => new { ur.UserId, ur.VillaId });

                entity
             .HasOne(e => e.Villa)
             .WithMany(e => e.UserVillas)
             .HasForeignKey(e => e.VillaId)
             .OnDelete(DeleteBehavior.Restrict);

                entity
             .HasOne(e => e.User)
             .WithMany()
             .HasForeignKey(e => e.UserId)
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

            builder.Entity<FeedBack>(entity =>
            {
            entity.HasKey(f => f.IdFeedBack);


            entity
            .HasOne(f => f.Villa)
            .WithMany(v => v.Feedbacks)
            .HasForeignKey(f => f.VillaId)
            .OnDelete(DeleteBehavior.Restrict);

            entity
            .HasOne(f => f.Booking)
            .WithMany(v => v.Feedbacks)
            .HasForeignKey(f => f.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

                entity
             .HasOne(f => f.Guest)
             .WithMany()
             .HasForeignKey(f => f.GuestId);


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



            builder.Entity<Location>().HasData(
           new Location { IdLocation = 1, NameLocation = "Sunny Beach" },
           new Location { IdLocation = 2, NameLocation = "Golden Sands" },
           new Location { IdLocation = 3, NameLocation = "Sozopol" }
       );

          

            //TypepLace

            var TypepLace1 = new TypePlace
            {
                IdTypePlace=1,
                NamePlace = "vila"
            };
            var TypepLace2 = new TypePlace
            {
                IdTypePlace = 2,
                NamePlace = "penthhouse"
            };

            var TypepLace3 = new TypePlace
            {
                IdTypePlace = 3,
                NamePlace = "apartment"
            };

            builder.Entity<TypePlace>().HasData(TypepLace1);
            builder.Entity<TypePlace>().HasData(TypepLace2);
            builder.Entity<TypePlace>().HasData(TypepLace3);

            builder.Entity<VillaPenthhouse>().HasData(

                new VillaPenthhouse
                {
                    IdVilla=1,
                    NameVilla= "Villa Rio",
                    IdPlace=1,
                    VillaInfo="This is Fantastic Place for relax and enjoy!",
                    VillaAddress= "New str 17",
                    ImageUrl= "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/2a/44/d7/42/sol-nessebar-palace-all.jpg?w=900&h=500&s=1",
                    CountRooms=4,
                    CountAdults=2,
                    CountChildren=3,
                    Bedrooms=3,
                    Bathrooms=4,
                    Area="200m2",
                    Parking="Yes",
                    LocationId=2,
                    IDManager= "7699db7d-964f-4782-8209-d76562e0fece",
                    IsDeleted =false
                },
                 new VillaPenthhouse
                 {
                     IdVilla = 2,
                     NameVilla = "Relax",
                     IdPlace = 3,
                     VillaInfo = "This is Fantastic Place for relax and enjoy!",
                     VillaAddress = "Balcan str 25",
                     ImageUrl = "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/2f/ab/45/e6/caption.jpg?w=900&h=500&s=1",
                     CountRooms = 4,
                     CountAdults = 4,
                     CountChildren = 2,
                     Bedrooms = 3,
                     Bathrooms = 4,
                     Area = "400m2",
                     Parking = "Yes",
                     LocationId = 2,
                     IDManager = "7699db7d-964f-4782-8209-d76562e0fece",
                     IsDeleted = false
                 },
                  new VillaPenthhouse
                  {
                      IdVilla = 3,
                      NameVilla = "Aphrodita",
                      IdPlace = 2,
                      VillaInfo = "This is Fantastic Place for relax and enjoy!",
                      VillaAddress = "New str 15",
                      ImageUrl = "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/18/46/67/88/cook-s-club-sunny-beach.jpg?w=900&h=500&s=1",
                      CountRooms = 6,
                      CountAdults = 2,
                      CountChildren = 2,
                      Bedrooms = 3,
                      Bathrooms = 4,
                      Area = "500m2",
                      Parking = "Yes",
                      LocationId = 2,
                      IDManager = "7699db7d-964f-4782-8209-d76562e0fece",
                      IsDeleted = false
                  }

                );


            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
        }
}
