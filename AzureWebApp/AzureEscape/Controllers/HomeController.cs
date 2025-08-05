using AzureEscape.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AzureEscape.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser?>? UserManager;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            this.UserManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            try
            {


                if (this.IsUserAuthenticated())
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
