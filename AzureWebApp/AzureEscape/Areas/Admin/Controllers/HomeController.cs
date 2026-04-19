using AzureAdd.DataModels;
using AzureEscape.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AzureEscape.Areas.Admin.Controllers
{



    [Area("Admin")] // or whatever your area is called
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser?>? UserManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            UserManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            try
            {
                //return Content("ADMIN OK");


                if (IsUserAuthenticated())
                {
                    return RedirectToAction("Index", "Vila");
                }

                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {

                    return View();
                    // return RedirectToAction("Index","Vacation");
                }
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [AllowAnonymous]
        public IActionResult Contact()
        {

            try
            {


                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


                return View("Views/Vila/ContactUs.cshtml");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [AllowAnonymous]
        public IActionResult Feedbacks()
        {

            try
            {



                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


                return View("Views/Vila/Feedbacks.cshtml");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }




        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Page500()
        {

            return View("Views/Shared/PageError500.cshtml");
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404)
            {

                return View("Views/Shared/PageError404.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            else if (statusCode == 500 || statusCode == null || statusCode == 0)
            {

                return View("Views/Shared/PageError500.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            return View("Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       
    }
}
