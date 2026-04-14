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
    public class AzureAddDbContext : IdentityDbContext<ApplicationUser>
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
                .WithMany(u=> u.VillaPenths)
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
             .WithMany(u=> u.UserVillas)
             .HasForeignKey(e => e.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<Booking>(entityres =>
            {

                entityres.HasKey(r => r.IdBooking);

                entityres.HasOne(g => g.Guest)
                        .WithMany(u=> u.AllBookings)
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
            .WithMany(u=> u.AllFeedbackss)
            .HasForeignKey(f => f.GuestId);


            });

            builder.Entity<Manager>(entity =>
            {
                entity
               .HasKey(m => m.Id);

                entity
                    .Property(m => m.IsDeleted)
                    .HasDefaultValue(false);

                entity
                    .HasOne(m => m.User)
                    .WithOne(u => u.Manager)
                    .HasForeignKey<Manager>(m => m.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasIndex(m => new { m.UserId })
                    .IsUnique();

                entity
                    .HasQueryFilter(m => m.IsDeleted == false);
            });

          //  var defaultUser = new IdentityUser
          //  {
          //      Id = "7699db7d-964f-4782-8209-d76562e0fece",
          //      UserName = "admin@horizons.com",
          //      NormalizedUserName = "ADMIN@HORIZONS.COM",
          //      Email = "admin@horizons.com",
          //      NormalizedEmail = "ADMIN@HORIZONS.COM",
          //      EmailConfirmed = true,
          //      PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
          //new IdentityUser { UserName = "admin@horizons.com" },
          //"Admin123!")
          //  };
          //  builder.Entity<IdentityUser>().HasData(defaultUser);



            builder.Entity<Location>().HasData(
            new Location { IdLocation = 1, NameLocation = "Sunny Beach" },
            new Location { IdLocation = 2, NameLocation = "Golden Sands" },
            new Location { IdLocation = 3, NameLocation = "Sozopol" },
            new Location { IdLocation = 4, NameLocation = "Nessebar" },
            new Location { IdLocation = 5, NameLocation = "Albena" },
            new Location { IdLocation = 6, NameLocation = "Borovets" },
            new Location { IdLocation = 7, NameLocation = "Bansko" },
            new Location { IdLocation = 8, NameLocation = "Pamporovo" },
            new Location { IdLocation = 9, NameLocation = "Varna" },
            new Location { IdLocation = 10, NameLocation = "Burgas" }
       );

          

            //TypepLace

            builder.Entity<TypePlace>().HasData(
            new TypePlace { IdTypePlace = 1, NamePlace = "vila" },
            new TypePlace { IdTypePlace = 2, NamePlace = "penthhouse" },
            new TypePlace { IdTypePlace = 3, NamePlace = "apartment" },
            new TypePlace { IdTypePlace = 4, NamePlace = "Studio" },
            new TypePlace { IdTypePlace = 5, NamePlace = "House" },
            new TypePlace { IdTypePlace = 6, NamePlace = "Bungalow" },
            new TypePlace { IdTypePlace = 7, NamePlace = "Hotel Room" },
            new TypePlace { IdTypePlace = 8, NamePlace = "Guest House" }
                );

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
                    IDManager=null, /*"7699db7d-964f-4782-8209-d76562e0fece",*/
                    PricePerNight=100,
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
                     IDManager =null, //"7699db7d-964f-4782-8209-d76562e0fece",
                     PricePerNight = 180,
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
                      IDManager =null, //"7699db7d-964f-4782-8209-d76562e0fece",
                      PricePerNight = 340,
                      IsDeleted = false
                  },
                     new VillaPenthhouse
                     {
                         IdVilla = 4,
                         NameVilla = "Mountain Escape",
                         IdPlace = 5,
                         VillaInfo = "Cozy mountain house with fireplace and forest view.",
                         VillaAddress = "Pine Street 8",
                         ImageUrl = "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85",
                         CountRooms = 3,
                         CountAdults = 4,
                         CountChildren = 2,
                         Bedrooms = 2,
                         Bathrooms = 2,
                         Area = "140m2",
                         Parking = "Yes",
                         LocationId = 7,
                         IDManager =null, //"7699db7d-964f-4782-8209-d76562e0fece",
                         PricePerNight = 130,
                         IsDeleted = false
                     },

                    new VillaPenthhouse
                    {
                        IdVilla = 5,
                        NameVilla = "Luxury Penthouse Sky",
                        IdPlace = 2,
                        VillaInfo = "Modern penthouse with panoramic city views.",
                        VillaAddress = "City Center 101",
                        ImageUrl = "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688",
                        CountRooms = 6,
                        CountAdults = 6,
                        CountChildren = 2,
                        Bedrooms = 3,
                        Bathrooms = 3,
                        Area = "300m2",
                        Parking = "Yes",
                        LocationId = 9,
                        IDManager = null,//"7699db7d-964f-4782-8209-d76562e0fece",
                        PricePerNight = 350,
                        IsDeleted = false
                    },

                    new VillaPenthhouse
                    {
                        IdVilla = 6,
                        NameVilla = "Family Holiday Home",
                        IdPlace = 5,
                        VillaInfo = "Perfect for families with kids, large garden included.",
                        VillaAddress = "Green Park 5",
                        ImageUrl = "https://images.unsplash.com/photo-1572120360610-d971b9d7767c",
                        CountRooms = 4,
                        CountAdults = 5,
                        CountChildren = 3,
                        Bedrooms = 3,
                        Bathrooms = 2,
                        Area = "180m2",
                        Parking = "Yes",
                        LocationId = 3,
                        IDManager =null, //"7699db7d-964f-4782-8209-d76562e0fece",
                        PricePerNight = 160,
                        IsDeleted = false
                    },

                    new VillaPenthhouse
                    {
                        IdVilla = 7,
                        NameVilla = "Sunset Paradise",
                        IdPlace = 1,
                        VillaInfo = "Enjoy stunning sunsets over the sea every evening.",
                        VillaAddress = "Sunset Blvd 77",
                        ImageUrl = "https://images.unsplash.com/photo-1499793983690-e29da59ef1c2",
                        CountRooms = 5,
                        CountAdults = 6,
                        CountChildren = 2,
                        Bedrooms = 4,
                        Bathrooms = 3,
                        Area = "270m2",
                        Parking = "Yes",
                        LocationId = 2,
                        IDManager = null, //"7699db7d-964f-4782-8209-d76562e0fece",
                        PricePerNight = 240,
                        IsDeleted = false
                    },

                    new VillaPenthhouse
                    {
                        IdVilla = 8,
                        NameVilla = "Budget Stay Studio",
                        IdPlace = 4,
                        VillaInfo = "Affordable and comfortable place near the beach.",
                        VillaAddress = "Beach Street 3",
                        ImageUrl = "https://images.unsplash.com/photo-1554995207-c18c203602cb",
                        CountRooms = 1,
                        CountAdults = 2,
                        CountChildren = 0,
                        Bedrooms = 1,
                        Bathrooms = 1,
                        Area = "45m2",
                        Parking = "No",
                        LocationId = 10,
                        IDManager = null,  //"7699db7d-964f-4782-8209-d76562e0fece",
                        PricePerNight = 60,
                        IsDeleted = false
                        },
                    new VillaPenthhouse
                    {
                        IdVilla = 9,
                        NameVilla = "Ocean Breeze Villa",
                        IdPlace = 1,
                        VillaInfo = "Beautiful seaside villa with private pool.",
                        VillaAddress = "Ocean Drive 12",
                        ImageUrl = "https://images.unsplash.com/photo-1502005229762-cf1b2da7c5d6",
                        CountRooms = 5,
                        CountAdults = 6,
                        CountChildren = 2,
                        Bedrooms = 4,
                        Bathrooms = 3,
                        Area = "260m2",
                        Parking = "Yes",
                        LocationId = 2,
                        IDManager = null,//"7699db7d-964f-4782-8209-d76562e0fece",
                        PricePerNight = 280,
                        IsDeleted = false
                    },

                    new VillaPenthhouse
                    {
                        IdVilla = 10,
                        NameVilla = "City Lights Penthouse",
                        IdPlace = 2,
                        VillaInfo = "Luxury penthouse with skyline view.",
                        VillaAddress = "Downtown 55",
                        ImageUrl = "https://images.unsplash.com/photo-1493809842364-78817add7ffb",
                        CountRooms = 4,
                        CountAdults = 4,
                        CountChildren = 1,
                        Bedrooms = 2,
                        Bathrooms = 2,
                        Area = "210m2",
                        Parking = "Yes",
                        LocationId = 9,
                        IDManager = null,//"7699db7d-964f-4782-8209-d76562e0fece",
                        PricePerNight = 320,
                        IsDeleted = false
                    },

                    new VillaPenthhouse
                    {
                        IdVilla = 11,
                        NameVilla = "Green Garden House",
                        IdPlace = 5,
                        VillaInfo = "Quiet house surrounded by nature.",
                        VillaAddress = "Garden Road 6",
                        ImageUrl = "https://images.unsplash.com/photo-1568605114967-8130f3a36994",
                        CountRooms = 3,
                        CountAdults = 4,
                        CountChildren = 2,
                        Bedrooms = 2,
                        Bathrooms = 2,
                        Area = "150m2",
                        Parking = "Yes",
                        LocationId = 4,
                        IDManager = null,  //"7699db7d-964f-4782-8209-d76562e0fece",
                        PricePerNight = 120,
                        IsDeleted = false
                    },

                    new VillaPenthhouse
                    {
                        IdVilla = 12,
                        NameVilla = "Beachfront Bungalow",
                        IdPlace = 6,
                        VillaInfo = "Relax right on the beach with amazing views.",
                        VillaAddress = "Coastline 1",
                        ImageUrl = "https://images.unsplash.com/photo-1505691723518-36a5ac3be353",
                        CountRooms = 2,
                        CountAdults = 3,
                        CountChildren = 1,
                        Bedrooms = 1,
                        Bathrooms = 1,
                        Area = "90m2",
                        Parking = "No",
                        LocationId = 2,
                        IDManager = null,//"7699db7d-964f-4782-8209-d76562e0fece",
                        PricePerNight = 140,
                        IsDeleted = false
                    },

                    new VillaPenthhouse
                    {
                        IdVilla = 13,
                        NameVilla = "Luxury Hotel Suite",
                        IdPlace = 7,
                        VillaInfo = "Premium hotel room with all services included.",
                        VillaAddress = "Hotel Avenue 99",
                        ImageUrl = "https://images.unsplash.com/photo-1551882547-ff40c63fe5fa",
                        CountRooms = 2,
                        CountAdults = 2,
                        CountChildren = 1,
                        Bedrooms = 1,
                        Bathrooms = 1,
                        Area = "80m2",
                        Parking = "Yes",
                        LocationId = 8,
                        IDManager = null,//"7699db7d-964f-4782-8209-d76562e0fece",
                        PricePerNight = 200,
                        IsDeleted = false
                    },

                    new VillaPenthhouse
                    {
                        IdVilla = 14,
                        NameVilla = "Cozy Guest House",
                        IdPlace = 8,
                        VillaInfo = "Warm and welcoming guest house.",
                        VillaAddress = "Village Center 10",
                        ImageUrl = "https://images.unsplash.com/photo-1523217582562-09d0def993a6",
                        CountRooms = 3,
                        CountAdults = 4,
                        CountChildren = 2,
                        Bedrooms = 2,
                        Bathrooms = 2,
                        Area = "130m2",
                        Parking = "Yes",
                        LocationId = 6,
                        IDManager = null,//"7699db7d-964f-4782-8209-d76562e0fece",
                        PricePerNight = 110,
                        IsDeleted = false
                    },

                    new VillaPenthhouse
                    {
                        IdVilla = 15,
                        NameVilla = "Modern Apartment Plus",
                        IdPlace = 3,
                        VillaInfo = "Stylish apartment in the heart of the city.",
                        VillaAddress = "Central Blvd 45",
                        ImageUrl = null,//"https://images.unsplash.com/photo-1493666438817-866a91353ca9",
                        CountRooms = 3,
                        CountAdults = 4,
                        CountChildren = 1,
                        Bedrooms = 2,
                        Bathrooms = 2,
                        Area = "120m2",
                        Parking = "No",
                        LocationId = 9,
                        IDManager = null, //"7699db7d-964f-4782-8209-d76562e0fece",
                        PricePerNight = 150,
                        IsDeleted = false
                    },

                    new VillaPenthhouse
                    {
                        IdVilla = 16,
                        NameVilla = "Elite Sky Penthouse",
                        IdPlace = 2,
                        VillaInfo = "Top floor penthouse with private jacuzzi.",
                        VillaAddress = "Sky Tower 200",
                        ImageUrl = "https://images.unsplash.com/photo-1501183638710-841dd1904471",
                        CountRooms = 5,
                        CountAdults = 6,
                        CountChildren = 2,
                        Bedrooms = 3,
                        Bathrooms = 3,
                        Area = "320m2",
                        Parking = "Yes",
                        LocationId = 9,
                        IDManager = null,//"7699db7d-964f-4782-8209-d76562e0fece",
                        PricePerNight = 400,
                        IsDeleted = false
                    }

           );


            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
        }
}
