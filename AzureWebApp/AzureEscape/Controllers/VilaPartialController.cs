using AzureApp.ViewModels;
using AzureServises.Core;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzureEscape.Controllers
{
    public class VilaPartialController : BaseController
    {

        private readonly IVilla vilaservice;
        private readonly ITownService townService;

        public VilaPartialController(IVilla villa, ITownService townService)
        {
            this.vilaservice = villa;
            this.townService = townService;
        }
        [HttpGet]
        public async Task<IActionResult> EditVillaPartial(int id)
        {
            var userId = this.GetUserId();

            var villa = await vilaservice.GetVilaTemplate(id, userId);
            villa.AllTownsModels = (IEnumerable<TownIndexViewModel>)await this.townService.TownViewDataAsync();
            villa.AllTypePlaces = (IEnumerable<TypePlaceIndexViewModel>)await this.townService.TypePlaceViewDataAsync();

            if (villa == null)
            {
                return NotFound();
            }


            return PartialView("Views/Vila/BookVillaView.cshtml", villa);
        }

        //post
        [HttpPost]
        public async Task<IActionResult> EditVilla(EditVilaViewModel editvillamodel)
        {

            try
            {

                string? userid = this.GetUserId();

                if (!ModelState.IsValid)
                {
                   // return BadRequest("Invalid model");
                     return RedirectToAction("Error", "Home");
                }

                bool editvilla = await vilaservice.EditVilla(userid, editvillamodel);

                // reservationmodel.roomdrp = (IEnumerable<RoomViewModel>)await this.vacationService.RoomViewDataAsync();

                if (editvilla == false)
                {
                   // return BadRequest("Update failed");
                    return RedirectToAction("Error", "Home");
                }


                editvillamodel.AllTownsModels = (IEnumerable<TownIndexViewModel>)await this.townService.TownViewDataAsync();
                editvillamodel.AllTypePlaces = (IEnumerable<TypePlaceIndexViewModel>)await this.townService.TypePlaceViewDataAsync();

               // RedirectToAction("EditVillaPartial");

                return Ok();
                // ViewBag.SuccessMessage = "Successful update of villa!";
                // return View("Views/Vila/EditVilla.cshtml", editvillamodel);


            }
            catch (Exception ex)
            {
              //  return StatusCode(500, "Server error");
                return RedirectToAction("Error", "Home");
            }


        }
    }
}
