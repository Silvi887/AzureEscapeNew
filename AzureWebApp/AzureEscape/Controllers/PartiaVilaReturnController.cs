using AzureServises.Core;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzureEscape.Controllers
{

    [Route("PartiaVilaReturn")]
    [ApiController]
    public class PartiaVilaReturnController : BaseInternalApiController
    {

        private readonly IVilla vilaservice;

            public PartiaVilaReturnController(IVilla villa)
        {
            this.vilaservice = villa;
        }
        [HttpGet("EditVillaPartial")]
        public async Task<IActionResult> EditVillaPartial(int id)
        {
            var userId = this.GetUserId();

            var villa = await vilaservice.GetForEditVila(id, userId);

            if (villa == null)
                return NotFound();

            return Ok(villa);
        }
    }
}
