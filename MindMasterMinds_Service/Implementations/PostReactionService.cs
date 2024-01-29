using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MindMasterMinds_Data;
using MindMasterMinds_Data.Entities;
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
    public class PostReactionService : BaseService, IPostReactionService
    {
        public PostReactionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ErrorResponse> CreatePostReaction(CreatePostReactionModel createPostReactionModel, Guid userId)
        {
            var checkReaction = await _unitOfWork.Reaction.FirstOrDefaultAsync(r => r.Id == createPostReactionModel.ReactionId);

            if (checkReaction == null) { throw new NotFoundException("Cảm xúc bài viết không có"); }

            var checkPost = await _unitOfWork.Post.FirstOrDefaultAsync(p => p.Id == createPostReactionModel.PostId);

            if (checkPost == null) { throw new NotFoundException("Bài viết không tồn tại"); }

            var checkUser = await _unitOfWork.User.FirstOrDefaultAsync(u => u.Id == userId);

            if (checkUser == null) { throw new NotFoundException("Người dùng không tồn tại"); }

            var checkPostReaction = await _unitOfWork.PostReaction.FirstOrDefaultAsync(pr => pr.PostId == createPostReactionModel.PostId && pr.UserId == userId);

            if (checkPostReaction != null)
            {
                checkPostReaction.ReactionId = createPostReactionModel.ReactionId;
                _unitOfWork.PostReaction.Update(checkPostReaction);
            }
            else
            {
                var postReaction = new PostReaction()
                {
                    PostId = createPostReactionModel.PostId,
                    ReactionId = createPostReactionModel.ReactionId,
                    UserId = userId,
                    CreationDate = DateTime.UtcNow
                };

                _unitOfWork.PostReaction.Add(postReaction);
            }

            await _unitOfWork.SaveChangesAsync();

            return new ErrorResponse()
            {
                Message = "Cảm xúc bài viết thành công"
            };
        }

        public async Task<ErrorResponse> DeletePostReaction(Guid postId, Guid userId)
        {
            var checkPost = await _unitOfWork.Post.FirstOrDefaultAsync(p => p.Id == postId);

            if (checkPost == null) { throw new NotFoundException("Bài viết không tồn tại"); }

            var checkUser = await _unitOfWork.User.FirstOrDefaultAsync(u => u.Id == userId);

            if (checkUser == null) { throw new NotFoundException("Người dùng không tồn tại"); }

            var checkPostReaction = await _unitOfWork.PostReaction.FirstOrDefaultAsync(pr => pr.PostId == postId && pr.UserId == userId);

            if (checkPostReaction == null) { throw new NotFoundException("Cảm xúc bài viết không tồn tại"); }

            _unitOfWork.PostReaction.Remove(checkPostReaction);

            await _unitOfWork.SaveChangesAsync();

            return new ErrorResponse()
            {
                Message = "Xóa cảm xúc bài viết thành công"
            };
        }

        public async Task<ErrorResponse> UpdatePostReaction(UpdatePostReactionModel updatePostReactionModel, Guid userId)
        {
            var checkReaction = await _unitOfWork.Reaction.FirstOrDefaultAsync(r => r.Id == updatePostReactionModel.ReactionId);

            if (checkReaction == null) { throw new NotFoundException("Cảm xúc bài viết không có"); }

            var checkPost = await _unitOfWork.Post.FirstOrDefaultAsync(p => p.Id == updatePostReactionModel.PostId);

            if (checkPost == null) { throw new NotFoundException("Bài viết không tồn tại"); }

            var checkUser = await _unitOfWork.User.FirstOrDefaultAsync(u => u.Id == userId);

            if (checkUser == null) { throw new NotFoundException("Người dùng không tồn tại"); }

            var checkPostReaction = await _unitOfWork.PostReaction.FirstOrDefaultAsync(pr => pr.PostId == updatePostReactionModel.PostId && pr.UserId == userId);

            if (checkPostReaction == null) { throw new NotFoundException("Cảm xúc bài viết không tồn tại"); }

            checkPostReaction.ReactionId = updatePostReactionModel.ReactionId;

            _unitOfWork.PostReaction.Update(checkPostReaction);

            await _unitOfWork.SaveChangesAsync();

            return new ErrorResponse()
            {
                Message = "Cập nhật cảm xúc bài viết thành công"
            };
        }

        public async Task<PostReactionViewModel> GetPostReactionById(Guid postId, Guid userId)
        {
            var checkPost = await _unitOfWork.Post.FirstOrDefaultAsync(p => p.Id == postId);

            if (checkPost == null) { throw new NotFoundException("Bài viết không tồn tại"); }

            var checkUser = await _unitOfWork.User.FirstOrDefaultAsync(u => u.Id == userId);

            if (checkUser == null) { throw new NotFoundException("Người dùng không tồn tại"); }

            var checkPostReaction = await _unitOfWork.PostReaction.GetMany(pr => pr.PostId == postId && pr.UserId == userId).ProjectTo<PostReactionViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (checkPostReaction == null) { throw new NotFoundException("Cảm xúc bài viết không tồn tại"); }

            return checkPostReaction;
        }
    }
}
