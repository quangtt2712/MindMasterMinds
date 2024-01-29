using MindMasterMinds_Data.Entities;
using MindMasterMinds_Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Repositories.Implementations
{
    public class PostReactionRepository : Repository<PostReaction>, IPostReactionRepository
    {
        public PostReactionRepository(MindMasterMinds_DBContext context) : base(context)
        {
        }
    }
}
