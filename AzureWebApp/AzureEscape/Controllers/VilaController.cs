using AzureApp.ViewModels;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace AzureEscape.Controllers
{
    public class VilaController : BaseController
    {

        private readonly IVilla vilaService;
        private readonly UserManager<IdentityUser?>? UserManager;

        public VilaController(IVilla vilaService, UserManager<IdentityUser> userManager)
        {
            this.vilaService = vilaService;
            this.UserManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            string? UserId = this.GetUserId();
            IEnumerable<VilaIndexViewModel> Allvillas = await this.vilaService.GetAllVillasAsync(UserId);

            var user = await UserManager.FindByIdAsync(UserId);


            ViewBag.EmailConfirmed = user?.EmailConfirmed ?? false;
            return View("Views/Vila/Index.cshtml", Allvillas);
        }
    }
}
