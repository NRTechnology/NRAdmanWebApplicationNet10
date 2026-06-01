using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    /// <summary>
    /// Model untuk RADIUS Policy di Mikrotik
    /// Menyimpan konfigurasi bandwidth/QoS yang akan diterapkan ke queue
    /// </summary>
    [Table("mikrotik_radius_policies")]
    public class MikrotikRadiusPolicy
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("policy_name")]
        [Display(Name = "Policy Name")]
        public string PolicyName { get; set; } = "";

        [Column("description")]
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Download limit dalam Mbps
        /// </summary>
        [Column("download_limit")]
        [Display(Name = "Download Limit (Mbps)")]
        public decimal? DownloadLimit { get; set; }

        /// <summary>
        /// Upload limit dalam Mbps
        /// </summary>
        [Column("upload_limit")]
        [Display(Name = "Upload Limit (Mbps)")]
        public decimal? UploadLimit { get; set; }

        /// <summary>
        /// Burst limit untuk download dalam Mbps
        /// </summary>
        [Column("burst_limit_down")]
        [Display(Name = "Burst Download (Mbps)")]
        public decimal? BurstLimitDown { get; set; }

        /// <summary>
        /// Burst limit untuk upload dalam Mbps
        /// </summary>
        [Column("burst_limit_up")]
        [Display(Name = "Burst Upload (Mbps)")]
        public decimal? BurstLimitUp { get; set; }

        /// <summary>
        /// Burst threshold untuk download dalam persen
        /// </summary>
        [Column("burst_threshold_down")]
        [Display(Name = "Burst Threshold Down (%)")]
        public int? BurstThresholdDown { get; set; }

        /// <summary>
        /// Burst threshold untuk upload dalam persen
        /// </summary>
        [Column("burst_threshold_up")]
        [Display(Name = "Burst Threshold Up (%)")]
        public int? BurstThresholdUp { get; set; }

        /// <summary>
        /// Burst time dalam detik
        /// </summary>
        [Column("burst_time")]
        [Display(Name = "Burst Time (seconds)")]
        public int? BurstTime { get; set; }

        /// <summary>
        /// Priority queue (1-16, 1 adalah highest)
        /// </summary>
        [Column("priority")]
        [Range(1, 16)]
        [Display(Name = "Priority")]
        public int Priority { get; set; } = 8;

        /// <summary>
        /// Status policy (aktif/nonaktif)
        /// </summary>
        [Column("is_active")]
        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Column("created_date")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [Column("created_by")]
        [MaxLength(255)]
        public string? CreatedBy { get; set; }

        [Column("modified_by")]
        [MaxLength(255)]
        public string? ModifiedBy { get; set; }
    }
}
