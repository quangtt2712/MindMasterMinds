using Microsoft.EntityFrameworkCore.Storage;
using MindMasterMinds_Data.Entities;
using MindMasterMinds_Data.Repositories.Implementations;
using MindMasterMinds_Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MindMasterMinds_DBContext _context;

        private IUserRepository _user = null!;
        private IUserRoleRepository _userRole = null!;
        private IWalletRepository _wallet = null!;
        private IRefreshTokenRepository _refreshToken = null!;
        private IPostRepository _post = null!;
        private IReactionRepository _reaction = null!;
        private IPostReactionRepository _postReaction = null!;
        private IPostCommentRepository _postComment = null!;

        public UnitOfWork(MindMasterMinds_DBContext context)
        {
            _context = context;
        }

        public IPostCommentRepository PostComment { get { return _postComment ??= new PostCommentRepository(_context); } }
        public IPostReactionRepository PostReaction { get { return _postReaction ??= new PostReactionRepository(_context); } }

        public IReactionRepository Reaction { get { return _reaction ??= new ReactionRepository(_context); } }

        public IPostRepository Post { get { return _post ??= new PostRepository(_context); } }
        public IRefreshTokenRepository RefreshToken { get { return _refreshToken ??= new RefreshTokenRepository(_context); } }
        public IUserRepository User { get { return _user ??= new UserRepository(_context); } }
        public IUserRoleRepository UserRole { get { return _userRole ??= new UserRoleRepository(_context); } }
        public IWalletRepository Wallet { get { return _wallet ??= new WalletRepository(_context); } }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
