using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Models.Requests.Put
{
    public class UpdatePostReactionModel
    {
        public Guid PostId { get; set; }
        public Guid ReactionId { get; set; }
    }
}
