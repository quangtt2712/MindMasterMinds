using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Models.Requests.Post
{
    public class RegisterStudentRequest
    {
        [Required(ErrorMessage = "Email là trường bắt buộc.")]
        [MaxLength(255, ErrorMessage = "Email không được vượt quá 255 kí tự.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Mật khẩu là trường bắt buộc.")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Mật khẩu phải từ 8 đến 25 kí tự.")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Xác nhận mật khẩu là trường bắt buộc.")]
        [MaxLength(50, ErrorMessage = "FirstName không được quá 50 kí tự.")]

        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Xác nhận mật khẩu là trường bắt buộc.")]
        [MaxLength(50, ErrorMessage = "LastName không được quá 50 kí tự.")]

        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "Xác nhận OTPEmailCode là trường bắt buộc.")]
        [MaxLength(6, ErrorMessage = "OTPEmailCode không được quá 6 kí tự.")]
        public string OTPEmailCode { get; set; } = null!;

    }
}
