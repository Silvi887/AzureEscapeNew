using AzureAdd.DataModels;
using AzureApp.ViewModels;
using AzureEscape.Controllers;
using AzureServises.Core;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Moq;
using System.Security.Claims;

namespace EscapeApp.Web.Tests;

public class ReserveControllerTets
{
    private Mock<IVilla> vilaServiceMock;
    private ReserveController controller;
    private Mock<UserManager<ApplicationUser>> userManagerMock;

    [SetUp]
    public void Setup()
    {
        vilaServiceMock = new Mock<IVilla>();

        controller = new ReserveController(
            vilaServiceMock.Object,
            userManagerMock.Object
            );

        // Mock logged user
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id")
        }));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
        var store = new Mock<IUserStore<ApplicationUser>>();
        userManagerMock = new Mock<UserManager<ApplicationUser>>(
           store.Object, null, null, null, null, null, null, null, null
       );

        controller = new ReserveController(
            vilaServiceMock.Object,
            userManagerMock.Object
        );
    }

    [TearDown]
    public void TearDown()
    {
        controller?.Dispose();
    }


    [Test]
    public async Task Add_InvalidModelState_ReturnsView()
    {
        // Arrange
        controller.ModelState.AddModelError("error", "invalid");

        var model = new AddReservationViewModel();

        // Act
        var result = await controller.Add(model);

        // Assert
        var viewResult = result as ViewResult;

        Assert.IsNotNull(viewResult);
        Assert.AreEqual("Views/Vila/AddReservation.cshtml", viewResult.ViewName);
        Assert.IsInstanceOf<AddReservationViewModel>(viewResult.Model);
        Assert.IsTrue(controller.ModelState.ErrorCount > 0);
    }

    [Test]
    public async Task Add_ServiceReturnsFalse_RedirectsToAddBooking()
    {
        // Arrange
        var model = new AddReservationViewModel();

        vilaServiceMock
            .Setup(x => x.AddBookingModel(It.IsAny<string>(), model))
            .ReturnsAsync(false);

        // Act
        var result = await controller.Add(model);

        // Assert
        var redirect = result as RedirectToActionResult;

        Assert.IsNotNull(redirect);
        Assert.AreEqual("AddBooking", redirect.ActionName);
        Assert.AreEqual("Reserve", redirect.ControllerName);
    }

    [Test]
    public async Task Add_ValidModel_ReturnsOk()
    {
        // Arrange
        var model = new AddReservationViewModel();

        vilaServiceMock
            .Setup(x => x.AddBookingModel(It.IsAny<string>(), model))
            .ReturnsAsync(true);

        // Act
        var result = await controller.Add(model);

        // Assert
        Assert.IsInstanceOf<OkResult>(result);
    }
    [Test]
    public async Task Add_WhenException_RedirectsToError()
    {
        // Arrange
        var model = new AddReservationViewModel();

        vilaServiceMock
            .Setup(x => x.AddBookingModel(It.IsAny<string>(), model))
            .ThrowsAsync(new System.Exception("DB error"));

        // Act
        var result = await controller.Add(model);

        // Assert
        var redirect = result as RedirectToActionResult;

        Assert.IsNotNull(redirect);
        Assert.AreEqual("Error", redirect.ActionName);
        Assert.AreEqual("Home", redirect.ControllerName);
    }

    [Test]
    public async Task Add_CallsServiceWithCorrectParameters()
    {
        // Arrange
        var model = new AddReservationViewModel();
        var userId = "test-user-id";

        vilaServiceMock
            .Setup(x => x.AddBookingModel(userId, model))
            .ReturnsAsync(true);

        // Act
        await controller.Add(model);

        // Assert
        vilaServiceMock.Verify(
            x => x.AddBookingModel(userId, model),
            Times.Once
        );
    }

    //all reservations
    [Test]
    public async Task AllReservations_ReturnsView_WithModel()
    {
        // Arrange
        var userId = "test-user-id";

        var reservations = new List<AllReservationsViewModel>
    {
        new AllReservationsViewModel()
    };

        vilaServiceMock
            .Setup(x => x.GetAllReservations(userId))
            .ReturnsAsync(reservations);

        userManagerMock
            .Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(new ApplicationUser
            {
                EmailConfirmed = true
            });

        // Act
        var result = await controller.AllReservations(null);

        // Assert
        var viewResult = result as ViewResult;

        Assert.IsNotNull(viewResult);
        Assert.AreEqual("Views/Vila/AllReservations.cshtml", viewResult.ViewName);
        Assert.AreEqual(reservations, viewResult.Model);

        Assert.IsTrue((bool)controller.ViewBag.EmailConfirmed);
    }

    [Test]
    public async Task AllReservations_WhenException_RedirectsToError()
    {
        // Arrange
        vilaServiceMock
            .Setup(x => x.GetAllReservations(It.IsAny<string>()))
            .ThrowsAsync(new System.Exception("DB error"));

        // Act
        var result = await controller.AllReservations(null);

        // Assert
        var redirect = result as RedirectToActionResult;

        Assert.IsNotNull(redirect);
        Assert.AreEqual("Error", redirect.ActionName);
        Assert.AreEqual("Home", redirect.ControllerName);
    }

    [Test]
    public async Task AllReservations_WhenUserNull_EmailConfirmedIsFalse()
    {
        // Arrange
        var userId = "test-user-id";

        vilaServiceMock
            .Setup(x => x.GetAllReservations(userId))
            .ReturnsAsync(new List<AllReservationsViewModel>());

        userManagerMock
            .Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync((ApplicationUser)null);

        // Act
        var result = await controller.AllReservations(null);

        // Assert
        var viewResult = result as ViewResult;

        Assert.IsNotNull(viewResult);
        Assert.IsFalse((bool)controller.ViewBag.EmailConfirmed);
    }

    [Test]
    public async Task AllReservations_CallsServiceWithCorrectUserId()
    {
        // Arrange
        var userId = "test-user-id";

        vilaServiceMock
            .Setup(x => x.GetAllReservations(userId))
            .ReturnsAsync(new List<AllReservationsViewModel>());

        userManagerMock
            .Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(new ApplicationUser());

        // Act
        await controller.AllReservations(null);

        // Assert
        vilaServiceMock.Verify(
            x => x.GetAllReservations(userId),
            Times.Once
        );
    }
    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}
