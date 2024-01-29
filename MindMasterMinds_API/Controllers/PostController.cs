using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMasterMinds_API.Configurations.Middleware;
using MindMasterMinds_Data.Models.Internal;
using MindMasterMinds_Data.Models.Requests.Get;
using MindMasterMinds_Data.Models.Requests.Post;
using MindMasterMinds_Data.Models.Views;
using MindMasterMinds_Service.Interfaces;
using MindMasterMinds_Utility.Constants;
using MindMasterMinds_Utility.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace MindMasterMinds_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService=postService;
        }

        [HttpPost]
        [Authorize(UserRole.Student, UserRole.Tutor)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Tạo một bài đăng giống facebook.")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostModel model)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var result = await _postService.CreatePost(model, auth!.Id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(UserRole.Student, UserRole.Tutor)]
        [ProducesResponseType(typeof(ListViewModel<PostViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lấy tất cả bài viết hiện có trong hệ thống cho người dùng.")]
        public async Task<IActionResult> GetAllPostForUser([FromQuery] PaginationRequestModel pagination)
        {
            var result = await _postService.GetAllPostForUser(pagination);
            return Ok(result);
        }
    }
}
