using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMasterMinds_Data.Models.Internal;
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
    public class PostReactionController : ControllerBase
    {
        private readonly IPostReactionService _postReactionService;

        public PostReactionController(IPostReactionService postReactionService)
        {
            _postReactionService=postReactionService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Thả cảm xúc hoặc cập nhật cảm xúc bài viết")]

        public async Task<IActionResult> CreatePostReaction([FromBody] CreatePostReactionModel model)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var result = await _postReactionService.CreatePostReaction(model, auth!.Id);
            return Ok(result);
        }

        [HttpDelete("{postId}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Thu hồi cảm xúc đã thả vào bài viết.")]

        public async Task<IActionResult> DeletePostReaction(Guid postId)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var result = await _postReactionService.DeletePostReaction(postId, auth!.Id);
            return Ok(result);
        }

        /*[HttpPut]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePostReaction([FromBody] UpdatePostReactionModel model)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var result = await _postReactionService.UpdatePostReaction(model, auth!.Id);
            return Ok(result);
        }*/

        [HttpGet("{postId}")]
        [ProducesResponseType(typeof(PostReactionViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Xem cảm xúc của mình đã thả từ 1 bài viết.")]

        public async Task<IActionResult> GetPostReactionById(Guid postId)
        {
            var auth = (AuthModel?)HttpContext.Items["User"];
            var result = await _postReactionService.GetPostReactionById(postId, auth!.Id);
            return Ok(result);
        }


    }
}
