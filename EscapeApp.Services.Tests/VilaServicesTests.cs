using AzureAdd.Data;
using AzureAdd.DataModels;
using AzureApp.ViewModels;
using AzureServises.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace EscapeApp.Services.Tests
{
    public class Tests
    {

        private Mock<UserManager<ApplicationUser>> userManagerMock;
        private VillaService service;
        private AzureAddDbContext dbContextMock;

        // //private Mock<AzureAddDbContext> dbContextMock;
        private Mock<DbSet<VillaPenthhouse>> villaDbSetMock;

        //private Mock<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>> userManagerMock;
      
   
        Mock<DbSet<Booking>> bookingsMock;

        [SetUp]
        public void Setup()
        {
            //dbContextMock = new Mock<AzureAddDbContext>();
              dbContextMock = CreateDbContext();

            var store = new Mock<IUserStore<ApplicationUser>>();
            userManagerMock = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

          //  villaDbSetMock = new Mock<DbSet<VillaPenthhouse>>();

           // dbContextMock.Setup(x => x.VillasPenthhouses)
            //    .Returns(villaDbSetMock.Object);

            service = new VillaService(dbContextMock, userManagerMock.Object);

          
        }
        private AzureAddDbContext CreateDbContext()
        {
            //var options = new DbContextOptionsBuilder<AzureAddDbContext>()
            //    .UseInMemoryDatabase(databaseName: "TestDb")
            //    .Options;

            var options = new DbContextOptionsBuilder<AzureAddDbContext>()
       .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
       .Options;

            return new AzureAddDbContext(options);
        }


        [TearDown]
        public void TearDown()
        {
            dbContextMock?.Dispose();
        }

        [Test]
        public async Task AddBookingModel_UserExists_ReturnsTrue_AndSavesBooking()
        {
            // Arrange
            var userId = "user1";

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(new ApplicationUser { Id = userId });

            var model = new AddReservationViewModel
            {
                StartDate = "2026-01-01",
                EndDate = "2026-01-05",
                AdultsCount = 2,
                ChildrenCount = 1,
                VillaId = "1",
                GuestFirstName = "John",
                LastNameG = "Doe",
                DateofBirth = "1990-01-01",
                GuestAddress = "Sofia",
                GuestEmail = "test@test.com",
                GuestPhoneNumber = "123456",
                TotalPrice = 500
            };

            // Act
            var result = await service.AddBookingModel(userId, model);

            // Assert
            Assert.IsTrue(result);

            var saved = dbContextMock.Bookings.FirstOrDefault();

            Assert.IsNotNull(saved);
            Assert.AreEqual(userId, saved.GuestId);
            Assert.AreEqual(1, saved.VillaId);
            Assert.AreEqual("John", saved.FirstName);
            Assert.AreEqual(500, saved.TotalPricePrice);
        }

        [Test]
        public async Task AddBookingModel_UserNotFound_ReturnsFalse()
        {
            // Arrange
            userManagerMock.Setup(x => x.FindByIdAsync("missing"))
                .ReturnsAsync((ApplicationUser)null);

            var model = new AddReservationViewModel
            {
                StartDate = "2026-01-01",
                EndDate = "2026-01-05",
                VillaId = "1"
            };

            // Act
            var result = await service.AddBookingModel("missing", model);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, dbContextMock.Bookings.Count());
        }

        [Test]
        public async Task AddVilaModel_UserNotFound_ReturnsFalse()
        {
            userManagerMock
                .Setup(x => x.FindByIdAsync("missing-user"))
                .ReturnsAsync((ApplicationUser)null);

            var model = new AddVillaIndexViewModel
            {
                NameVilla = "Villa"
            };

            var result = await service.AddVilaModel("missing-user", model);

            Assert.IsFalse(result);
            Assert.AreEqual(0, dbContextMock.VillasPenthhouses.Count());
        }

        [Test]
        public async Task GetAllReservations_NormalUser_ReturnsOwnReservationsOnly()
        {
            var userId = "user1";

            var user = new ApplicationUser { Id = userId };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(false);

            var villa = new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Villa A",
                Area="200m3",
                Parking="yea",
                VillaAddress="Test",
                VillaInfo="Test Test"
            };

            dbContextMock.Bookings.AddRange(
                new Booking
                {
                    IdBooking = 1,
                    GuestId = "user1",
                    IsDeleted = false,

                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    NumberOfPhone = "123456",

                    VillaId = 1,
                    VillaPenthhouse = villa,

                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(1),

                    TotalPricePrice = 500
                },
                new Booking
                {
                    IdBooking = 2,
                    GuestId = "user2",
                    IsDeleted = false,

                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    NumberOfPhone = "123456",

                    VillaId = 1,
                    VillaPenthhouse = villa,

                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(1),

                    TotalPricePrice = 500
                }
            );

            dbContextMock.SaveChanges();

            var result = await service.GetAllReservations(userId);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result.First().IdBooking);
        }

        [Test]
        public async Task GetFavoritePlaces_ReturnsFavorites()
        {
            var userId = "user1";

            var user = new ApplicationUser { Id = userId };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            var location = new Location { IdLocation = 1, NameLocation = "Sofia" };

            dbContextMock.Locations.Add(location);

            var villa = new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Villa A",
                ImageUrl = "img.jpg",
                IsDeleted = false,

                IdPlace = 1,
                VillaInfo = "Nice villa",
                VillaAddress = "Sofia street",
                CountAdults = 2,
                CountChildren = 1,
                Bedrooms = 1,
                Bathrooms = 1,
                Area = "120",
                Parking = "Yes",
                LocationId = 1,
                Location = location,
                PricePerNight = 100
            };

            dbContextMock.VillasPenthhouses.Add(villa);

            dbContextMock.UserVilla.Add(new UserVilla
            {
                UserId = userId,
                VillaId = 1,
                Villa = villa
            });

            dbContextMock.SaveChanges();

            var result = await service.GetFavoritePlaces(userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Villa A", result.First().VilaName);
        }

        [Test]
        public async Task FavoritePlaces_AddsFavorite_ReturnsTrue()
        {
            var userId = "user1";

            var user = new ApplicationUser { Id = userId };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            var location = new Location { IdLocation = 1, NameLocation = "Sofia" };
            dbContextMock.Locations.Add(location);

            dbContextMock.VillasPenthhouses.Add(new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Villa A",
                IdPlace = 1,
                VillaInfo = "Test",
                VillaAddress = "Sofia",
                CountAdults = 2,
                CountChildren = 1,
                Bedrooms = 1,
                Bathrooms = 1,
                Area = "100",
                Parking = "Yes",
                LocationId = 1,
                Location = location,
                PricePerNight = 100,
                IsDeleted = false
            });

            dbContextMock.SaveChanges();

            var result = await service.FavoritePlaces(userId, 1);

            Assert.IsTrue(result);
            Assert.AreEqual(1, dbContextMock.UserVilla.Count());
        }

        //here11:26

        [Test]
        public async Task GetForDeleteReservation_ReturnsViewModel_WhenReservationExists()
        {
            var userId = "user1";

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(new ApplicationUser { Id = userId });

            var villa = new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Villa A",
                 Area = "100m3",
                Parking = "yes",
                VillaAddress = "Test Street 35 Lazur clousy",
                VillaInfo = "test Test"
            };

            dbContextMock.VillasPenthhouses.Add(villa);

            dbContextMock.Bookings.Add(new Booking
            {
                IdBooking = 1,
                GuestId = userId,
                FirstName = "John",
                LastName = "Doe",
                NumberOfPhone = "123",
                DateOfBirth = new DateTime(1990, 1, 1),
                VillaId = 1,
                VillaPenthhouse = villa,
                StartDate = new DateTime(2026, 1, 1),
                EndDate = new DateTime(2026, 1, 5),
                AdultsCount = 2,
                ChildrenCount = 1,
                IsDeleted = false
            });

            dbContextMock.SaveChanges();

            var result = await service.GetForDeleteReservation(1, userId);

            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.IdBooking.ToString());
            Assert.AreEqual("Villa A", result.HotelName);
        }

        [Test]
        public async Task DeleteReservation_SetsIsDeleted_AndReturnsTrue()
        {
            var userId = "user1";

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(new ApplicationUser { Id = userId });

            dbContextMock.Bookings.Add(new Booking
            {
                IdBooking = 1,
                IsDeleted = false,
                GuestId = userId,
                FirstName = "John",
                LastName = "Doe",
                NumberOfPhone = "123",
                DateOfBirth = new DateTime(1990, 1, 1),
                VillaId = 1
            });

            dbContextMock.SaveChanges();

            var result = await service.DeleteReservation(userId, 1);

            Assert.IsTrue(result);

            var booking = dbContextMock.Bookings.First();
            Assert.IsTrue(booking.IsDeleted);
        }

        [Test]
        public async Task GetForEditReservation_ReturnsEditModel_WhenExists()
        {
            var userId = "user1";

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(new ApplicationUser { Id = userId });

            var villa = new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Villa A",
                Area = "200m3",
                Parking = "yes",
                VillaAddress = "Test Street 17 Lazur synnny",
                VillaInfo = "test Test"

            };
            

            dbContextMock.VillasPenthhouses.Add(villa);

            dbContextMock.Bookings.Add(new Booking
            {
                IdBooking = 1,
                GuestId = userId,
                FirstName = "John",
                LastName = "Doe",
                NumberOfPhone = "123",
                DateOfBirth = new DateTime(1990, 1, 1),

                VillaId = 1,
                VillaPenthhouse = villa,

                StartDate = new DateTime(2026, 1, 1),
                EndDate = new DateTime(2026, 1, 5),
                AdultsCount = 2,
                ChildrenCount = 1
            });

            dbContextMock.SaveChanges();

            var result = await service.GetForEditReservation(1, userId);

            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.IdBooking);
            Assert.AreEqual("Villa A", result.VilaName);
            Assert.AreEqual("John", result.GuestFirstName);
        }
        [Test]
        public async Task EditReservation_UpdatesBooking_ReturnsTrue()
        {
            var userId = "user1";

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(new ApplicationUser { Id = userId });

            var booking = new Booking
            {
                IdBooking = 1,
                GuestId = userId,
                FirstName = "Old",
                LastName = "Name",
                NumberOfPhone = "111",
                DateOfBirth = new DateTime(1990, 1, 1),

                StartDate = new DateTime(2026, 1, 1),
                EndDate = new DateTime(2026, 1, 5),

                AdultsCount = 1,
                ChildrenCount = 0,
                VillaId = 1
            };

            dbContextMock.Bookings.Add(booking);
            dbContextMock.SaveChanges();

            var editModel = new EditBooking
            {
                IdBooking = "1",
                StartDate = "2026-02-01",
                EndDate = "2026-02-05",
                AdultsCount = 3,
                ChildrenCount = 2,
                GuestFirstName = "New",
                LastNameG = "User",
                DateofBirth = "1995-01-01",
                GuestAddress = "Addr",
                GuestEmail = "test@test.com",
                GuestPhoneNumber = "999"
            };

            var result = await service.EditReservation(userId, editModel);

            Assert.IsTrue(result);

            var updated = dbContextMock.Bookings.First();

            Assert.AreEqual("New", updated.FirstName);
            Assert.AreEqual(3, updated.AdultsCount);
            Assert.AreEqual(2, updated.ChildrenCount);
        }

        //11:37
        [Test]
        public async Task GetVilaDetailsAsync_ReturnsVillaDetails_WhenExists()
        {
            var userId = "user1";

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(new ApplicationUser { Id = userId });

            var location = new Location { IdLocation = 1, NameLocation = "Sofia" };
            var type = new TypePlace { IdTypePlace = 1, NamePlace = "Luxury" };

            dbContextMock.Locations.Add(location);
            dbContextMock.TypePlaces.Add(type);

            dbContextMock.VillasPenthhouses.Add(new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Villa A",
                VillaInfo = "Info",
                VillaAddress = "Addr",
                ImageUrl = "img.jpg",
                CountAdults = 2,
                CountChildren = 1,
                CountRooms = 3,
                Bedrooms = 2,
                Bathrooms = 1,
                Parking = "Yes",
                Area = "120",
                LocationId = 1,
                Location = location,
                TypePlace = type,
                IDManager = userId
            });

            dbContextMock.SaveChanges();

            var result = await service.GetVilaDetailsAsync(1, userId);

            Assert.IsNotNull(result);
            Assert.AreEqual("Villa A", result.VilaName);
            Assert.AreEqual("Sofia", result.TownName);
            Assert.AreEqual("Luxury", result.TypePlace);
            Assert.IsTrue(result.IsManager);
        }

        [Test]
        public async Task LeaveFeedBack_AddsFeedback_ReturnsTrue()
        {
            var userId = "user1";

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(new ApplicationUser
                {
                    Id = userId,
                    UserName = "John"
                });

            dbContextMock.VillasPenthhouses.Add(new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Villa A",
                VillaInfo = "Info",
                VillaAddress = "Addr",
                CountAdults = 2,
                CountChildren = 1,
                Bedrooms = 1,
                Bathrooms = 1,
                Area = "100",
                Parking = "Yes",
                LocationId = 1
            });

            dbContextMock.Bookings.Add(new Booking
            {
                IdBooking = 1,
                VillaId = 1,
                GuestId = userId,
                FirstName = "John",
                LastName = "Doe",
                NumberOfPhone = "123",
                DateOfBirth = new DateTime(1990, 1, 1)
            });

            dbContextMock.SaveChanges();

            var model = new BookingFeedbackViewModel
            {
                IdBooking = 1,
                VillaId = 1,
                FeedbackMessage = "Great place!",
                Rating = 5
            };

            var result = await service.LeaveFeedBack(userId, model);

            Assert.IsTrue(result);
            Assert.AreEqual(1, dbContextMock.FeedBacks.Count());
        }

        [Test]
        public async Task GetAllFeedbacks_ReturnsAllFeedbacks()
        {
            var user = new ApplicationUser
            {
                Id = "user1",
                UserName = "John"
            };

            var villa = new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Villa A",
                VillaInfo = "Info",
                VillaAddress = "Addr",
                CountAdults = 2,
                CountChildren = 1,
                Bedrooms = 1,
                Bathrooms = 1,
                Area = "100",
                Parking = "Yes",
                LocationId = 1
            };

            dbContextMock.VillasPenthhouses.Add(villa);

            dbContextMock.FeedBacks.Add(new FeedBack
            {
                BookingId = 1,
                VillaId = 1,
                FeedbackMessage = "Nice!",
                Rating = 5,
                Villa = villa,
                Guest = user
            });

            dbContextMock.SaveChanges();

            var result = await service.GetAllFeedbacks(null);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Villa A", result.First().VillaName);
            Assert.AreEqual("John", result.First().ClientName);
        }

        [Test]
        public async Task GetForEditVila_ReturnsEditModel_WhenExists()
        {
            var userId = "user1";

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(new ApplicationUser { Id = userId });

            var location = new Location { IdLocation = 1, NameLocation = "Sofia" };
            var type = new TypePlace { IdTypePlace = 1, NamePlace = "Luxury" };

            dbContextMock.Locations.Add(location);
            dbContextMock.TypePlaces.Add(type);

            dbContextMock.VillasPenthhouses.Add(new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Villa A",
                VillaInfo = "Info",
                VillaAddress = "Addr",
                ImageUrl = "img.jpg",
                CountRooms = 3,
                CountAdults = 2,
                CountChildren = 1,
                Bedrooms = 2,
                Bathrooms = 1,
                Area = "120",
                Parking = "Yes",
                LocationId = 1,
                Location = location,
                TypePlace = type,
                IDManager = userId
            });

            dbContextMock.SaveChanges();

            var result = await service.GetForEditVila(1, userId);

            Assert.IsNotNull(result);
            Assert.AreEqual("Villa A", result.NameVilla);
            Assert.AreEqual("Sofia", result.LocationName);
            Assert.AreEqual("Luxury", result.NamePlace);
        }

        [Test]
        public async Task EditVilla_UpdatesVilla_ReturnsTrue()
        {
            var userId = "user1";

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(new ApplicationUser { Id = userId });

            dbContextMock.VillasPenthhouses.Add(new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Old Villa",
                VillaInfo = "Old",
                VillaAddress = "Old",
                ImageUrl = "old.jpg",
                CountRooms = 1,
                CountAdults = 1,
                CountChildren = 0,
                Bedrooms = 1,
                Bathrooms = 1,
                Area = "100",
                Parking = "No",
                LocationId = 1
            });

            dbContextMock.SaveChanges();

            var model = new EditVilaViewModel
            {
                IdVilla = 1,
                NameVilla = "New Villa",
                IdTypePlace = 1,
                VillaInfo = "New Info",
                VillaAddress = "New Addr",
                ImageUrl = "new.jpg",
                CountRooms = 3,
                CountAdults = 2,
                CountChildren = 1,
                Bedrooms = 2,
                Bathrooms = 2,
                Area = "150",
                Parking = "Yes",
                IdTown = 1
            };

            var result = await service.EditVilla(userId, model);

            Assert.IsTrue(result);

            var updated = dbContextMock.VillasPenthhouses.First();

            Assert.AreEqual("New Villa", updated.NameVilla);
            Assert.AreEqual("New Info", updated.VillaInfo);
        }
        //12:19
        [Test]
        public async Task GetVilaDetailsAsync_ReturnsCorrectDetails()
        {
            // Arrange
            var userId = "user1";

            userManagerMock
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(new ApplicationUser { Id = userId });

            var villa = new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Villa A",
                CountAdults = 2,
                CountChildren = 1,
                CountRooms = 3,
                Bedrooms = 2,
                Bathrooms = 1,
                Parking = "Yes",
                ImageUrl = "img.jpg",
                VillaInfo = "Nice",
                IDManager = userId,
                Location = new Location { NameLocation = "Sofia" },
                TypePlace = new TypePlace { NamePlace = "Villa" },
                Area = "170m3",
                VillaAddress = "Test Lazur",
               
            };

            dbContextMock.VillasPenthhouses.Add(villa);
            dbContextMock.SaveChanges();

            // Act
            var result = await service.GetVilaDetailsAsync(1, userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Villa A", result.VilaName);
            Assert.AreEqual("Sofia", result.TownName);
            Assert.AreEqual("Villa", result.TypePlace);
            Assert.IsTrue(result.IsManager);
        }
        [Test]
        public async Task GetVilaDetailsAsync_ReturnsNull_WhenVillaNotFound()
        {
            userManagerMock
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            var result = await service.GetVilaDetailsAsync(999, "user1");

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllVillasSearch_ReturnsOnlyAvailableVillas()
        {
            // Arrange
            var start = "2026-01-01";
            var end = "2026-01-10";

            var villa1 = new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Free Villa",
                Location = new Location { NameLocation = "Sofia" },
                TypePlace = new TypePlace { NamePlace = "Villa" },
                AllBookings = new List<Booking>(), // ? no bookings
                 Area = "190m3",
                Parking = "no",
                VillaAddress = "Test Lazur",
                VillaInfo = "Test Test"
            };

            var villa2 = new VillaPenthhouse
            {
                IdVilla = 2,
                NameVilla = "Booked Villa",
                Location = new Location { NameLocation = "Plovdiv" },
                TypePlace = new TypePlace { NamePlace = "Villa" },
                AllBookings = new List<Booking>
        {
            new Booking
            {
                StartDate = new DateTime(2026, 1, 2),
                EndDate = new DateTime(2026, 1, 5),
               FirstName="Natali",
               GuestId="4333445",
               LastName="Jonson",
               NumberOfPhone="33445667777"
            }
        },
                Area="230m3",
                Parking="no",   
                VillaAddress="Latinka 75",
                VillaInfo="data info"


            };

            dbContextMock.VillasPenthhouses.AddRange(villa1, villa2);
            dbContextMock.SaveChanges();

            // Act
            var result = await service.GetAllVillasSearch(null, start, end, 1, 10);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual("Free Villa", result.Items.First().NameVilla);
        }
        [Test]
        public async Task GetAllVillasSearch_PaginatesCorrectly()
        {
            for (int i = 1; i <= 5; i++)
            {
                dbContextMock.VillasPenthhouses.Add(new VillaPenthhouse
                {
                    IdVilla = i,
                    NameVilla = "Villa " + i,
                    Location = new Location { NameLocation = "City" },
                    TypePlace = new TypePlace { NamePlace = "Type" },
                    AllBookings = new List<Booking>(),

                    Area="170m3",
                    Parking="yes",
                    VillaAddress="Test Lazur",
                    VillaInfo="Test Test"
                });
            }

            dbContextMock.SaveChanges();

            var result = await service.GetAllVillasSearch(null, "2026-01-01", "2026-01-10", 2, 2);

            Assert.AreEqual(2, result.Items.Count());
            Assert.AreEqual(3, result.TotalPages); // 5 items / page size 2
        }

    }
}