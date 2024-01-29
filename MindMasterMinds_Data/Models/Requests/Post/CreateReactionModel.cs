using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Models.Requests.Post
{
    public class CreateReactionModel
    {
        [Required(ErrorMessage = "Tên không được để trống.")]
        [MaxLength(50, ErrorMessage = "Tên không được quá 50 kí tự.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Hình ảnh không được để trống.")]

        public IFormFile Image { get; set; } = null!;
    }
}
