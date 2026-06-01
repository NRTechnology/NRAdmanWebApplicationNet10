using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    /// <summary>
    /// Model untuk melacak konfigurasi queue yang diterapkan pada router Mikrotik
    /// </summary>
    [Table("mikrotik_queue_config")]
    public class MikrotikQueueConfig
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Reference ke Router
        /// </summary>
        [Column("router_id")]
        [ForeignKey("Router")]
        public Guid? RouterId { get; set; }
        public virtual Router? Router { get; set; }

        /// <summary>
        /// Reference ke Policy
        /// </summary>
        [Column("policy_id")]
        [ForeignKey("Policy")]
        public int? PolicyId { get; set; }
        public virtual MikrotikRadiusPolicy? Policy { get; set; }

        /// <summary>
        /// ID queue dari Mikrotik API (untuk tracking)
        /// </summary>
        [Column("mikrotik_queue_id")]
        [MaxLength(50)]
        [Display(Name = "Mikrotik Queue ID")]
        public string? MikrotikQueueId { get; set; }

        /// <summary>
        /// Nama queue di Mikrotik
        /// </summary>
        [Column("queue_name")]
        [MaxLength(255)]
        [Display(Name = "Queue Name")]
        public string? QueueName { get; set; }

        /// <summary>
        /// Target IP address / subnet untuk queue
        /// </summary>
        [Column("target_address")]
        [MaxLength(50)]
        [Display(Name = "Target Address")]
        public string? TargetAddress { get; set; }

        /// <summary>
        /// Status deployment
        /// </summary>
        [Column("deployment_status")]
        [Required]
        [Display(Name = "Deployment Status")]
        public EnumDeploymentStatus DeploymentStatus { get; set; } = EnumDeploymentStatus.Pending;

        /// <summary>
        /// Status sinkronisasi
        /// </summary>
        [Column("sync_status")]
        [Display(Name = "Sync Status")]
        public EnumSyncStatus? SyncStatus { get; set; } = EnumSyncStatus.NotSynced;

        /// <summary>
        /// Pesan error terakhir (jika ada)
        /// </summary>
        [Column("last_error")]
        [MaxLength(500)]
        public string? LastError { get; set; }

        /// <summary>
        /// Tanggal deployment
        /// </summary>
        [Column("deployed_date")]
        public DateTime? DeployedDate { get; set; }

        /// <summary>
        /// Tanggal sinkronisasi terakhir
        /// </summary>
        [Column("last_sync_date")]
        public DateTime? LastSyncDate { get; set; }

        /// <summary>
        /// Versi konfigurasi (untuk tracking perubahan)
        /// </summary>
        [Column("config_version")]
        public int ConfigVersion { get; set; } = 1;

        /// <summary>
        /// Metadata JSON untuk parameter queue yang kompleks
        /// </summary>
        [Column("config_metadata")]
        [MaxLength(2000)]
        public string? ConfigMetadata { get; set; }

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

    public enum EnumDeploymentStatus
    {
        [Display(Name = "Pending")]
        Pending = 1,
        [Display(Name = "In Progress")]
        InProgress = 2,
        [Display(Name = "Deployed")]
        Deployed = 3,
        [Display(Name = "Failed")]
        Failed = 4,
        [Display(Name = "Rolled Back")]
        RolledBack = 5
    }

    public enum EnumSyncStatus
    {
        [Display(Name = "Not Synced")]
        NotSynced = 1,
        [Display(Name = "In Sync")]
        InSync = 2,
        [Display(Name = "Out Of Sync")]
        OutOfSync = 3,
        [Display(Name = "Sync Failed")]
        SyncFailed = 4
    }
}
