using AzureApp.ViewModels;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AzureEscape.Controllers
{
    public class FeedbackController : BaseController
    {

        private readonly IVilla vilaService;

        private readonly UserManager<IdentityUser?>? UserManager;

        public FeedbackController(IVilla vilaService, UserManager<IdentityUser> userManager)
        {
            this.vilaService = vilaService;
            this.UserManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LeaveFeedback(string bookingId, string id)
        {
            var model = new BookingFeedbackViewModel
            {
                BookingId = 1,
                VillaId = int.Parse(id),
                 VillaName = ""
            };
            return View("Views/Vila/LeaveFeedBacks.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> LeaveFeedback(BookingFeedbackViewModel modelFeedback)
        {

            string Userid= this.GetUserId();

            modelFeedback.VillaName = "";
            if (!ModelState.IsValid)
            {
            return View(modelFeedback);
            }

            bool isvalid = await vilaService.LeaveFeedBack(Userid, modelFeedback);
            if (!isvalid)
            {

                IEnumerable<VilaIndexViewModel> Allvillas = await this.vilaService.GetAllVillasAsync(Userid);
                return View("Views/Vila/Index.cshtml");

               // return View("Views/Vila/Index.cshtml", Allvillas);
            }

            // Save feedback to DB or process it
            TempData["Success"] = "Thanks for your feedback!";
            return View("Views/Vila/LeaveFeedBacks.cshtml", modelFeedback);
        }

        [HttpGet]
        public async Task<IActionResult> AllFeedBacks()
        {
            string Userid = this.GetUserId();
            bool isvalid = false;
            IEnumerable<BookingFeedbackViewModel> allfeedbacks = await this.vilaService.GetAllFeedbacks(Userid);

            return View("Views/Vila/AllFeedBacks.cshtml", allfeedbacks);
        }
    }
}
