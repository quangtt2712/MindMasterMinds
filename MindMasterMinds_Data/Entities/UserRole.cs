using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Entities
{
    public partial class UserRole
    {
        public UserRole()
        {
            Users = new HashSet<User>();
        }

        public Guid Id { get; set; }
        [Required]
        [MaxLength(255, ErrorMessage = "Role không được vượt quá 255 kí tự.")]
        public string RoleName { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; } = null!;
    }
}
