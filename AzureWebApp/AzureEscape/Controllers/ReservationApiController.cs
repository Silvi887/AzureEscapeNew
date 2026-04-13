using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzureEscape.Controllers
{
    [Route("ReservationApi")]
    [ApiController]
    public class ReservationApiController :BaseInternalApiController
    {
        private readonly IAvailableDates availableDates;

        public ReservationApiController(IAvailableDates datesavailable)
        {
            this.availableDates = datesavailable;
        }
    
        [HttpGet("GetAllDates")]
        public async Task<IActionResult> GetAllDates(int idvilla)
        {

            var dates= await availableDates.bookingDatesViewModels(idvilla);
            return this.Ok(dates);
        }
    }
}
