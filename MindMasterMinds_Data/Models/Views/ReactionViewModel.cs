using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Models.Views
{
    public class ReactionViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
    }
}
