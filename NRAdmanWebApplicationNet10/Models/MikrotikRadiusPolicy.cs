using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    /// <summary>
    /// Model untuk RADIUS Policy di Mikrotik
    /// Menyimpan konfigurasi bandwidth/QoS yang akan diterapkan ke queue
    /// </summary>
    [Index(nameof(PolicyName), IsUnique = true)]
    public class MikrotikRadiusPolicy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Policy Name")]
        public string PolicyName { get; set; } = "";
        
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Download limit dalam Mbps
        /// </summary>
        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Download Limit (Mbps)")]
        public decimal DownloadLimit { get; set; } = 0;

        /// <summary>
        /// Upload limit dalam Mbps
        /// </summary>
        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Upload Limit (Mbps)")]
        public decimal UploadLimit { get; set; } = 0;

        /// <summary>
        /// Burst limit untuk download dalam Mbps
        /// </summary>
        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Burst Download (Mbps)")]
        public decimal BurstLimitDown { get; set; } = 0;

        /// <summary>
        /// Burst limit untuk upload dalam Mbps
        /// </summary>
        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Burst Upload (Mbps)")]
        public decimal BurstLimitUp { get; set; } = 0;

        /// <summary>
        /// Burst threshold untuk download dalam persen
        /// </summary>
        [Required]
        [Range(0, 100)]
        [Display(Name = "Burst Threshold Down (%)")]
        public int BurstThresholdDown { get; set; } = 0;

        /// <summary>
        /// Burst threshold untuk upload dalam persen
        /// </summary>
        [Required]
        [Range(0, 100)]
        [Display(Name = "Burst Threshold Up (%)")]
        public int BurstThresholdUp { get; set; } = 0;

        /// <summary>
        /// Burst time dalam detik
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "Burst Time (seconds)")]
        public int BurstTime { get; set; } = 0;

        /// <summary>
        /// Priority queue (1-16, 1 adalah highest)
        /// </summary>
        [Required]
        [Range(1, 16)]
        [Display(Name = "Priority")]
        public int Priority { get; set; } = 8;

        /// <summary>
        /// Status policy (aktif/nonaktif)
        /// </summary>
        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
        
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? ModifiedDate { get; set; }
        
        [MaxLength(255)]
        public string? CreatedBy { get; set; }
        
        [MaxLength(255)]
        public string? ModifiedBy { get; set; }
    }
}
