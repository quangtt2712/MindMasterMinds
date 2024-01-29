using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Data.Entities
{
    public partial class TransactionWallet
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        [Required(ErrorMessage = "Số tiền là trường bắt buộc.")]
        [Range(100_000, 5_000_000, ErrorMessage = "Số tiền phải lớn hơn hoặc bằng 100000.")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Loại giao dịch là trường bắt buộc.")]
        [StringLength(50, ErrorMessage = "Loại giao dịch không được vượt quá 50 ký tự.")]
        public string Type { get; set; } = null!;
        [Required(ErrorMessage = "Trạng thái là trường bắt buộc.")]
        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự.")]
        public string Status { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public virtual Wallet Wallet { get; set; } = null!;
    }
}
