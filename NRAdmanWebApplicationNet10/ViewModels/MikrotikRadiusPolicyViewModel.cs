using NRAdmanWebApplicationNet10.Models;
using System.ComponentModel.DataAnnotations;

namespace NRAdmanWebApplicationNet10.ViewModels
{
    /// <summary>
    /// ViewModel untuk Mikrotik RADIUS Policy Management
    /// </summary>
    public class MikrotikRadiusPolicyViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Policy Name tidak boleh kosong.")]
        [Display(Name = "Policy Name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Policy Name harus antara 3-100 karakter.")]
        public string PolicyName { get; set; } = "";

        [Display(Name = "Description")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Display(Name = "Download Limit (Mbps)")]
        [Range(0.1, 10000, ErrorMessage = "Download limit harus antara 0.1-10000.")]
        public decimal? DownloadLimit { get; set; }

        [Display(Name = "Upload Limit (Mbps)")]
        [Range(0.1, 10000, ErrorMessage = "Upload limit harus antara 0.1-10000.")]
        public decimal? UploadLimit { get; set; }

        [Required(ErrorMessage = "Queue Type harus dipilih.")]
        [Display(Name = "Queue Type")]
        public EnumQueueType QueueType { get; set; } = EnumQueueType.SimpleQueue;

        [Required(ErrorMessage = "Priority harus dipilih.")]
        [Display(Name = "Priority (1=Highest, 16=Lowest)")]
        [Range(1, 16, ErrorMessage = "Priority harus antara 1-16.")]
        public int Priority { get; set; } = 8;

        [Display(Name = "Burst Download Limit (Mbps)")]
        [Range(0, 10000, ErrorMessage = "Burst limit harus antara 0-10000.")]
        public decimal? BurstLimitDown { get; set; }

        [Display(Name = "Burst Upload Limit (Mbps)")]
        [Range(0, 10000, ErrorMessage = "Burst limit harus antara 0-10000.")]
        public decimal? BurstLimitUp { get; set; }

        [Display(Name = "Burst Threshold Down (%)")]
        [Range(1, 100, ErrorMessage = "Threshold harus antara 1-100%.")]
        public int? BurstThresholdDown { get; set; }

        [Display(Name = "Burst Threshold Up (%)")]
        [Range(1, 100, ErrorMessage = "Threshold harus antara 1-100%.")]
        public int? BurstThresholdUp { get; set; }

        [Display(Name = "Burst Time (seconds)")]
        [Range(1, 3600, ErrorMessage = "Burst time harus antara 1-3600 detik.")]
        public int? BurstTime { get; set; }

        [Display(Name = "Max Limit Download (Mbps)")]
        [Range(0, 10000, ErrorMessage = "Max limit harus antara 0-10000.")]
        public decimal? MaxLimitDown { get; set; }

        [Display(Name = "Max Limit Upload (Mbps)")]
        [Range(0, 10000, ErrorMessage = "Max limit harus antara 0-10000.")]
        public decimal? MaxLimitUp { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        // Read-only fields
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Created By")]
        public string? CreatedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Modified By")]
        public string? ModifiedBy { get; set; }
    }
}
