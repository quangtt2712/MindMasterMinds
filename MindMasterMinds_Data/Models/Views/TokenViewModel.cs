using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Models.Views
{
    public class TokenViewModel
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;   
        public UserViewModel UserViewLogin { get; set; } = null!;
    }
}
