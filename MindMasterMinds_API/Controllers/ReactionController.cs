using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMasterMinds_API.Configurations.Middleware;
using MindMasterMinds_Data.Models.Requests.Get;
using MindMasterMinds_Data.Models.Requests.Post;
using MindMasterMinds_Data.Models.Requests.Put;
using MindMasterMinds_Data.Models.Views;
using MindMasterMinds_Service.Interfaces;
using MindMasterMinds_Utility.Constants;
using MindMasterMinds_Utility.Helpers;
using Swashbuckle.AspNetCore.Annotations;

namespace MindMasterMinds_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        private readonly IReactionService _reactionService;

        public ReactionController(IReactionService reactionService)
        {
            _reactionService=reactionService;
        }

        [HttpPost]
        //[Authorize(UserRole.Admin)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [SwaggerOperation(Summary = "Tạo một cảm xúc thả bài viết.")]

        public async Task<IActionResult> CreateReaction([FromForm] CreateReactionModel model)
        {
            var result = await _reactionService.CreateReaction(model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(UserRole.Admin)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Xóa một cảm xúc thả bài viết.")]
        public async Task<IActionResult> DeleteReaction(Guid id)
        {
            var result = await _reactionService.DeleteReaction(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(UserRole.Admin)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Cập nhật một cảm xúc thả bài viết.")]
        public async Task<IActionResult> UpdateReaction([FromForm] UpdateReactionModel model, Guid id)
        {
            var result = await _reactionService.UpdateReaction(model, id);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListViewModel<ReactionViewModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lấy tất cả cảm xúc thả bài viết.")]
        public async Task<IActionResult> GetAllReaction([FromQuery] PaginationRequestModel pagination)
        {
            var result = await _reactionService.GetAllReaction(pagination);
            return Ok(result);
        }
    }
}
