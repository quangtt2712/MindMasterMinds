using MindMasterMinds_Data.Models.Internal;
using MindMasterMinds_Data.Models.Requests.Post;
using MindMasterMinds_Data.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Service.Interfaces
{
    public interface IAuthService
    {
        Task<TokenViewModel> Authenticated(AuthRequest auth);
        Task<AuthModel> GetAuth(Guid id);
    }
}
