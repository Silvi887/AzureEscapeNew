using AzureAdd.Data;
using AzureAdd.DataModels;
using AzureApp.ViewModels;
using AzureServises.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;


using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace EscapeApp.Services.Tests
{
    public class Tests
    {
        private Mock<AzureAddDbContext> dbContextMock;
        private Mock<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>> userManagerMock;
        private VillaService service;
        private Mock<DbSet<VillaPenthhouse>> villaDbSetMock;
        Mock<DbSet<Booking>> bookingsMock;

        [SetUp]
        public void Setup()
        {
            dbContextMock = new Mock<AzureAddDbContext>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            userManagerMock = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

            villaDbSetMock = new Mock<DbSet<VillaPenthhouse>>();

            dbContextMock.Setup(x => x.VillasPenthhouses)
                .Returns(villaDbSetMock.Object);

            service = new VillaService(dbContextMock.Object, userManagerMock.Object);

          
        }
        private AzureAddDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AzureAddDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            return new AzureAddDbContext(options);
        }

        [Test]
        public async Task AddVilaModel_UserExists_ReturnsTrue()
        {
            // Arrange
            var userId = "user-1";

            var user = new ApplicationUser { Id = userId };

            userManagerMock.Setup(x => x.Users)
                .Returns(new List<ApplicationUser> { user }.AsQueryable());

            AddVillaIndexViewModel model = new AddVillaIndexViewModel
            {
                NameVilla = "Test Villa",
                IdTypePlace = 1,
                VillaInfo = "Info",
                VillaAddress = "Address",
                ImageUrl = "img.jpg",
                CountRooms = 3,
                CountAdults = 2,
                CountChildren = 1,
                Bedrooms = 2,
                Bathrooms = 1,
                Area = "120",
                Parking = "yes",
                IdTown = 5
            };

            villaDbSetMock.Setup(x => x.Add(It.IsAny<VillaPenthhouse>()));

            dbContextMock.Setup(x => x.SaveChanges())
                .Returns(1);

            // Act
            var result = await service.AddVilaModel(userId, model);

            // Assert
            Assert.IsTrue(result);

            villaDbSetMock.Verify(x => x.Add(It.IsAny<VillaPenthhouse>()), Times.Once);
            dbContextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public async Task AddVilaModel_UserNotFound_ReturnsFalse()
        {
            // Arrange
            var userId = "missing-user";

            userManagerMock.Setup(x => x.Users)
                .Returns(new List<ApplicationUser>().AsQueryable());

            var model = new AddVillaIndexViewModel();

            // Act
            var result = await service.AddVilaModel(userId, model);

            // Assert
            Assert.IsFalse(result);

            villaDbSetMock.Verify(x => x.Add(It.IsAny<VillaPenthhouse>()), Times.Never);
            dbContextMock.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Test]
        public void AddVilaModel_WhenException_Throws()
        {
            // Arrange
            var userId = "user-1";

            userManagerMock.Setup(x => x.Users)
                .Throws(new System.Exception("DB failure"));

            var model = new AddVillaIndexViewModel();

            // Act + Assert
            Assert.ThrowsAsync<System.Exception>(async () =>
            {
                await service.AddVilaModel(userId, model);
            });
        }

        [Test]
        public async Task AddVilaModel_MapsModelCorrectly()
        {
            // Arrange
            var userId = "user-1";
            var user = new ApplicationUser { Id = userId };

            userManagerMock.Setup(x => x.Users)
                .Returns(new List<ApplicationUser> { user }.AsQueryable());

            AddVillaIndexViewModel model = new AddVillaIndexViewModel
            {
                NameVilla = "Luxury Villa",
                IdTypePlace = 2,
                VillaAddress = "Sofia",
                VillaInfo = "Nice villa"
            };

            VillaPenthhouse capturedVilla = null;

            villaDbSetMock.Setup(x => x.Add(It.IsAny<VillaPenthhouse>()))
                .Callback<VillaPenthhouse>(v => capturedVilla = v);

            dbContextMock.Setup(x => x.SaveChanges()).Returns(1);

            // Act
            await service.AddVilaModel(userId, model);

            // Assert
            Assert.IsNotNull(capturedVilla);
            Assert.AreEqual("Luxury Villa", capturedVilla.NameVilla);
            Assert.AreEqual(userId, capturedVilla.IDManager);
            Assert.AreEqual("Sofia", capturedVilla.VillaAddress);
        }


        //allreservations
        [Test]
        public async Task GetAllReservations_Admin_ReturnsAllBookings()
        {
            // Arrange
            var userId = "admin-id";

            var user = new ApplicationUser { Id = userId };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(true);

            var data = new List<Booking>
    {
        new Booking
        {
            IdBooking = 1,
            GuestId = "user1",
            IsDeleted = false,
            StartDate = System.DateTime.Now,
            EndDate = System.DateTime.Now.AddDays(1),
            VillaPenthhouse = new VillaPenthhouse
            {
                NameVilla = "Villa A"
            }
        },
        new Booking
        {
            IdBooking = 2,
            GuestId = "user2",
            IsDeleted = false,
            VillaPenthhouse = new VillaPenthhouse
            {
                NameVilla = "Villa B"
            }
        }
    }.AsQueryable();

            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.Provider)
                .Returns(data.Provider);
            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.Expression)
                .Returns(data.Expression);
            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.ElementType)
                .Returns(data.ElementType);
            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.GetEnumerator())
                .Returns(data.GetEnumerator());

            // Act
            var result = await service.GetAllReservations(userId);

            // Assert
            Assert.AreEqual(2, result.Count());
        }


        [Test]
        public async Task GetAllReservations_NormalUser_ReturnsOnlyOwnBookings()
        {
            // Arrange
            var userId = "user1";

            var user = new ApplicationUser { Id = userId };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(false);

            var data = new List<Booking>
    {
        new Booking
        {
            IdBooking = 1,
            GuestId = "user1",
            IsDeleted = false,
            VillaPenthhouse = new VillaPenthhouse { NameVilla = "Villa A" }
        },
        new Booking
        {
            IdBooking = 2,
            GuestId = "user2",
            IsDeleted = false,
            VillaPenthhouse = new VillaPenthhouse { NameVilla = "Villa B" }
        }
    }.AsQueryable();

            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.Provider).Returns(data.Provider);
            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.Expression).Returns(data.Expression);
            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.ElementType).Returns(data.ElementType);
            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            // Act
            var result = await service.GetAllReservations(userId);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("user1", result.First().IsUserGuest ? "user1" : null);
        }

        [Test]
        public async Task GetAllReservations_UserNull_DoesNotThrow()
        {
            // Arrange
            string userId = "missing";

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync((ApplicationUser)null);

            userManagerMock.Setup(x => x.IsInRoleAsync(null, "Admin"))
                .ReturnsAsync(false);

            var data = new List<Booking>().AsQueryable();

            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.Provider).Returns(data.Provider);
            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.Expression).Returns(data.Expression);
            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.ElementType).Returns(data.ElementType);
            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            // Act
            var result = await service.GetAllReservations(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task GetAllReservations_FiltersDeletedBookings()
        {
            // Arrange
            var userId = "admin-id";

            var user = new ApplicationUser { Id = userId };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin"))
                .ReturnsAsync(true);

            var data = new List<Booking>
    {
        new Booking { IdBooking = 1, IsDeleted = false },
        new Booking { IdBooking = 2, IsDeleted = true }
    }.AsQueryable();

            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.Provider).Returns(data.Provider);
            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.Expression).Returns(data.Expression);
            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.ElementType).Returns(data.ElementType);
            bookingsMock.As<IQueryable<Booking>>()
                .Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            // Act
            var result = await service.GetAllReservations(userId);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        //Favorite Villa
        [Test]
        public async Task GetFavoritePlaces_UserExists_ReturnsFavorites()
        {
            // Arrange
            var context = CreateDbContext();

            var userId = "user1";

            var user = new ApplicationUser { Id = userId };

            var villa = new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Villa A",
                ImageUrl = "img.jpg",
                IsDeleted = false,
                Location = new Location { NameLocation = "Sofia" }
            };

            context.UserVilla.Add(new UserVilla
            {
                UserId = userId,
                VillaId = 1,
                Villa = villa
            });

            context.SaveChanges();

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            var service = new VillaService(context, userManagerMock.Object);

            // Act
            var result = await service.GetFavoritePlaces(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Villa A", result.First().VilaName);
        }

        [Test]
        public async Task GetFavoritePlaces_UserNotFound_ReturnsNull()
        {
            // Arrange
            var context = CreateDbContext();

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            userManagerMock.Setup(x => x.FindByIdAsync("missing"))
                .ReturnsAsync((ApplicationUser)null);

            var service = new VillaService(context, userManagerMock.Object);

            // Act
            var result = await service.GetFavoritePlaces("missing");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetFavoritePlaces_ExcludesDeletedVillas()
        {
            // Arrange
            var context = CreateDbContext();

            var userId = "user1";

            var villa = new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Hidden Villa",
                IsDeleted = true,
                Location = new Location { NameLocation = "Sofia" }
            };

            context.UserVilla.Add(new UserVilla
            {
                UserId = userId,
                VillaId = 1,
                Villa = villa
            });

            context.SaveChanges();

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(new ApplicationUser { Id = userId });

            var service = new VillaService(context, userManagerMock.Object);

            // Act
            var result = await service.GetFavoritePlaces(userId);

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task FavoritePlaces_UserAndVillaExist_AddsFavorite_ReturnsTrue()
        {
            // Arrange
            var context = CreateDbContext();

            var userId = "user1";

            var user = new ApplicationUser { Id = userId };

            var villa = new VillaPenthhouse
            {
                IdVilla = 1
            };

            context.VillasPenthhouses.Add(villa);
            context.SaveChanges();

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            var service = new VillaService(context, userManagerMock.Object);

            // Act
            var result = await service.FavoritePlaces(userId, 1);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, context.UserVilla.Count());
        }

        [Test]
        public async Task FavoritePlaces_WhenAlreadyExists_DoesNotDuplicate_ReturnsTrue()
        {
            // Arrange
            var context = CreateDbContext();

            var userId = "user1";

            var user = new ApplicationUser { Id = userId };

            var villa = new VillaPenthhouse { IdVilla = 1 };

            context.VillasPenthhouses.Add(villa);

            context.UserVilla.Add(new UserVilla
            {
                UserId = userId,
                VillaId = 1
            });

            context.SaveChanges();

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            var service = new VillaService(context, userManagerMock.Object);

            // Act
            var result = await service.FavoritePlaces(userId, 1);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, context.UserVilla.Count()); // still 1, no duplicate
        }

        [Test]
        public async Task FavoritePlaces_UserNotFound_ReturnsFalse()
        {
            // Arrange
            var context = CreateDbContext();

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            userManagerMock.Setup(x => x.FindByIdAsync("missing"))
                .ReturnsAsync((ApplicationUser)null);

            var service = new VillaService(context, userManagerMock.Object);

            // Act
            var result = await service.FavoritePlaces("missing", 1);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task FavoritePlaces_VillaNotFound_ReturnsFalse()
        {
            // Arrange
            var context = CreateDbContext();

            var userId = "user1";

            var user = new ApplicationUser { Id = userId };

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            var service = new VillaService(context, userManagerMock.Object);

            // Act
            var result = await service.FavoritePlaces(userId, 999);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveFavorite_ExistingFavorite_RemovesAndReturnsTrue()
        {
            // Arrange
            var context = CreateDbContext();

            var userId = "user1";

            var user = new ApplicationUser { Id = userId };

            context.UserVilla.Add(new UserVilla
            {
                UserId = userId,
                VillaId = 1
            });

            context.SaveChanges();

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            var service = new VillaService(context, userManagerMock.Object);

            // Act
            var result = await service.RemoveFavorite(userId, 1);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, context.UserVilla.Count());
        }

        [Test]
        public async Task RemoveFavorite_NotExisting_ReturnsFalse()
        {
            // Arrange
            var context = CreateDbContext();

            var userId = "user1";

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(new ApplicationUser { Id = userId });

            var service = new VillaService(context, userManagerMock.Object);

            // Act
            var result = await service.RemoveFavorite(userId, 1);

            // Assert
            Assert.IsFalse(result);
        }


        [Test]
        public async Task RemoveFavorite_UserNull_ReturnsFalse()
        {
            // Arrange
            var context = CreateDbContext();

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            userManagerMock.Setup(x => x.FindByIdAsync("missing"))
                .ReturnsAsync((ApplicationUser)null);

            var service = new VillaService(context, userManagerMock.Object);

            // Act
            var result = await service.RemoveFavorite("missing", 1);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteReservation_ReturnsTrue_WhenReservationExists()
        {
            // Arrange


            var options = new DbContextOptionsBuilder<AzureAddDbContext>()
                .UseInMemoryDatabase("DeleteTestDb")
                .Options;

            var context = new AzureAddDbContext(options);

            var user = new ApplicationUser { Id = "user1" };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(x => x.FindByIdAsync("user1"))
                .ReturnsAsync(user);

            var reservation = new Booking
            {
                IdBooking = 1,
                IsDeleted = false
            };

            context.Bookings.Add(reservation);
            context.SaveChanges();

            var service = new VillaService(context, userManager.Object);

            // Act
            var result = await service.DeleteReservation("user1", 1);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(context.Bookings.First().IsDeleted);
        }


        [Test]
        public async Task DeleteReservation_ReturnsFalse_WhenUserNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AzureAddDbContext>()
                .UseInMemoryDatabase("DeleteTestDb2")
                .Options;

            var context = new AzureAddDbContext(options);

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(x => x.FindByIdAsync("badUser"))
                .ReturnsAsync((ApplicationUser)null);

            var reservation = new Booking
            {
                IdBooking = 1,
                IsDeleted = false
            };

            context.Bookings.Add(reservation);
            context.SaveChanges();

            var service = new VillaService(context, userManager.Object);

            // Act
            var result = await service.DeleteReservation("badUser", 1);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteReservation_ReturnsFalse_WhenReservationNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AzureAddDbContext>()
                .UseInMemoryDatabase("DeleteTestDb3")
                .Options;

            var context = new AzureAddDbContext(options);

            var user = new ApplicationUser { Id = "user1" };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(x => x.FindByIdAsync("user1"))
                .ReturnsAsync(user);

            var service = new VillaService(context, userManager.Object);

            // Act
            var result = await service.DeleteReservation("user1", 999);

            // Assert
            Assert.IsFalse(result);
        }


        [Test]
        public async Task GetForEditReservation_ReturnsEditBooking_WhenDataExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AzureAddDbContext>()
                .UseInMemoryDatabase("EditGetTestDb")
                .Options;

            var context = new AzureAddDbContext(options);

            var villa = new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Luxury Villa"
            };

            context.VillasPenthhouses.Add(villa);

            context.Bookings.Add(new Booking
            {
                IdBooking = 1,
                StartDate = new DateTime(2026, 01, 01),
                EndDate = new DateTime(2026, 01, 05),
                AdultsCount = 2,
                ChildrenCount = 1,
                VillaId = 1,
                VillaPenthhouse = villa,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 01, 01),
                Address = "Sofia",
                Email = "john@test.com",
                NumberOfPhone = "12345"
            });

            context.SaveChanges();

            var user = new ApplicationUser { Id = "user1" };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(x => x.FindByIdAsync("user1"))
                .ReturnsAsync(user);

            var service = new VillaService(context, userManager.Object);

            // Act
            var result = await service.GetForEditReservation(1, "user1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.IdBooking);
            Assert.AreEqual("Luxury Villa", result.VilaName);
            Assert.AreEqual("John", result.GuestFirstName);
        }


        [Test]
        public async Task GetForEditReservation_ReturnsNull_WhenUserNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AzureAddDbContext>()
                .UseInMemoryDatabase("EditGetTestDb2")
                .Options;

            var context = new AzureAddDbContext(options);

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(x => x.FindByIdAsync("badUser"))
                .ReturnsAsync((ApplicationUser)null);

            var service = new VillaService(context, userManager.Object);

            // Act
            var result = await service.GetForEditReservation(1, "badUser");

            // Assert
            Assert.IsNull(result);
        }


        [Test]
        public async Task EditReservation_ReturnsTrue_WhenUpdateIsSuccessful()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AzureAddDbContext>()
                .UseInMemoryDatabase("EditUpdateTestDb")
                .Options;

            var context = new AzureAddDbContext(options);

            var villa = new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Villa"
            };

            context.VillasPenthhouses.Add(villa);

            context.Bookings.Add(new Booking
            {
                IdBooking = 1,
                StartDate = new DateTime(2026, 01, 01),
                EndDate = new DateTime(2026, 01, 05),
                AdultsCount = 2,
                ChildrenCount = 1,
                VillaId = 1,
                VillaPenthhouse = villa,
                FirstName = "Old",
                LastName = "Name",
                DateOfBirth = new DateTime(1990, 01, 01),
                Address = "Old",
                Email = "old@test.com",
                NumberOfPhone = "000"
            });

            context.SaveChanges();

            var user = new ApplicationUser { Id = "user1" };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(x => x.FindByIdAsync("user1"))
                .ReturnsAsync(user);

            var service = new VillaService(context, userManager.Object);

            var model = new EditBooking
            {
                IdBooking = "1",
                StartDate = "2026-02-01",
                EndDate = "2026-02-05",
                AdultsCount = 3,
                ChildrenCount = 2,
                GuestFirstName = "New",
                LastNameG = "User",
                DateofBirth = "1995-01-01",
                GuestAddress = "New Address",
                GuestEmail = "new@test.com",
                GuestPhoneNumber = "999"
            };

            // Act
            var result = await service.EditReservation("user1", model);

            // Assert
            Assert.IsTrue(result);

            var updated = context.Bookings.First();

            Assert.AreEqual("New", updated.FirstName);
            Assert.AreEqual(3, updated.AdultsCount);
        }

        [Test]
        public async Task GetVilaDetailsAsync_ReturnsDetails_WhenVillaExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AzureAddDbContext>()
                .UseInMemoryDatabase("VillaDetailsDb")
                .Options;

            var context = new AzureAddDbContext(options);

            var location = new Location { IdLocation = 1, NameLocation = "Sofia" };
            var type = new TypePlace { IdTypePlace = 1, NamePlace = "Luxury" };

            context.Locations.Add(location);
            context.TypePlaces.Add(type);

            var villa = new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Sea View",
                CountChildren = 2,
                CountAdults = 4,
                CountRooms = 3,
                Bedrooms = 2,
                Bathrooms = 2,
                Parking = "yes",
                ImageUrl = "img.jpg",
                VillaInfo = "Nice villa",
                Location = location,
                TypePlace = type,
                IDManager = "manager1"
            };

            context.VillasPenthhouses.Add(villa);
            context.SaveChanges();

            var manager = new ApplicationUser { Id = "manager1" };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(x => x.FindByIdAsync("manager1"))
                .ReturnsAsync(manager);

            var service = new VillaService(context, userManager.Object);

            // Act
            var result = await service.GetVilaDetailsAsync(1, "manager1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Sea View", result.VilaName);
            Assert.AreEqual("Sofia", result.TownName);
            Assert.AreEqual("Luxury", result.TypePlace);
            Assert.IsTrue(result.IsManager);
        }

        [Test]
        public async Task GetVilaDetailsAsync_ReturnsNull_WhenVillaNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AzureAddDbContext>()
                .UseInMemoryDatabase("VillaDetailsDb2")
                .Options;

            var context = new AzureAddDbContext(options);

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(x => x.FindByIdAsync("user1"))
                .ReturnsAsync(new ApplicationUser { Id = "user1" });

            var service = new VillaService(context, userManager.Object);

            // Act
            var result = await service.GetVilaDetailsAsync(999, "user1");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetVilaDetailsAsync_IsManagerFalse_WhenUserNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AzureAddDbContext>()
                .UseInMemoryDatabase("VillaDetailsDb3")
                .Options;

            var context = new AzureAddDbContext(options);

            var location = new Location { IdLocation = 1, NameLocation = "Sofia" };
            var type = new TypePlace { IdTypePlace = 1, NamePlace = "Luxury" };

            context.Locations.Add(location);
            context.TypePlaces.Add(type);

            var villa = new VillaPenthhouse
            {
                IdVilla = 1,
                NameVilla = "Sea View",
                Location = location,
                TypePlace = type,
                IDManager = "manager1"
            };

            context.VillasPenthhouses.Add(villa);
            context.SaveChanges();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            userManager.Setup(x => x.FindByIdAsync("badUser"))
                .ReturnsAsync((ApplicationUser)null);

            var service = new VillaService(context, userManager.Object);

            // Act
            var result = await service.GetVilaDetailsAsync(1, "badUser");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsManager);
        }


       
    }
}