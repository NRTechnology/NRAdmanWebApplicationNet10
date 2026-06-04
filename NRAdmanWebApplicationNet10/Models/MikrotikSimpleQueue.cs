using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    public class MikrotikSimpleQueue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Required]
        [Display(Name = "Router")]
        [ForeignKey(nameof(Router))]
        public Guid RouterId { get; set; }

        [Required]
        public virtual NetworkRouter? Router { get; set; }
        
        [Required]
        [MaxLength(255)]
        [Display(Name = "Queue Name")]
        public string QueueName { get; set; } = "";
        
        [Required]
        [MaxLength(50)]
        [Display(Name = "Target Address (IP/Subnet)")]
        public string TargetAddress { get; set; } = "";
        
        [MaxLength(255)]
        [Display(Name = "Parent Queue")]
        public string? Parent { get; set; }
                
        [Display(Name = "Max Limit (bps)")]
        public long? MaxLimit { get; set; }
        
        [Display(Name = "Burst Limit (bps)")]
        public long? BurstLimit { get; set; }
        
        [Display(Name = "Burst Threshold (bps)")]
        public long? BurstThreshold { get; set; }
        
        [Display(Name = "Burst Time (seconds)")]
        public int? BurstTime { get; set; }
        
        [Display(Name = "Priority")]
        public int Priority { get; set; } = 8;
        
        [MaxLength(255)]
        [Display(Name = "Packet Mark")]
        public string? PacketMark { get; set; }
        
        [MaxLength(500)]
        [Display(Name = "Comment")]
        public string? Comment { get; set; }
        
        [Display(Name = "Disabled")]
        public bool Disabled { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        
        [MaxLength(450)]
        public string? CreatedBy { get; set; }
        
        [MaxLength(450)]
        public string? UpdatedBy { get; set; }
    }
}
