using MindMasterMinds_Data.Models.Requests.Post;
using MindMasterMinds_Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Service.Interfaces
{
    public interface IUserService
    {
        Task<ErrorResponse> SendOTPEmail(string email);
        Task<ErrorResponse> AccountRegister(RegisterStudentRequest registerStudentRequest);
    }
}
