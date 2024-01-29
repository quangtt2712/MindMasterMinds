using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Models.Requests.Post
{
    public class CreatePostModel
    {
        public string? Content { get; set; } 
        public IFormFile? Image { get; set; }
    }
}
