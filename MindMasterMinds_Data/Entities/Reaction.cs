using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Entities
{
    public class Reaction
    {
        public Reaction()
        {
            PostReactions = new HashSet<PostReaction>();
        }

        public Guid Id { get; set; }
        [MaxLength(50, ErrorMessage = "Tên không được quá 50 kí tự.")]
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;

        public virtual ICollection<PostReaction>? PostReactions { get; set; }
    }
}
