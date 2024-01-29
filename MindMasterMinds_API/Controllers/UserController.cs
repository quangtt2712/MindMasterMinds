using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMasterMinds_Data.Models.Requests.Post;
using MindMasterMinds_Service.Interfaces;
using MindMasterMinds_Utility.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace MindMasterMinds_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService=userService;
        }

        [HttpPost("send-OTP-email")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [SwaggerOperation(Summary = "Đăng kí email lấy mã OTP")]

        public async Task<ActionResult<ErrorResponse>> SendOTPEmail([FromBody]string email)
        {
            var response = await _userService.SendOTPEmail(email);
            return Ok(response);
        }

        [HttpPost("register-student")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [SwaggerOperation(Summary = "Đăng kí tài khoản học sinh")]
        public async Task<ActionResult<ErrorResponse>> RegisterStudent([FromBody]RegisterStudentRequest registerStudentRequest)
        {
            var response = await _userService.AccountRegister(registerStudentRequest);
            return Ok(response);
        }
    }
}
