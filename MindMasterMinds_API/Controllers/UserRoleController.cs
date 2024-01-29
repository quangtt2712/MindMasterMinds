using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMasterMinds_API.Configurations.Middleware;
using MindMasterMinds_Data.Models.Views;
using MindMasterMinds_Service.Interfaces;
using MindMasterMinds_Utility.Exceptions;
using MindMasterMinds_Utility.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace MindMasterMinds_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService=userRoleService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lấy tất cả role")]
        public async Task<ActionResult<UserRoleViewModel>> GetUserRoles()
        {
            try
            {
                var response = await _userRoleService.GetUserRoles();
                return Ok(response);
            }
            catch(Exception ex)
            {
                throw new BadRequestException(ex.Message);
            }
            
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Lấy role theo id")]
        public async Task<ActionResult<UserRoleViewModel>> GetUserRole(Guid id)
        {
            try
            {
                var response = await _userRoleService.GetUserRole(id);
                return response != null ? Ok(response) : throw new NotFoundException("Không tìm thấy role");
            } 
            catch
            {
                throw;
            }
            
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [Authorize(MindMasterMinds_Utility.Constants.UserRole.Admin)]
        [SwaggerOperation(Summary = "Tạo role cho người dùng")]
        public async Task<ActionResult<UserRoleViewModel>> CreateUserRole(string roleName)
        {
            try
            {
                var response = await _userRoleService.CreateUserRole(roleName);
                return CreatedAtAction(nameof(GetUserRole), new { id = response.Id }, response);
            }
            catch
            {
                throw new BadRequestException("Tạo role thất bại");
            }
            
        }
    }
}
