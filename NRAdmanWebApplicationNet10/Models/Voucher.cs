using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Index(nameof(Code), IsUnique = true)]
    public class Voucher
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        [Column("code")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(Package))]
        [Column("package_id")]
        public int PackageId { get; set; }

        public Package? Package { get; set; }

        [Column("validity_days")]
        public int ValidityDays { get; set; }

        [Column("max_devices")]
        public int MaxDevices { get; set; } = 1;

        [Column("is_used")]
        public bool IsUsed { get; set; }

        [Column("used_at")]
        public DateTimeOffset? UsedAt { get; set; }

        [MaxLength(64)]
        [Column("used_by")]
        public string? UsedBy { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
