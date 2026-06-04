using System.ComponentModel.DataAnnotations;

namespace NRAdmanWebApplicationNet10.ViewModels
{
    public class MikrotikRadiusPolicyViewModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Policy Name wajib diisi.")]
        [Display(Name = "Policy Name")]
        [MaxLength(255)]
        public string PolicyName { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [MaxLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Download Limit wajib diisi.")]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Download Limit harus lebih besar dari 0.")]
        [Display(Name = "Download Limit (Mbps)")]
        public decimal DownloadLimit { get; set; }

        [Required(ErrorMessage = "Upload Limit wajib diisi.")]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Upload Limit harus lebih besar dari 0.")]
        [Display(Name = "Upload Limit (Mbps)")]
        public decimal UploadLimit { get; set; }

        [Display(Name = "Enable Burst")]
        public bool EnableBurst { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Burst Download (Mbps)")]
        public decimal? BurstLimitDown { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Burst Upload (Mbps)")]
        public decimal? BurstLimitUp { get; set; }

        [Range(0, 100)]
        [Display(Name = "Burst Threshold Download (%)")]
        public int? BurstThresholdDown { get; set; }

        [Range(0, 100)]
        [Display(Name = "Burst Threshold Upload (%)")]
        public int? BurstThresholdUp { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Burst Time (Seconds)")]
        public int? BurstTime { get; set; }

        [Required(ErrorMessage = "Priority wajib diisi.")]
        [Range(1, 8,
            ErrorMessage = "Priority harus antara 1 sampai 8.")]
        [Display(Name = "Priority")]
        public int Priority { get; set; } = 8;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        //
        // Tampilan saja (tidak disimpan)
        //

        public DateTimeOffset? CreatedDate { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }
    }
}
