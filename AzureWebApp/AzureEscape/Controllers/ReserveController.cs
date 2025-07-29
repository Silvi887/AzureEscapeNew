using AzureApp.ViewModels;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AzureEscape.Controllers
{
    public class ReserveController : BaseController
    {
        private readonly IVilla vilaService;
       
        private readonly UserManager<IdentityUser?>? UserManager;

        public ReserveController(IVilla vilaService, UserManager<IdentityUser> userManager)
        {
            this.vilaService = vilaService;
            this.UserManager = userManager;
            
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddBooking(string? id)
        {
            try
            {

                string[] ArrVilaName = id.Split(',');

                // int idhotel1 = int.Parse(id);
                AddReservationViewModel inAddReservation = new AddReservationViewModel()
                {
                    VillaId = ArrVilaName[0],
                    VilaName = ArrVilaName[1],
                    // 
                    StartDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                    EndDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                   // roomdrp = (IEnumerable<RoomViewModel>)await this.vacationService.RoomViewDataAsync(),
                    DateofBirth = DateTime.UtcNow.ToString("yyyy-MM-dd")

                };
                return View("Views/Vila/AddReservation.cshtml", inAddReservation);

                // return this.View()

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index));

            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddReservationViewModel inAddReservation)
        {
            try
            {

                //int idhotel1 = int.Parse(idhotel);
                string? UserId = this.GetUserId();
                if (!this.ModelState.IsValid)
                {
                    // return this.View(inAddReservation);

                    return this.RedirectToAction(nameof(Add));
                }

                bool isvalid = await vilaService.AddBookingModel(UserId, inAddReservation);

                if (isvalid == false)
                {

                    ModelState.AddModelError(string.Empty, "Fatal error accure while adding a reservation!");
                    return this.RedirectToAction(nameof(Add));
                }


               // inAddReservation.roomdrp = (IEnumerable<RoomViewModel>)await this.vacationService.RoomViewDataAsync();
                ViewBag.SuccessMessage = "Successful reservation!";
                return View("Views/Vila/AddReservation.cshtml", inAddReservation);

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Views/Home/Index.cshtml");

            }
        }


        [HttpGet]

        public async Task<IActionResult> AllReservations(string? Userid)
        {

            string? UserId = this.GetUserId();

            IEnumerable<AllReservationsViewModel> allreservations = await this.vilaService.GetAllReservations(UserId);
            var user = await UserManager.FindByIdAsync(UserId);
            ViewBag.EmailConfirmed = user?.EmailConfirmed ?? false;
            return View("Views/Vila/AllReservations.cshtml", allreservations);
           // return View(allreservations);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string id) //Delete
        {
            int id1 = int.Parse(id);
            var UserId = this.GetUserId();

            DeleteReservationIndexViewModel selectedreservation = await vilaService.GetForDeleteReservation(id1, UserId);

            if (selectedreservation != null)
            {
                return View("Views/Vila/DeleteReservation.cshtml", selectedreservation);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]

        public async Task<IActionResult> Delete(DeleteReservationIndexViewModel deletedres)
        {

            try
            {

                string? userid = this.GetUserId();

                if (!ModelState.IsValid)
                {

                    return View(nameof(Index));
                }
                bool editreservation = await vilaService.DeleteReservation(userid, deletedres.IdBooking);

                //deletedres.roomdrp = (IEnumerable<RoomViewModel>)await this.vacationService.RoomViewDataAsync();

                if (editreservation == false)
                {
                    return View("Views/Vila/Edit.cshtml", deletedres);
                }


                return this.RedirectToAction(nameof(AllReservations));

                // ViewBag.SuccessMessage = "Successful update of reservation!";
                // return View("Views/Vacation/AllReservations.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index));

            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            int id1 = int.Parse(id);
            var UserId = this.GetUserId();
            EditBooking currentreservation = await vilaService.GetForEditReservation(id1, UserId);
            //currentreservation.roomdrp = (IEnumerable<RoomViewModel>)await this.vacationService.RoomViewDataAsync();

            if (this.ModelState.IsValid)
            {
                return View("Views/Vila/EditReservation.cshtml", currentreservation);
            }

            return View(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBooking reservationmodel)
        {

            try
            {

                string? userid = this.GetUserId();

                if (!ModelState.IsValid)
                {

                    return View(nameof(Index));
                }
                bool editreservation = await vilaService
                                           .EditReservation(userid, reservationmodel);

               // reservationmodel.roomdrp = (IEnumerable<RoomViewModel>)await this.vacationService.RoomViewDataAsync();

                if (editreservation == false)
                {
                    return View("Views/Vila/EditReservation.cshtml", reservationmodel);
                }



                ViewBag.SuccessMessage = "Successful update of reservation!";
                return View("Views/Vila/EditReservation.cshtml", reservationmodel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.RedirectToAction(nameof(Index));

            }


        }




    }
}
