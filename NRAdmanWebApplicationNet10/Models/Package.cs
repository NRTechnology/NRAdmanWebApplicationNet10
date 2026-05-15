using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Index(nameof(Code), IsUnique = true)]
    public class Package
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        [Column("code")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("description")]
        public string? Description { get; set; }

        // Example: 1M/1M
        [Required]
        [MaxLength(64)]
        [Column("rate_limit")]
        public string RateLimit { get; set; } = string.Empty;

        // in seconds
        [Column("session_timeout")]
        public int SessionTimeout { get; set; }

        // in seconds
        [Column("idle_timeout")]
        public int? IdleTimeout { get; set; }

        // total quota in MB
        [Column("quota_mb")]
        public long? QuotaMb { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
