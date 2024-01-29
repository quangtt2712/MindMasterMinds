using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Entities
{
    public class Post
    {
        public Post()
        {
            PostComments = new HashSet<PostComment>();
            PostReactions = new HashSet<PostReaction>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool IsDeleted { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<PostComment>? PostComments { get; set; }
        public virtual ICollection<PostReaction>? PostReactions { get; set; }

    }
}
