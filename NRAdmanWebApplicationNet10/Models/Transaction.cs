using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Index(nameof(TransactionCode), IsUnique = true)]
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string TransactionCode { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(Customer))]
        public Guid CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }

        [Required]
        [ForeignKey(nameof(Package))]
        public Guid PackageId { get; set; }

        public virtual Package? Package { get; set; }

        [Required] public decimal Amount { get; set; } = 0;

        [Required]
        [MaxLength(20)]
        public string PaymentMethod { get; set; } = "Cash";

        // Pending, Paid, Cancelled
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        
        public DateTimeOffset? PaidAt { get; set; }

        
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
