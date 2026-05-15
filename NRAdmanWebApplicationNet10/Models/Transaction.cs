using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Index(nameof(TransactionCode), IsUnique = true)]
    public class Transaction
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("transaction_code")]
        public string TransactionCode { get; set; } = string.Empty;

        [ForeignKey(nameof(Customer))]
        [Column("customer_id")]
        public int? CustomerId { get; set; }

        public Customer? Customer { get; set; }

        [Required]
        [ForeignKey(nameof(Package))]
        [Column("package_id")]
        public int PackageId { get; set; }

        public Package? Package { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("payment_method")]
        public string PaymentMethod { get; set; } = "Cash";

        // Pending, Paid, Cancelled
        [Required]
        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; } = "Pending";

        [Column("paid_at")]
        public DateTimeOffset? PaidAt { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
