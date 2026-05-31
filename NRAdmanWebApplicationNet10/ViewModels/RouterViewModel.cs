using NRAdmanWebApplicationNet10.Attributes;
using NRAdmanWebApplicationNet10.Models;
using System.ComponentModel.DataAnnotations;

namespace NRAdmanWebApplicationNet10.ViewModels
{
    public class RouterViewModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Router Type harus dipilih.")]
        [Display(Name = "Router Type")]
        public EnumRouterType RouterType { get; set; } = EnumRouterType.Mikrotik;

        [Required(ErrorMessage = "Ip Address tidak boleh kosong.")]
        [Display(Name = "IP Address")]
        [StringLength(19, MinimumLength = 7, ErrorMessage = "IP Address harus antara 7-19 karakter.")]
        [IpAddressValidation(ErrorMessage = "Format IP Address atau CIDR tidak valid.")]
        public string IpAddress { get; set; } = "";

        [Required(ErrorMessage = "Router Name tidak boleh kosong.")]
        [Display(Name = "Router Name")]
        [MaxLength(255, ErrorMessage = "Router Name maksimal 255 karakter.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Router Username tidak boleh kosong.")]
        [Display(Name = "Router Username")]
        [MaxLength(100, ErrorMessage = "Router Username maksimal 100 karakter.")]
        public string Username { get; set; } = "admin";

        [Required(ErrorMessage = "Router Password tidak boleh kosong.")]
        [Display(Name = "Router Password")]
        [MaxLength(100, ErrorMessage = "Router Password maksimal 100 karakter.")]
        public string Password { get; set; } = "admin";

        [Required(ErrorMessage = "Router SSH Port tidak boleh kosong.")]
        [Display(Name = "Router SSH Port")]
        [Range(1, 65535, ErrorMessage = "Router SSH Port harus antara 1-65535.")]
        public int SshPorts { get; set; } = 22;

        [Required(ErrorMessage = "Router Port tidak boleh kosong.")]
        [Display(Name = "Router API Port")]
        [Range(1, 65535, ErrorMessage = "Router API Port harus antara 1-65535.")]
        public int ApiPorts { get; set; } = 8729;

        [Display(Name = "Description")]
        [MaxLength(255, ErrorMessage = "Description maksimal 255 karakter.")]
        public string? Description { get; set; }
    }
}
