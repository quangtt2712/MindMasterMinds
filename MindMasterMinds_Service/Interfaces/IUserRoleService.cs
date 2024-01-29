using MindMasterMinds_Data.Models.Views;
using MindMasterMinds_Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Service.Interfaces
{
    public interface IUserRoleService 
    {
        Task<UserRoleViewModel> CreateUserRole(string roleName);
        Task<UserRoleViewModel> GetUserRole(Guid id);
        Task<List<UserRoleViewModel>> GetUserRoles();

    }
}
