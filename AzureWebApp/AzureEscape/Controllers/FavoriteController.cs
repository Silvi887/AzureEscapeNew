using AzureApp.ViewModels;
using AzureServises.Core;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AzureEscape.Controllers
{
    public class FavoriteController :BaseController
    {
        private readonly IVilla vilaService;
       
        private readonly UserManager<IdentityUser?>? UserManager;

        public FavoriteController(IVilla vilaService,UserManager<IdentityUser> userManager)
        {
            this.vilaService = vilaService;
            this.UserManager = userManager;
           
        }

        public async Task<IActionResult> Index()
        {
            try
            {

                return View("Views/Home/Index.cshtml");
            }

            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home");

            }
            }

        [HttpGet]
        public async Task<IActionResult> GetFavorite()
        {


            string? userid = this.GetUserId();
            IEnumerable<FavoriteVilaIndexViewModel>? AllPlaces = await vilaService.GetFavoritePlaces(userid);
            try
            {

                // AllHotels = await this.vacationService.GetFavotiteReservation(userid);

                if (AllPlaces == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View("Views/Vila/FavoritePlaces.cshtml", AllPlaces);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return RedirectToAction("Error", "Home");

                //  return View("Views/Vila/FavoritePlaces.cshtml", AllPlaces);
                //return this.RedirectToAction(nameof(Index));

            }

        }

        [HttpPost]
        public async Task<IActionResult> GetFavorite(int id)
        {
            string? userid = this.GetUserId();

            try
            {
                bool isavalid = await vilaService.FavoritePlaces(userid, id);

                if (isavalid == false)
                {
                    return this.RedirectToAction(nameof(Index));
                }
                return this.RedirectToAction(nameof(GetFavorite));


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Error", "Home");

              //  return this.RedirectToAction(nameof(Index));

            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteFavorite(int id)
        {

            try
            {

                var Userid = this.GetUserId();
                bool isavalid = await vilaService.RemoveFavorite(Userid, id);

                if (isavalid == false)
                {
                    return this.RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(GetFavorite));
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Error", "Home");
                // return this.RedirectToAction(nameof(Index));

            }
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
