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
    public interface IPostReactionService
    {
        Task<ErrorResponse> CreatePostReaction(CreatePostReactionModel createPostReactionModel, Guid userId);
        Task<ErrorResponse> DeletePostReaction(Guid postId, Guid userId);
        Task<ErrorResponse> UpdatePostReaction(UpdatePostReactionModel updatePostReactionModel, Guid userId);
        Task<PostReactionViewModel> GetPostReactionById(Guid postId, Guid userId);
    }
}
