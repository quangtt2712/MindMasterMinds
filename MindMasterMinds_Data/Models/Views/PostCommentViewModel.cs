using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Models.Views
{
    public class PostCommentViewModel
    {
        public Guid Id { get; set; }
        public UserViewModel User { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
