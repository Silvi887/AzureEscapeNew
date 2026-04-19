using AzureAdd.DataModels;
using AzureApp.ViewModels;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AzureEscape.Controllers
{
    public class ReserveController : BaseController
    {
        private readonly IVilla vilaService;
        private readonly UserManager<ApplicationUser?>? UserManager;

        public ReserveController(IVilla vilaService, UserManager<ApplicationUser> userManager)
        {
            this.vilaService = vilaService;
            this.UserManager = userManager;
            
        }

        public virtual string? GetUserId()
        {
            return UserManager?.GetUserId(User);
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
                return PartialView("Views/Vila/AddReservation.cshtml", inAddReservation);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Error", "Home");

            }

        }

        [HttpPost]
        public async Task<IActionResult> Add(AddReservationViewModel inAddReservation)
        {
            try
            {
              
                string? UserId = this.GetUserId();
                if (!this.ModelState.IsValid)
                {
                    // return this.View(inAddReservation);


                    //   return Ok();
                    ModelState.AddModelError(string.Empty, "Fatal error accure while adding a reservation!");
                    return View("Views/Vila/AddReservation.cshtml", inAddReservation);
                }

                bool isvalid = await vilaService.AddBookingModel(UserId, inAddReservation);

                if (isvalid == false)
                {

                    ModelState.AddModelError(string.Empty, "Fatal error accure while adding a reservation!");
                    return this.RedirectToAction(nameof(AddBooking), "Reserve");
                }
                return Ok();


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Error", "Home");
              

            }
        }


        [HttpGet]
        public async Task<IActionResult> AllReservations(string? Userid)
        {
            try
            {


                string? UserId = this.GetUserId();

                IEnumerable<AllReservationsViewModel> allreservations = await this.vilaService.GetAllReservations(UserId);
                var user = await UserManager.FindByIdAsync(UserId);
                ViewBag.EmailConfirmed = user?.EmailConfirmed ?? false;
                return View("Views/Vila/AllReservations.cshtml", allreservations);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");

            }
        
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string id) //Delete
        {

            try
            {

                int id1 = int.Parse(id);
                var UserId = this.GetUserId();

                DeleteReservationIndexViewModel selectedreservation = await vilaService.GetForDeleteReservation(id1, UserId);

                if (selectedreservation != null)
                {
                    return View("Views/Vila/DeleteReservation.cshtml", selectedreservation);
                }
                return RedirectToAction("Index","Vila");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index", "Vila");

            }
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

                if (editreservation == false)
                {
                    return View("Views/Vila/Edit.cshtml", deletedres);
                }


                return this.RedirectToAction(nameof(AllReservations));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return RedirectToAction("Error", "Home");
             

            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {

            try
            {

                int id1 = int.Parse(id);
                var UserId = this.GetUserId();
                EditBooking currentreservation = await vilaService.GetForEditReservation(id1, UserId);
               

                if (this.ModelState.IsValid)
                {
                 
                    return PartialView("Views/Vila/EditReservationPartial.cshtml", currentreservation);
                }

                return View(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
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

             

                if (editreservation == false)
                {
                   
                    return View("Views/Vila/EditReservationPartial.cshtml", reservationmodel);
                }


                return Ok();
               
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return RedirectToAction("Error", "Home");
              

            }


        }




    }
}
