using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMasterMinds_Data.Models.Requests.Post;
using MindMasterMinds_Data.Models.Views;
using MindMasterMinds_Service.Interfaces;
using MindMasterMinds_Utility.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace MindMasterMinds_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService=authService;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(TokenViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Đăng nhập.")]
        public async Task<ActionResult<TokenViewModel>> Login([FromBody] AuthRequest auth)
        {
            var result = await _authService.Authenticated(auth);
            return Ok(result);
        }
    }
}
