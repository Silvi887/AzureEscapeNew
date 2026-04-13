using AzureApp.ViewModels;
using AzureServises.Core;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzureEscape.Controllers
{
    public class EditBookingPartialController : BaseController
    {

        private readonly IVilla vilaService;

        public EditBookingPartialController(IVilla villaservice)
        {
            this.vilaService = villaservice;
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {

            try
            {

                int id1 = int.Parse(id);
                var UserId = this.GetUserId();
                EditBooking currentreservation = await vilaService.GetForEditReservation(id1, UserId);
                //currentreservation.roomdrp = (IEnumerable<RoomViewModel>)await this.vacationService.RoomViewDataAsync();

                if (this.ModelState.IsValid)
                {
                    return PartialView("Views/Vila/EditReservationPartial.cshtml", currentreservation);
                }


                return PartialView("Views/Vila/EditReservationPartial.cshtml", currentreservation);
               // return View(nameof(Index));
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

                // reservationmodel.roomdrp = (IEnumerable<RoomViewModel>)await this.vacationService.RoomViewDataAsync();

                if (editreservation == false)
                {
                    return View("Views/Vila/EditReservation.cshtml", reservationmodel);
                }


                return Ok();
               // ViewBag.SuccessMessage = "Successful update of reservation!";
              //  return View("Views/Vila/EditReservation.cshtml", reservationmodel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return RedirectToAction("Error", "Home");
                //  return this.RedirectToAction(nameof(Index));

            }


        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
