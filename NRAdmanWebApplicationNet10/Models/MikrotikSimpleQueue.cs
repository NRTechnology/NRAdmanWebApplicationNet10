using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Table("mikrotik_simple_queues")]
    public class MikrotikSimpleQueue
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nas_id")]
        [Required]
        [Display(Name = "NAS")]
        public int NasId { get; set; }

        [ForeignKey("NasId")]
        public virtual Nas? Nas { get; set; }

        [Column("queue_name")]
        [Required]
        [MaxLength(255)]
        [Display(Name = "Queue Name")]
        public string QueueName { get; set; } = "";

        [Column("target_address")]
        [Required]
        [MaxLength(50)]
        [Display(Name = "Target Address (IP/Subnet)")]
        public string TargetAddress { get; set; } = "";

        [Column("parent")]
        [MaxLength(255)]
        [Display(Name = "Parent Queue")]
        public string? Parent { get; set; }

        [Column("max_limit")]
        [Display(Name = "Max Limit (bps)")]
        public long? MaxLimit { get; set; }

        [Column("burst_limit")]
        [Display(Name = "Burst Limit (bps)")]
        public long? BurstLimit { get; set; }

        [Column("burst_threshold")]
        [Display(Name = "Burst Threshold (bps)")]
        public long? BurstThreshold { get; set; }

        [Column("burst_time")]
        [Display(Name = "Burst Time (seconds)")]
        public int? BurstTime { get; set; }

        [Column("priority")]
        [Display(Name = "Priority")]
        public int Priority { get; set; } = 8;

        [Column("packet_mark")]
        [MaxLength(255)]
        [Display(Name = "Packet Mark")]
        public string? PacketMark { get; set; }

        [Column("comment")]
        [MaxLength(500)]
        [Display(Name = "Comment")]
        public string? Comment { get; set; }

        [Column("disabled")]
        [Display(Name = "Disabled")]
        public bool Disabled { get; set; } = false;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("created_by")]
        [MaxLength(450)]
        public string? CreatedBy { get; set; }

        [Column("updated_by")]
        [MaxLength(450)]
        public string? UpdatedBy { get; set; }
    }
}
