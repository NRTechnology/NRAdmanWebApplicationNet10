using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Table("mikrotik_radius_policy")]
    public class MikrotikRadiusPolicy
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("policy_name")]
        [Required]
        [MaxLength(100)]
        [Display(Name = "Policy Name")]
        public string PolicyName { get; set; } = "";

        [Column("description")]
        [MaxLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        // Bandwidth Management
        [Column("download_limit")]
        [Display(Name = "Download Limit (Mbps)")]
        public decimal? DownloadLimit { get; set; }

        [Column("upload_limit")]
        [Display(Name = "Upload Limit (Mbps)")]
        public decimal? UploadLimit { get; set; }

        // Queue Type
        [Column("queue_type")]
        [Required]
        [Display(Name = "Queue Type")]
        public EnumQueueType QueueType { get; set; } = EnumQueueType.SimpleQueue;

        // Priority (1-16, lower = higher priority)
        [Column("priority")]
        [Display(Name = "Priority (1-16)")]
        public int Priority { get; set; } = 8;

        // Burst Settings
        [Column("burst_limit_down")]
        [Display(Name = "Burst Download Limit (Mbps)")]
        public decimal? BurstLimitDown { get; set; }

        [Column("burst_limit_up")]
        [Display(Name = "Burst Upload Limit (Mbps)")]
        public decimal? BurstLimitUp { get; set; }

        [Column("burst_threshold_down")]
        [Display(Name = "Burst Threshold Down (%)")]
        public int? BurstThresholdDown { get; set; }

        [Column("burst_threshold_up")]
        [Display(Name = "Burst Threshold Up (%)")]
        public int? BurstThresholdUp { get; set; }

        [Column("burst_time")]
        [Display(Name = "Burst Time (seconds)")]
        public int? BurstTime { get; set; }

        // Advanced Settings
        [Column("max_limit_down")]
        [Display(Name = "Max Limit Download (Mbps)")]
        public decimal? MaxLimitDown { get; set; }

        [Column("max_limit_up")]
        [Display(Name = "Max Limit Upload (Mbps)")]
        public decimal? MaxLimitUp { get; set; }

        [Column("is_active")]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Column("created_date")]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("modified_date")]
        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Column("created_by")]
        [MaxLength(255)]
        public string? CreatedBy { get; set; }

        [Column("modified_by")]
        [MaxLength(255)]
        public string? ModifiedBy { get; set; }
    }

    public enum EnumQueueType
    {
        [Display(Name = "Simple Queue")]
        SimpleQueue = 1,
        [Display(Name = "PCQ")]
        PCQ = 2,
        [Display(Name = "HTB")]
        HTB = 3
    }
}
