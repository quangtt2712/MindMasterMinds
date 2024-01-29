using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Models.Requests.Put
{
    public class UpdateReactionModel
    {
        [MaxLength(50, ErrorMessage = "Tên không được quá 50 kí tự.")]
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
    }
}
