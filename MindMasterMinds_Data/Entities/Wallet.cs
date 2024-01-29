using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Entities
{
    public partial class Wallet
    {
        public Wallet()
        {
            TransactionWallets = new HashSet<TransactionWallet>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<TransactionWallet> TransactionWallets { get; set; }
    }
}
