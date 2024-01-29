using AutoMapper;
using AutoMapper.QueryableExtensions;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MindMasterMinds_Data;
using MindMasterMinds_Data.Models.Requests.Get;
using MindMasterMinds_Data.Models.Requests.Post;
using MindMasterMinds_Data.Models.Views;
using MindMasterMinds_Service.Interfaces;
using MindMasterMinds_Utility.Exceptions;
using MindMasterMinds_Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Service.Implementations
{
    public class PostService : BaseService, IPostService
    {
        public PostService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ErrorResponse> CreatePost(CreatePostModel model, Guid userId)
        {
            using(var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if (model.Image == null && model.Content == null)
                    {
                        throw new BadRequestException("Bài viết phải có nội dung hoặc ảnh.");
                    }

                    var user = await _unitOfWork.User.FirstOrDefaultAsync(x => x.Id == userId);
                    if (user == null)
                    {
                        throw new NotFoundException("Không tìm thấy người dùng.");
                    }

                    var post = new MindMasterMinds_Data.Entities.Post();

                    post.UserId = userId;
                    post.Id = Guid.NewGuid();
                    if (model.Content != null)
                    {
                        post.Content = model.Content;
                    }

                    if (model.Image != null)
                    {
                        try
                        {
                            post.Image = await UploadImageToFirebase.UploadProductImageToFirebase(model.Image!);
                        } catch {
                                                        throw new BadRequestException("Ảnh không hợp lệ.");
                        }
                    }

                    post.CreationDate = DateTime.UtcNow;
                    _unitOfWork.Post.Add(post);
                    await _unitOfWork.SaveChangesAsync();
                    transaction.Commit();
                    return new ErrorResponse
                    {
                        Message = "Tạo bài viết thành công."
                    };
                }
                catch 
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<ListViewModel<PostViewModel>> GetAllPostForUser(PaginationRequestModel pagination)
        {
            var posts = _unitOfWork.Post.GetMany(p => p.IsDeleted == false).Include(p => p.User).OrderByDescending(p => p.CreationDate).ProjectTo<PostViewModel>(_mapper.ConfigurationProvider);

            var getListPost = await posts.Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await posts.AsNoTracking().CountAsync();

            if (getListPost != null || getListPost != null && getListPost.Any())
            {
                return new ListViewModel<PostViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                        TotalRow = totalRow
                    },
                    Data = getListPost
                };
            }
            return null!;

        }





    }
}
