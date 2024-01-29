using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MindMasterMinds_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpGet]
        [Route("ipn")]
        [SwaggerOperation(Summary = "API này để VnPay dùng.")]
        public ActionResult Ipn()
        {
            return Ok();
        }
    }
}
