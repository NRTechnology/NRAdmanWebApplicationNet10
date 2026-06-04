using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Index(nameof(Code), IsUnique = true)]
    public class Voucher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(Package))]
        public Guid PackageId { get; set; }

        [Required]
        public virtual Package? Package { get; set; }

        [Required] public int ValidityDays { get; set; } = 0;

        [Required]
        public int MaxDevices { get; set; } = 1;

        [Required]
        public bool IsUsed { get; set; } = false;

        [Required]
        public DateTimeOffset? UsedAt { get; set; }

        [Required]
        [MaxLength(64)]
        public string? UsedBy { get; set; } = string.Empty;

        [Required]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
