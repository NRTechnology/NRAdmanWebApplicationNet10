using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Index(nameof(Code), IsUnique = true)]
    public class Package
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        // Example: 1M/1M
        [Required]
        [MaxLength(64)]
        public string RateLimit { get; set; } = string.Empty;

        // in seconds
        [Required] 
        public int SessionTimeout { get; set; } = 0;

        // in seconds
        public int? IdleTimeout { get; set; }

        // total quota in MB
        public long? QuotaMb { get; set; }

        [Required] 
        public decimal Price { get; set; } = 0;

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
