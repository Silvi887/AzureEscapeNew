using AzureAdd.DataModels;
using AzureApp.ViewModels;
using AzureEscape.Controllers;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace EscapeApp.Web.Tests;

[TestFixture]
public class VilaControllerTests
{

    private Mock<IVilla> vilaServiceMock;
    private Mock<ITownService> townServiceMock;
    private Mock<UserManager<ApplicationUser>> userManagerMock;

    private VilaController controller;
    [SetUp]
    public void Setup()
    {
        vilaServiceMock = new Mock<IVilla>();
        townServiceMock = new Mock<ITownService>();

        var store = new Mock<IUserStore<ApplicationUser>>();
        userManagerMock = new Mock<UserManager<ApplicationUser>>(
            store.Object, null, null, null, null, null, null, null, null
        );

        controller = new VilaController(
            vilaServiceMock.Object,
            townServiceMock.Object,
            userManagerMock.Object
        );

        // Mock user
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id")
        }));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [TearDown]
    public void TearDown()
    {
        controller?.Dispose();
    }

    [Test]
    public async Task AddVilla_Get_ReturnsViewWithModel()
    {
        // Arrange
        townServiceMock
            .Setup(x => x.TownViewDataAsync())
            .ReturnsAsync(new List<TownIndexViewModel>());

        townServiceMock
            .Setup(x => x.TypePlaceViewDataAsync())
            .ReturnsAsync(new List<TypePlaceIndexViewModel>());

        // Act
        var result = await controller.AddVilla();

        // Assert
        var viewResult = result as ViewResult;

        Assert.IsNotNull(viewResult);
        Assert.AreEqual("Views/Vila/AddVilla.cshtml", viewResult.ViewName);

        var model = viewResult.Model as AddVillaIndexViewModel;
        Assert.IsNotNull(model);
        Assert.IsNotNull(model.AllTownsModels);
        Assert.IsNotNull(model.AllTypePlaces);
    }
    [Test]
    public async Task AddVilla_Get_OnException_RedirectsToError()
    {
        // Arrange
        townServiceMock
            .Setup(x => x.TownViewDataAsync())
            .ThrowsAsync(new System.Exception());

        // Act
        var result = await controller.AddVilla();

        // Assert
        var redirect = result as RedirectToActionResult;

        Assert.IsNotNull(redirect);
        Assert.AreEqual("Error", redirect.ActionName);
        Assert.AreEqual("Home", redirect.ControllerName);
    }

    [Test]
    public async Task AddVilla_Post_Success_RedirectsToIndex()
    {
        // Arrange
        var model = new AddVillaIndexViewModel();

        vilaServiceMock
            .Setup(x => x.AddVilaModel(It.IsAny<string>(), model))
            .ReturnsAsync(true);

        // Act
        var result = await controller.AddVilla(model);

        // Assert
        var redirect = result as RedirectToActionResult;

        Assert.IsNotNull(redirect);
        Assert.AreEqual("Index", redirect.ActionName);
        Assert.AreEqual("Vila", redirect.ControllerName);
    }
    [Test]
    public async Task AddVilla_Post_Failed_ReturnsViewWithReloadedData()
    {
        // Arrange
        var model = new AddVillaIndexViewModel();

        vilaServiceMock
            .Setup(x => x.AddVilaModel(It.IsAny<string>(), model))
            .ReturnsAsync(false);

        townServiceMock
            .Setup(x => x.TownViewDataAsync())
            .ReturnsAsync(new List<TownIndexViewModel>());

        townServiceMock
            .Setup(x => x.TypePlaceViewDataAsync())
            .ReturnsAsync(new List<TypePlaceIndexViewModel>());

        // Act
        var result = await controller.AddVilla(model);

        // Assert
        var viewResult = result as ViewResult;

        Assert.IsNotNull(viewResult);
        Assert.AreEqual("Views/Vila/AddVilla.cshtml", viewResult.ViewName);

        var returnedModel = viewResult.Model as AddVillaIndexViewModel;
        Assert.IsNotNull(returnedModel);
        Assert.IsNotNull(returnedModel.AllTownsModels);
    }
    [Test]
    public async Task AddVilla_Post_Exception_ReturnsViewWithModel()
    {
        // Arrange
        var model = new AddVillaIndexViewModel();

        vilaServiceMock
            .Setup(x => x.AddVilaModel(It.IsAny<string>(), model))
            .ThrowsAsync(new System.Exception());

        townServiceMock
            .Setup(x => x.TownViewDataAsync())
            .ReturnsAsync(new List<TownIndexViewModel>());

        townServiceMock
            .Setup(x => x.TypePlaceViewDataAsync())
            .ReturnsAsync(new List<TypePlaceIndexViewModel>());

        // Act
        var result = await controller.AddVilla(model);

        // Assert
        var viewResult = result as ViewResult;

        Assert.IsNotNull(viewResult);
        Assert.AreEqual("Views/Vila/AddVilla.cshtml", viewResult.ViewName);
    }

    //Details Tests
    [Test]
    public async Task Details_ValidId_ReturnsViewWithModel()
    {
        // Arrange
        string id = "5";

        var expectedModel = new DetailsIndexVilla();

        vilaServiceMock
            .Setup(x => x.GetVilaDetailsAsync(5, It.IsAny<string>()))
            .ReturnsAsync(expectedModel);

        // Act
        var result = await controller.Details(id);

        // Assert
        var viewResult = result as ViewResult;

        Assert.IsNotNull(viewResult);
        Assert.AreEqual("Views/Vila/DetailsVila.cshtml", viewResult.ViewName);
        Assert.AreEqual(expectedModel, viewResult.Model);
    }

    [Test]
    public async Task Details_CallsServiceWithCorrectParameters()
    {
        // Arrange
        string id = "10";

        vilaServiceMock
            .Setup(x => x.GetVilaDetailsAsync(10, It.IsAny<string>()))
            .ReturnsAsync(new DetailsIndexVilla());

        // Act
        await controller.Details(id);

        // Assert
        vilaServiceMock.Verify(x =>
            x.GetVilaDetailsAsync(10, It.IsAny<string>()),
            Times.Once);
    }

    [Test]
    public async Task Details_InvalidId_RedirectsToError()
    {
        // Arrange
        string id = "invalid"; // will fail int.Parse

        // Act
        var result = await controller.Details(id);

        // Assert
        var redirect = result as RedirectToActionResult;

        Assert.IsNotNull(redirect);
        Assert.AreEqual("Error", redirect.ActionName);
        Assert.AreEqual("Home", redirect.ControllerName);
    }

    [Test]
    public async Task Details_ServiceThrows_RedirectsToError()
    {
        // Arrange
        string id = "3";

        vilaServiceMock
            .Setup(x => x.GetVilaDetailsAsync(3, It.IsAny<string>()))
            .ThrowsAsync(new System.Exception());

        // Act
        var result = await controller.Details(id);

        // Assert
        var redirect = result as RedirectToActionResult;

        Assert.IsNotNull(redirect);
        Assert.AreEqual("Error", redirect.ActionName);
        Assert.AreEqual("Home", redirect.ControllerName);
    }

    //edit villa
    [Test]
    public async Task EditVilla_Get_Valid_ReturnsView()
    {
        // Arrange
        var model = new EditVilaViewModel();

        vilaServiceMock.Setup(x => x.GetForEditVila(1, It.IsAny<string>()))
            .ReturnsAsync(model);

        townServiceMock.Setup(x => x.TownViewDataAsync())
            .ReturnsAsync(new List<TownIndexViewModel>());

        townServiceMock.Setup(x => x.TypePlaceViewDataAsync())
            .ReturnsAsync(new List<TypePlaceIndexViewModel>());

        // Act
        var result = await controller.EditVilla("1");

        // Assert
        var viewResult = result as ViewResult;

        Assert.IsNotNull(viewResult);
        Assert.AreEqual("Views/Vila/BookVillaView.cshtml", viewResult.ViewName);
        Assert.IsInstanceOf<EditVilaViewModel>(viewResult.Model);
    }

    [Test]
    public async Task EditVilla_Get_InvalidModelState_RedirectsToError()
    {
        // Arrange
        controller.ModelState.AddModelError("error", "invalid");

        // Act
        var result = await controller.EditVilla("1");

        // Assert
        var redirect = result as RedirectToActionResult;

        Assert.IsNotNull(redirect);
        Assert.AreEqual("Error", redirect.ActionName);
        Assert.AreEqual("Home", redirect.ControllerName);
    }

    [Test]
    public async Task EditVilla_Get_Exception_RedirectsToError()
    {
        // Arrange
        vilaServiceMock.Setup(x => x.GetForEditVila(It.IsAny<int>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await controller.EditVilla("1");

        // Assert
        var redirect = result as RedirectToActionResult;

        Assert.IsNotNull(redirect);
        Assert.AreEqual("Error", redirect.ActionName);
    }

    [Test]
    public async Task EditVilla_Get_InvalidId_RedirectsToError()
    {
        // Act
        var result = await controller.EditVilla("invalid-id");

        // Assert
        var redirect = result as RedirectToActionResult;

        Assert.IsNotNull(redirect);
        Assert.AreEqual("Error", redirect.ActionName);
    }

    [Test]
    public async Task EditVilla_Post_InvalidModelState_RedirectsToError()
    {
        // Arrange
        controller.ModelState.AddModelError("error", "invalid");

        var model = new EditVilaViewModel();

        // Act
        var result = await controller.EditVilla(model);

        // Assert
        var redirect = result as RedirectToActionResult;

        Assert.IsNotNull(redirect);
        Assert.AreEqual("Error", redirect.ActionName);
    }

    [Test]
    public async Task EditVilla_Post_ServiceFails_RedirectsToError()
    {
        // Arrange
        var model = new EditVilaViewModel();

        vilaServiceMock.Setup(x => x.EditVilla(It.IsAny<string>(), model))
            .ReturnsAsync(false);

        // Act
        var result = await controller.EditVilla(model);

        // Assert
        var redirect = result as RedirectToActionResult;

        Assert.IsNotNull(redirect);
        Assert.AreEqual("Error", redirect.ActionName);
    }

    [Test]
    public async Task EditVilla_Post_Success_ReturnsPartialView()
    {
        // Arrange
        var model = new EditVilaViewModel();

        vilaServiceMock.Setup(x => x.EditVilla(It.IsAny<string>(), model))
            .ReturnsAsync(true);

        townServiceMock.Setup(x => x.TownViewDataAsync())
            .ReturnsAsync(new List<TownIndexViewModel>());

        townServiceMock.Setup(x => x.TypePlaceViewDataAsync())
            .ReturnsAsync(new List<TypePlaceIndexViewModel>());

        // Act
        var result = await controller.EditVilla(model);

        // Assert
        var partial = result as PartialViewResult;

        Assert.IsNotNull(partial);
        Assert.AreEqual("Views/Vila/BookVillaView.cshtml", partial.ViewName);
        Assert.IsInstanceOf<EditVilaViewModel>(partial.Model);
    }

    [Test]
    public async Task EditVilla_Post_Exception_RedirectsToError()
    {
        // Arrange
        var model = new EditVilaViewModel();

        vilaServiceMock.Setup(x => x.EditVilla(It.IsAny<string>(), model))
            .ThrowsAsync(new Exception());

        // Act
        var result = await controller.EditVilla(model);

        // Assert
        var redirect = result as RedirectToActionResult;

        Assert.IsNotNull(redirect);
        Assert.AreEqual("Error", redirect.ActionName);
    }

    //load AllVilas
    //[Test]
    //public async Task Index_ReturnsView_WithModel()
    //{
    //    // Arrange
    //    var userId = "test-user-id";

    //    var villas = new List<object>(); // replace with real VM type

    //    vilaServiceMock
    //        .Setup(x => x.GetAllVillasAsync(userId, 1, 3))
    //        .ReturnsAsync(villas);

    //    userManagerMock
    //        .Setup(x => x.FindByIdAsync(userId))
    //        .ReturnsAsync(new ApplicationUser
    //        {
    //            EmailConfirmed = true
    //        });

    //    // Act
    //    var result = await controller.Index();

    //    // Assert
    //    var viewResult = result as ViewResult;

    //    Assert.IsNotNull(viewResult);
    //    Assert.AreEqual("Views/Vila/Index.cshtml", viewResult.ViewName);
    //    Assert.AreEqual(villas, viewResult.Model);

    //    Assert.IsTrue((bool)controller.ViewBag.EmailConfirmed);
    //}
    //Search by date

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}
