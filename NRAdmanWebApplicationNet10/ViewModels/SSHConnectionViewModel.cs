using NRAdmanWebApplicationNet10.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NRAdmanWebApplicationNet10.ViewModels
{
    public class SshConnectionViewModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Router Name tidak boleh kosong.")]
        [Display(Name = "Router Name")]
        [MaxLength(255, ErrorMessage = "Router Name maksimal 255 karakter.")]
        public string RouterName { get; set; } = "";

        [Required(ErrorMessage = "Ip Address tidak boleh kosong.")]
        [Display(Name = "IP Address")]
        [StringLength(19, MinimumLength = 7, ErrorMessage = "IP Address harus antara 7-19 karakter.")]
        [IpAddressValidation(ErrorMessage = "Format IP Address atau CIDR tidak valid.")]
        public string IpAddress{ get; set; } = "";

        [Required(ErrorMessage = "Router Name tidak boleh kosong.")]
        [Display(Name = "Router Name")]
        [MaxLength(255, ErrorMessage = "Router Name maksimal 255 karakter.")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Router Port tidak boleh kosong.")]
        [Display(Name = "Router Port")]
        [Range(1, 65535, ErrorMessage = "Router Port harus antara 1-65535.")]
        public int Port { get; set; } = 22;
    }
}
