using AzureApp.ViewModels;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace AzureEscape.Controllers
{
    public class VilaController : BaseController
    {

        private readonly IVilla vilaService;
        private readonly ITownService townService;
        private readonly UserManager<IdentityUser?>? UserManager;

        public VilaController(IVilla vilaService, ITownService townservice1, UserManager<IdentityUser> userManager)
        {
            this.vilaService = vilaService;
            this.UserManager = userManager;
            this.townService = townservice1;
        }

        public async Task<IActionResult> Index()
        {
            string? UserId = this.GetUserId();
            IEnumerable<VilaIndexViewModel> Allvillas = await this.vilaService.GetAllVillasAsync(UserId);

            var user = await UserManager.FindByIdAsync(UserId);


            ViewBag.EmailConfirmed = user?.EmailConfirmed ?? false;
            return View("Views/Vila/Index.cshtml", Allvillas);
        }

        [HttpGet]
        public async Task<IActionResult> AddVilla()
        {

            try
            {

                //if (!ModelState.IsValid)
                // {
                AddVillaIndexViewModel addvilla = new AddVillaIndexViewModel()
                {
                    AllTownsModels = (IEnumerable<TownIndexViewModel>)await this.townService.TownViewDataAsync(),
                    AllTypePlaces = (IEnumerable<TypePlaceIndexViewModel>)await this.townService.TypePlaceViewDataAsync(),
                };
                return View("Views/Vila/AddVilla.cshtml", addvilla);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddVilla(AddVillaIndexViewModel modelvila)
        {

            try
            {
                string? UserId = this.GetUserId();
                bool isvalid= await vilaService.AddVilaModel(UserId, modelvila);

                if (isvalid==false)
                {
                    modelvila = new AddVillaIndexViewModel()
                    {
                        AllTownsModels = (IEnumerable<TownIndexViewModel>)await this.townService.TownViewDataAsync(),
                        AllTypePlaces = (IEnumerable<TypePlaceIndexViewModel>)await this.townService.TypePlaceViewDataAsync(),
                    };

                    return View("Views/Vila/AddVilla.cshtml", modelvila);

                }

                ViewBag.SuccessMessage = "Successful addes vila!";


                return RedirectToAction(nameof(Index), "Vila");
                // return RedirectToAction("AddVilla", "Vila");

            }
        
            catch (Exception ex)
            {

                modelvila.AllTownsModels = (IEnumerable<TownIndexViewModel>)await this.townService.TownViewDataAsync();
                modelvila.AllTypePlaces = (IEnumerable<TypePlaceIndexViewModel>)await this.townService.TypePlaceViewDataAsync();
                Console.WriteLine(ex.Message);
                return View("Views/Vila/AddVilla.cshtml", modelvila);
            }



    }


        [AllowAnonymous]

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            int id1 = int.Parse(id);
            string? UserId = this.GetUserId();
            var VilaDetails = await this.vilaService.GetVilaDetailsAsync(id1, UserId);


            return View("Views/Vila/DetailsVila.cshtml", VilaDetails);
        }




    }

}
