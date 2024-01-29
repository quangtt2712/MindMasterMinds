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
    public class ReactionService : BaseService, IReactionService
    {
        public ReactionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ErrorResponse> CreateReaction(CreateReactionModel createReactionModel)
        {
            var checkNameReaction = await _unitOfWork.Reaction.FirstOrDefaultAsync(x => x.Name == createReactionModel.Name);

            if (checkNameReaction != null)
            {
                throw new ConflictException("Tên biểu cảm đã tồn tại.");
            }

            var reaction = new Reaction
            {
                Id = Guid.NewGuid(),
                Name = createReactionModel.Name          
            };

            try
            {
                reaction.Image = await UploadImageToFirebase.UploadProductImageToFirebase(createReactionModel.Image!);
            }
            catch
            {
                throw new BadRequestException("Ảnh không hợp lệ.");
            }

            _unitOfWork.Reaction.Add(reaction);
            await _unitOfWork.SaveChangesAsync();

            return new ErrorResponse
            {
                Message = "Tạo biểu cảm thành công."
            };
        }

        public async Task<ErrorResponse> DeleteReaction(Guid id)
        {
            var reaction = await _unitOfWork.Reaction.FirstOrDefaultAsync(x => x.Id == id);

            if (reaction == null)
            {
                throw new NotFoundException("Không tìm thấy biểu cảm.");
            }

            var checkReaction = _unitOfWork.PostReaction.GetMany(x => x.ReactionId == id);
            if (checkReaction != null)
            {
                _unitOfWork.PostReaction.RemoveRange(checkReaction);
                await _unitOfWork.SaveChangesAsync();
            }

            _unitOfWork.Reaction.Remove(reaction);
            await _unitOfWork.SaveChangesAsync();

            return new ErrorResponse
            {
                Message = "Xóa biểu cảm thành công."
            };
        }

        public async Task<ErrorResponse> UpdateReaction(UpdateReactionModel updateReactionModel, Guid id)
        {

            if (updateReactionModel.Name == null && updateReactionModel.Image == null)
            {
                throw new BadRequestException("Không có thông tin cập nhật.");
            }

            var reaction = await _unitOfWork.Reaction.FirstOrDefaultAsync(x => x.Id == id);

            if (reaction == null)
            {
                throw new NotFoundException("Không tìm thấy biểu cảm.");
            }

            var checkNameReaction = await _unitOfWork.Reaction.FirstOrDefaultAsync(x => x.Name == updateReactionModel.Name && x.Id != reaction.Id);

            if (checkNameReaction != null)
            {
                throw new ConflictException("Tên biểu cảm đã tồn tại.");
            }

            if (updateReactionModel.Name != null)
            {
                reaction.Name = updateReactionModel.Name;
            }

            try
            {
                if (updateReactionModel.Image != null)
                {
                    reaction.Image = await UploadImageToFirebase.UploadProductImageToFirebase(updateReactionModel.Image);
                }
            } 
            catch
            {
                throw new BadRequestException("Ảnh không hợp lệ.");
            }


            _unitOfWork.Reaction.Update(reaction);
            await _unitOfWork.SaveChangesAsync();

            return new ErrorResponse
            {
                Message = "Cập nhật biểu cảm thành công."
            };
        }

        public async Task<ListViewModel<ReactionViewModel>> GetAllReaction(PaginationRequestModel pagination)
        {
            var reactions = _unitOfWork.Reaction.GetAll().ProjectTo<ReactionViewModel>(_mapper.ConfigurationProvider);

            var getListReaction = await reactions.Skip(pagination.PageNumber * pagination.PageSize).Take(pagination.PageSize).AsNoTracking().ToListAsync();
            var totalRow = await reactions.AsNoTracking().CountAsync();

            if (getListReaction != null || getListReaction != null && getListReaction.Any())
            {
                return new ListViewModel<ReactionViewModel>
                {
                    Pagination = new PaginationViewModel
                    {
                        PageNumber = pagination.PageNumber,
                        PageSize = pagination.PageSize,
                        TotalRow = totalRow
                    },
                    Data = getListReaction
                };
            }

            return null!;

        }
    }
}
