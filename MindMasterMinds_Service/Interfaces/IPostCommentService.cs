using MindMasterMinds_Data.Models.Requests.Get;
using MindMasterMinds_Data.Models.Requests.Post;
using MindMasterMinds_Data.Models.Requests.Put;
using MindMasterMinds_Data.Models.Views;
using MindMasterMinds_Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Service.Interfaces
{
    public interface IPostCommentService
    {
        Task<ErrorResponse> CreatePostComment(CreatePostCommentModel createPostCommentModel, Guid userId);
        Task<ErrorResponse> DeletePostComment(Guid postCommentId, Guid userId);
        Task<ListViewModel<PostCommentViewModel>> GetALlPostCommentByPostId(Guid postId, PaginationRequestModel pagination);
        Task<ErrorResponse> UpdatePostComment(UpdatePostCommentModel updatePostCommentModel, Guid userId);
    }
}
