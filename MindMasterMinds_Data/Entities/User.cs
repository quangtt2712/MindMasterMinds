using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Entities
{
    public partial class User
    {

        public User()
        {
            RefreshTokens = new HashSet<RefreshToken>();
            PostComments = new HashSet<PostComment>();
            Posts = new HashSet<Post>();
            PostReactions = new HashSet<PostReaction>();
        }

        public Guid Id { get; set; }

        [Required(ErrorMessage = "Email là trường bắt buộc.")]
        [MaxLength(255, ErrorMessage = "Email không được vượt quá 255 kí tự.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }

        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; } 
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreationDate { get; set; } 
        public DateTime? LastUpdateDate { get; set; }
        public string? Avatar { get; set; }

        public string? OTPEmailCode { get; set; } 
        public Guid? UserRoleId { get; set; }

        public DateTime? OTPEmailExpirationDate { get; set; }

        public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }
        public virtual Wallet? Wallet { get; set; } 

        public virtual UserRole? UserRole { get; set; }
        public virtual ICollection<PostComment>? PostComments { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<PostReaction>? PostReactions { get; set; }

    }
}
