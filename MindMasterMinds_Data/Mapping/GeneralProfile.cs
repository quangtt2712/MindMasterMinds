using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Entities.UserRole, Models.Views.UserRoleViewModel>();
            CreateMap<Entities.User, Models.Views.UserViewModel>();
            CreateMap<Entities.Post, Models.Views.PostViewModel>();
            CreateMap<Entities.Reaction, Models.Views.ReactionViewModel>();
            CreateMap<Entities.PostReaction, Models.Views.PostReactionViewModel>();
            CreateMap<Entities.PostComment, Models.Views.PostCommentViewModel>();
        }
    }
}
