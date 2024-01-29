using Microsoft.EntityFrameworkCore.Storage;
using MindMasterMinds_Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data
{
    public interface IUnitOfWork
    {
        public IUserRepository User { get; }
        public IUserRoleRepository UserRole { get; }
        public IWalletRepository Wallet { get; }
        public IRefreshTokenRepository RefreshToken { get; }
        public IPostRepository Post { get; }
        public IReactionRepository Reaction { get; }
        public IPostReactionRepository PostReaction { get; }
        public IPostCommentRepository PostComment { get; }
        IDbContextTransaction BeginTransaction();
        Task<int> SaveChangesAsync();
    }
}
