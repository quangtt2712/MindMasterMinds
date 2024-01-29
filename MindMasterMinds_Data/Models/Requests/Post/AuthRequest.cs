using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Models.Requests.Post
{
    public class AuthRequest
    {
        [Required(ErrorMessage = "Email là trường bắt buộc.")]
        [MaxLength(255, ErrorMessage = "Email không được vượt quá 255 kí tự.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Mật khẩu là trường bắt buộc.")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Mật khẩu phải từ 8 đến 25 kí tự.")]

        public string Password { get; set; } = null!;
    }
}
