using MindMasterMinds_Data.Models.Requests.Get;
using MindMasterMinds_Data.Models.Requests.Post;
using MindMasterMinds_Data.Models.Views;
using MindMasterMinds_Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Service.Interfaces
{
    public interface IPostService
    {
        Task<ErrorResponse> CreatePost(CreatePostModel model, Guid userId);
        Task<ListViewModel<PostViewModel>> GetAllPostForUser(PaginationRequestModel pagination);
    }
}
