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
                Id = 1,
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
        public void Test1()
        {
            Assert.Pass();
        }
    }
}