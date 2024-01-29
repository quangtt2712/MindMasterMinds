using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Models.Views
{
    public class PostViewModel
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool IsDeleted { get; set; }
        public UserViewModel User { get; set; } = null!;
        public int CommentsCount { get; set; }
        public int ReactionsCount { get; set; }
    }
}
