using Azure;
using AzureAdd.DataModels;
using AzureApp.ViewModels;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;



namespace AzureEscape.Areas.Admin.Controllers
{
    public class VilaController : BaseController
    {

        private readonly IVilla vilaService;
        private readonly ITownService townService;
        private readonly UserManager<ApplicationUser?>? UserManager;

        public VilaController(IVilla vilaService, ITownService townservice1, UserManager<ApplicationUser> userManager)
        {
            this.vilaService = vilaService;
            UserManager = userManager;
            townService = townservice1;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page=1)
        {

            try
            { 

            string? UserId = GetUserId();
                //Pagination
            int pageSize = 3; // how many villas per page

            var  Allvillas = await vilaService.GetAllVillasAsync(UserId, page, pageSize);
            var user = await UserManager.FindByIdAsync(UserId);


            ViewBag.EmailConfirmed = user?.EmailConfirmed ?? false;
            return View("~/Areas/Views/Vila/Index.cshtml", Allvillas);
            }
            catch (Exception ex) { 
                return RedirectToAction("Error", "Home");
            }
            }

        [AllowAnonymous]
        public async Task<IActionResult> SearchVilaByDate(string startDate, string endDate,int page=1)
        {

            try
            {

                string? UserId = GetUserId();
                int pageSize = 6;
                PagedResult<VilaIndexViewModel> AllVillasSearch = await vilaService.GetAllVillasSearch(UserId, startDate, endDate, page, pageSize);

            
                return View("Index", AllVillasSearch);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

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
                    AllTownsModels = await townService.TownViewDataAsync(),
                    AllTypePlaces = await townService.TypePlaceViewDataAsync(),
                };
                return View("Views/Vila/AddVilla.cshtml", addvilla);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Error", "Home");
              
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddVilla(AddVillaIndexViewModel modelvila)
        {

            try
            {
                string? UserId = GetUserId();
                bool isvalid= await vilaService.AddVilaModel(UserId, modelvila);

                if (isvalid==false)
                {
                    modelvila = new AddVillaIndexViewModel()
                    {
                        AllTownsModels = await townService.TownViewDataAsync(),
                        AllTypePlaces = await townService.TypePlaceViewDataAsync(),
                    };

                    return View("Views/Vila/AddVilla.cshtml", modelvila);

                }

                ViewBag.SuccessMessage = "Successful addes vila!";

                return RedirectToAction(nameof(Index), "Vila");
              

            }
        
            catch (Exception ex)
            {

                modelvila.AllTownsModels = await townService.TownViewDataAsync();
                modelvila.AllTypePlaces = await townService.TypePlaceViewDataAsync();
                Console.WriteLine(ex.Message);
                return View("Views/Vila/AddVilla.cshtml", modelvila);
            }



    }


        [AllowAnonymous]

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            try
            {

                int id1 = int.Parse(id);
                string? UserId = GetUserId();
                var VilaDetails = await vilaService.GetVilaDetailsAsync(id1, UserId);


                return View("~/Areas/Views/Vila/DetailsVila.cshtml", VilaDetails);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditVilla(string id)
        {

            try
            {

                int id1 = int.Parse(id);
                var UserId = GetUserId();
                EditVilaViewModel currentvilla = await vilaService.GetForEditVila(id1, UserId);

                currentvilla.AllTownsModels = await townService.TownViewDataAsync();
                currentvilla.AllTypePlaces = await townService.TypePlaceViewDataAsync();

                if (ModelState.IsValid)
                {

                    return View("Views/Vila/BookVillaView.cshtml", currentvilla);

                }

                return RedirectToAction("Error", "Home");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditVilla(EditVilaViewModel editvillamodel)
        {

            try
            {

                string? userid = GetUserId();

                if (!ModelState.IsValid)
                {

                    return RedirectToAction("Error", "Home");
                }

                bool editvilla = await vilaService
                                        .EditVilla(userid, editvillamodel);
            

                if (editvilla == false)
                {
                    return RedirectToAction("Error", "Home");
                }


                editvillamodel.AllTownsModels = await townService.TownViewDataAsync();
                editvillamodel.AllTypePlaces = await townService.TypePlaceViewDataAsync();

                ViewBag.SuccessMessage = "Successful update of villa!";
                return PartialView("Views/Vila/BookVillaView.cshtml", editvillamodel);


            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }


        }


    }

}
