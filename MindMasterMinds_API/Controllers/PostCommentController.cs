using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMasterMinds_Data.Models.Internal;
using MindMasterMinds_Data.Models.Requests.Get;
using MindMasterMinds_Data.Models.Requests.Post;
using MindMasterMinds_Data.Models.Requests.Put;
using MindMasterMinds_Data.Models.Views;
using MindMasterMinds_Service.Interfaces;
using MindMasterMinds_Utility.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace MindMasterMinds_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostCommentController : ControllerBase
    {
        private readonly IPostCommentService _postCommentService;

        public PostCommentController(IPostCommentService postCommentService)
        {
            _postCommentService=postCommentService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Tạo 1 bình luận trên 1 bài đăng.")]
        public async Task<IActionResult> CreatePostComment([FromBody] CreatePostCommentModel model)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var result = await _postCommentService.CreatePostComment(model, auth!.Id);
            return Ok(result);
        }

        [HttpDelete("{postCommentId}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Xóa 1 bình luận.")]
        public async Task<IActionResult> DeletePostComment(Guid postCommentId)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var result = await _postCommentService.DeletePostComment(postCommentId, auth!.Id);
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Cập nhật bình luận.")]
        public async Task<IActionResult> UpdatePostComment([FromBody] UpdatePostCommentModel model)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var result = await _postCommentService.UpdatePostComment(model, auth!.Id);
            return Ok(result);
        }

        [HttpGet("all-post-comment-by-postId/{postId}")]
        [ProducesResponseType(typeof(ListViewModel<PostCommentViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Lấy tất cả bình luận của 1 bài viết (bình luận chưa xóa).")]
        public async Task<IActionResult> GetPostCommentById(Guid postId, [FromQuery] PaginationRequestModel pagination)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var result = await _postCommentService.GetALlPostCommentByPostId(postId, pagination);
            return Ok(result);
        }
    }
}
