using AzureApp.ViewModels;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AzureEscape.Controllers
{
    public class FeedbackController : BaseController
    {

        private readonly IVilla vilaService;
        private readonly ITownService TownService;

        private readonly UserManager<IdentityUser?>? UserManager;

        public FeedbackController(IVilla vilaService,ITownService townservice  , UserManager<IdentityUser> userManager)
        {
            this.vilaService = vilaService;
            this.UserManager = userManager;
            this.TownService = townservice;

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
        public async Task<IActionResult> LeaveFeedback(string bookingId, string id)
        {

            try
            {

                string? userid=this.GetUserId();



                var allidsbookings = await this.TownService.TypeIdBookingFeedback(userid, id);

                if (allidsbookings == null || !allidsbookings.Any())
                {
                    TempData["FeedbackError"] = "You don't have permission to leave feedback.";
                   // return RedirectToAction("Index", "Home"); // Or redirect to a relevant page
                }

                var model =  new BookingFeedbackViewModel
                {
                   // BookingId = 1,
                    VillaId = int.Parse(id),
                    VillaName = "",
                    idbookingsforuser= allidsbookings

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
