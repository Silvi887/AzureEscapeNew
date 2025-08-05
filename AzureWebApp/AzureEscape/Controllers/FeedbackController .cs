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

            try
            {


                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");

            }
            }

        [HttpGet]
        public IActionResult LeaveFeedback(string bookingId, string id)
        {

            try
            {

                var model = new BookingFeedbackViewModel
                {
                    BookingId = 1,
                    VillaId = int.Parse(id),
                    VillaName = ""
                };
                return View("Views/Vila/LeaveFeedBacks.cshtml", model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");

            }
            }

        [HttpPost]
        public async Task<IActionResult> LeaveFeedback(BookingFeedbackViewModel modelFeedback)
        {

            try
            {

                string Userid = this.GetUserId();

                modelFeedback.VillaName = "";
                if (!ModelState.IsValid)
                {

                    return RedirectToAction("Error", "Home");
                    return View(modelFeedback);
                }

                bool isvalid = await vilaService.LeaveFeedBack(Userid, modelFeedback);
                if (!isvalid)
                {

                    IEnumerable<VilaIndexViewModel> Allvillas = await this.vilaService.GetAllVillasAsync(Userid);
                    return RedirectToAction("Error", "Home");
                    //  return View("Views/Vila/Index.cshtml");

                    // return View("Views/Vila/Index.cshtml", Allvillas);
                }

                // Save feedback to DB or process it
                TempData["Success"] = "Thanks for your feedback!";
                return View("Views/Vila/LeaveFeedBacks.cshtml", modelFeedback);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AllFeedBacks()
        {

            try
            {

                string Userid = this.GetUserId();
                bool isvalid = false;
                IEnumerable<BookingFeedbackViewModel> allfeedbacks = await this.vilaService.GetAllFeedbacks(Userid);

                return View("Views/Vila/AllFeedBacks.cshtml", allfeedbacks);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");

            }
            }
    }
}
