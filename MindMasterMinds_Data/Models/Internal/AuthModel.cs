using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Models.Internal
{
    public class AuthModel
    {
        public Guid Id { get; set; }
        public string Role { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
