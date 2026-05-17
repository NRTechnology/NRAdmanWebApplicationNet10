using System.ComponentModel.DataAnnotations;

namespace NRAdmanWebApplicationNet10.ViewModels
{
    public class MikrotikSimpleQueueViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "NAS harus dipilih.")]
        [Display(Name = "NAS")]
        public int NasId { get; set; }

        [Required(ErrorMessage = "Queue Name tidak boleh kosong.")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Queue Name harus antara 3-255 karakter.")]
        [Display(Name = "Queue Name")]
        public string QueueName { get; set; } = "";

        [Required(ErrorMessage = "Target Address tidak boleh kosong.")]
        [StringLength(50, ErrorMessage = "Target Address maksimal 50 karakter.")]
        [Display(Name = "Target Address (IP/Subnet)")]
        [RegularExpression(@"^(\d{1,3}\.){3}\d{1,3}(/\d{1,2})?$", ErrorMessage = "Format IP Address atau Subnet tidak valid.")]
        public string TargetAddress { get; set; } = "";

        [StringLength(255, ErrorMessage = "Parent Queue maksimal 255 karakter.")]
        [Display(Name = "Parent Queue")]
        public string? Parent { get; set; }

        [Display(Name = "Max Limit (bps)")]
        [Range(0, long.MaxValue, ErrorMessage = "Max Limit harus bernilai positif atau 0.")]
        public long? MaxLimit { get; set; }

        [Display(Name = "Burst Limit (bps)")]
        [Range(0, long.MaxValue, ErrorMessage = "Burst Limit harus bernilai positif atau 0.")]
        public long? BurstLimit { get; set; }

        [Display(Name = "Burst Threshold (bps)")]
        [Range(0, long.MaxValue, ErrorMessage = "Burst Threshold harus bernilai positif atau 0.")]
        public long? BurstThreshold { get; set; }

        [Display(Name = "Burst Time (seconds)")]
        [Range(0, int.MaxValue, ErrorMessage = "Burst Time harus bernilai positif atau 0.")]
        public int? BurstTime { get; set; }

        [Required(ErrorMessage = "Priority harus dipilih.")]
        [Range(0, 16, ErrorMessage = "Priority harus antara 0-16.")]
        [Display(Name = "Priority")]
        public int Priority { get; set; } = 8;

        [StringLength(255, ErrorMessage = "Packet Mark maksimal 255 karakter.")]
        [Display(Name = "Packet Mark")]
        public string? PacketMark { get; set; }

        [StringLength(500, ErrorMessage = "Comment maksimal 500 karakter.")]
        [Display(Name = "Comment")]
        public string? Comment { get; set; }

        [Display(Name = "Disabled")]
        public bool Disabled { get; set; } = false;

        // Additional property for UI
        [Display(Name = "NAS Name")]
        public string? NasName { get; set; }
    }
}
