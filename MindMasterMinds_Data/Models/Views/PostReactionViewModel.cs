using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Models.Views
{
    public class PostReactionViewModel
    {
        public DateTime CreationDate { get; set; }
        public UserViewModel User { get; set; } = null!;
        public ReactionViewModel Reaction { get; set; } = null!;
        public PostViewModel Post { get; set; } = null!;

    }
}
