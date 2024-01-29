using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MindMasterMinds_Data;
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
    public class UserRoleService : BaseService, IUserRoleService
    {
        public UserRoleService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<UserRoleViewModel> CreateUserRole(string roleName)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var checkRole = await _unitOfWork.UserRole.FirstOrDefaultAsync(x => x.RoleName == roleName);

                    if (checkRole != null) { throw new ConflictException("Role này đã tồn tại"); }

                    var role = new MindMasterMinds_Data.Entities.UserRole
                    {
                        Id = Guid.NewGuid(),
                        RoleName = roleName
                    };

                    _unitOfWork.UserRole.Add(role);

                    await _unitOfWork.SaveChangesAsync();

                    transaction.Commit();

                    return await GetUserRole(role.Id);

                } catch
                {
                    transaction.Rollback();
                    throw new BadRequestException("Tạo role thất bại");
                }
            }
        }

        public async Task<UserRoleViewModel> GetUserRole(Guid id)
        {
            return await _unitOfWork.UserRole.GetMany(userrole => userrole.Id.Equals(id))
                            .ProjectTo<UserRoleViewModel>(_mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync() ?? null!;
        }

        public async Task<List<UserRoleViewModel>> GetUserRoles()
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var roleAdmin = await _unitOfWork.UserRole.FirstOrDefaultAsync(x => x.RoleName == MindMasterMinds_Utility.Constants.UserRole.Admin);

                    if (roleAdmin == null)
                    {
                        var admin = new MindMasterMinds_Data.Entities.UserRole
                        {
                            Id = Guid.NewGuid(),
                            RoleName = MindMasterMinds_Utility.Constants.UserRole.Admin
                        };

                        _unitOfWork.UserRole.Add(admin);

                        await _unitOfWork.SaveChangesAsync();
                    }

                    var roleStudent = await _unitOfWork.UserRole.FirstOrDefaultAsync(x => x.RoleName == MindMasterMinds_Utility.Constants.UserRole.Student);

                    if (roleStudent == null)
                    {
                        var student = new MindMasterMinds_Data.Entities.UserRole
                        {
                            Id = Guid.NewGuid(),
                            RoleName = MindMasterMinds_Utility.Constants.UserRole.Student
                        };
                        _unitOfWork.UserRole.Add(student);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    var roleTutor = await _unitOfWork.UserRole.FirstOrDefaultAsync(x => x.RoleName == MindMasterMinds_Utility.Constants.UserRole.Tutor);

                    if (roleTutor == null)
                    {
                        var tutor = new MindMasterMinds_Data.Entities.UserRole
                        {
                            Id = Guid.NewGuid(),
                            RoleName = MindMasterMinds_Utility.Constants.UserRole.Tutor
                        };
                        _unitOfWork.UserRole.Add(tutor);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    transaction.Commit();
                    return await _unitOfWork.UserRole.GetAll()
                                    .ProjectTo<UserRoleViewModel>(_mapper.ConfigurationProvider)
                                    .ToListAsync();
                } 
                catch
                {
                    transaction.Rollback();
                    throw new BadRequestException("Lấy role thất bại");
                }
            }
                
        }
    }
}
