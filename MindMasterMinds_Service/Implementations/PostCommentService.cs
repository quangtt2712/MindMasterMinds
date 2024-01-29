using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MindMasterMinds_Data;
using MindMasterMinds_Data.Entities;
using MindMasterMinds_Data.Models.Requests.Get;
using MindMasterMinds_Data.Models.Requests.Post;
using MindMasterMinds_Data.Models.Requests.Put;
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
    public class PostCommentService : BaseService, IPostCommentService
    {
        public PostCommentService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ErrorResponse> CreatePostComment(CreatePostCommentModel createPostCommentModel, Guid userId)
        {
            var checkUser = await _unitOfWork.User.FirstOrDefaultAsync(u => u.Id == userId);

            if (checkUser == null) { throw new NotFoundException("Người dùng không tồn tại"); }

            var checkPost = await _unitOfWork.Post.FirstOrDefaultAsync(p => p.Id == createPostCommentModel.PostId);

            if (checkPost == null) { throw new NotFoundException("Bài viết không tồn tại"); }

            var postComment = new PostComment()
            {
                Id = Guid.NewGuid(),
                PostId = createPostCommentModel.PostId,
                UserId = userId,
                Comment = createPostCommentModel.Comment,
                CreationDate = DateTime.UtcNow
            };

            _unitOfWork.PostComment.Add(postComment);
            await _unitOfWork.SaveChangesAsync();

            return new ErrorResponse()
            {
                Message = "Tạo bình luận thành công"
            };
        }

        public async Task<ErrorResponse> DeletePostComment(Guid postCommentId, Guid userId)
        {
            var checkPostComment = await _unitOfWork.PostComment.FirstOrDefaultAsync(pc => pc.Id == postCommentId && pc.UserId == userId);

            if (checkPostComment == null) { throw new NotFoundException("Bình luận không tồn tại"); }

            checkPostComment.IsDeleted = true;
            _unitOfWork.PostComment.Update(checkPostComment);
            await _unitOfWork.SaveChangesAsync();

            return new ErrorResponse()
            {
                Message = "Xóa bình luận thành công"
            };
        }

        public async Task<ListViewModel<PostCommentViewModel>> GetALlPostCommentByPostId(Guid postId, PaginationRequestModel pagination)
        {
            var checkPost = await _unitOfWork.Post.FirstOrDefaultAsync(p => p.Id == postId);

            if (checkPost == null) { throw new NotFoundException("Bài viết không tồn tại"); }

            var listPostComment = _unitOfWork.PostComment.GetMany(p => p.IsDeleted == false).Include(p => p.User).OrderByDescending(p => p.CreationDate).ProjectTo<PostCommentViewModel>(_mapper.ConfigurationProvider);

            var getlistPostComment = await listPostComment.Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await listPostComment.AsNoTracking().CountAsync();

            if (getlistPostComment != null || getlistPostComment != null && getlistPostComment.Any())
            {
                return new ListViewModel<PostCommentViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                        TotalRow = totalRow
                    },
                    Data = getlistPostComment
                };
            }
            return null!;
        }

        public async Task<ErrorResponse> UpdatePostComment(UpdatePostCommentModel updatePostCommentModel, Guid userId)
        {
            var checkPostComment = await _unitOfWork.PostComment.FirstOrDefaultAsync(pc => pc.Id == updatePostCommentModel.PostCommentId && pc.UserId == userId);

            if (checkPostComment == null) { throw new NotFoundException("Bình luận không tồn tại"); }

            checkPostComment.Comment = updatePostCommentModel.Comment;
            checkPostComment.LastModifiedDate = DateTime.UtcNow;
            _unitOfWork.PostComment.Update(checkPostComment);
            await _unitOfWork.SaveChangesAsync();

            return new ErrorResponse()
            {
                Message = "Cập nhật bình luận thành công"
            };
        }


    }
}
