using Microsoft.AspNetCore.Mvc;

namespace AzureEscape.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
