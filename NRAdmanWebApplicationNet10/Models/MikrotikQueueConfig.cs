using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    /// <summary>
    /// Model untuk melacak konfigurasi queue yang diterapkan pada router Mikrotik
    /// </summary>
    public class MikrotikQueueConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Reference ke Router
        /// </summary>
        [Required]
        [ForeignKey(nameof(NetworkRouter))]
        public Guid RouterId { get; set; }

        [Required]
        public virtual NetworkRouter? Router { get; set; }

        /// <summary>
        /// Reference ke Policy
        /// </summary>
        [Required]
        [ForeignKey(nameof(MikrotikRadiusPolicy))]
        public Guid PolicyId { get; set; }

        [Required]
        public virtual MikrotikRadiusPolicy? Policy { get; set; }

        /// <summary>
        /// ID queue dari Mikrotik API (untuk tracking)
        /// </summary>
        [MaxLength(50)]
        [Display(Name = "Mikrotik Queue ID")]
        public string? MikrotikQueueId { get; set; }

        /// <summary>
        /// Nama queue di Mikrotik
        /// </summary>
        [MaxLength(255)]
        [Display(Name = "Queue Name")]
        public string? QueueName { get; set; }

        /// <summary>
        /// Target IP address / subnet untuk queue
        /// </summary>
        [MaxLength(50)]
        [Display(Name = "Target Address")]
        public string? TargetAddress { get; set; }

        /// <summary>
        /// Status deployment
        /// </summary>
        [Required]
        [Display(Name = "Deployment Status")]
        public EnumDeploymentStatus DeploymentStatus { get; set; } = EnumDeploymentStatus.Pending;

        /// <summary>
        /// Status sinkronisasi
        /// </summary>
        [Display(Name = "Sync Status")]
        public EnumSyncStatus? SyncStatus { get; set; } = EnumSyncStatus.NotSynced;

        /// <summary>
        /// Pesan error terakhir (jika ada)
        /// </summary>
        [MaxLength(500)]
        public string? LastError { get; set; }

        /// <summary>
        /// Tanggal deployment
        /// </summary>
        public DateTime? DeployedDate { get; set; }

        /// <summary>
        /// Tanggal sinkronisasi terakhir
        /// </summary>
        public DateTime? LastSyncDate { get; set; }

        /// <summary>
        /// Versi konfigurasi (untuk tracking perubahan)
        /// </summary>
        public int ConfigVersion { get; set; } = 1;

        /// <summary>
        /// Metadata JSON untuk parameter queue yang kompleks
        /// </summary>
        [MaxLength(2000)]
        public string? ConfigMetadata { get; set; }
        
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? ModifiedDate { get; set; }
        
        [MaxLength(255)]
        public string? CreatedBy { get; set; }
        
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
