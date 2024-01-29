using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Entities
{
    public class MindMasterMinds_DBContext : DbContext
    {
        public MindMasterMinds_DBContext()
        {
        }

        public MindMasterMinds_DBContext(DbContextOptions<MindMasterMinds_DBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.Entity<PostReaction>()
                .HasKey(pr => new { pr.PostId, pr.UserId });
        }

        public virtual DbSet<TransactionWallet> TransactionWallets { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public virtual DbSet<Wallet> Wallets { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<PostComment> PostComments { get; set; } = null!;
        public virtual DbSet<PostReaction> PostReactions { get; set; } = null!;
        public virtual DbSet<Reaction> Reactions { get; set; } = null!;
    }
}
