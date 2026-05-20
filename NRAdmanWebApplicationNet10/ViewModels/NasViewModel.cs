using NRAdmanWebApplicationNet10.Attributes;
using NRAdmanWebApplicationNet10.Models;
using System.ComponentModel.DataAnnotations;

namespace NRAdmanWebApplicationNet10.ViewModels
{
    public class NasViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "NAS Name tidak boleh kosong.")]
        [Display(Name = "NAS Name")]
        [StringLength(19, MinimumLength = 7, ErrorMessage = "NAS Name harus antara 7-19 karakter.")]
        [IpAddressValidation(ErrorMessage = "Format IP Address atau CIDR tidak valid.")]
        public string NasName { get; set; } = "";

        [Required(ErrorMessage = "Short Name tidak boleh kosong.")]
        [Display(Name = "Short Name")]
        [MaxLength(100, ErrorMessage = "Short Name maksimal 100 karakter.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Short Name harus antara 5-100 karakter.")]
        public string ShortName { get; set; } = "";

        [Required(ErrorMessage = "Type tidak boleh kosong.")]
        [Display(Name = "Type")]
        [MaxLength(20, ErrorMessage = "Type maksimal 20 karakter.")]
        public string Type { get; set; } = "other";

        [Required(ErrorMessage = "Port tidak boleh kosong.")] 
        [Display(Name = "Port")]
        [Range(1, 65535, ErrorMessage = "Port harus antara 1-65535.")]
        public int Ports { get; set; } = 1812;

        [Required(ErrorMessage = "Secret tidak boleh kosong.")]
        [Display(Name = "Secret")]
        [MaxLength(20, ErrorMessage = "Secret maksimal 20 karakter.")]
        public string Secret { get; set; } = "";

        [Display(Name = "Server")]
        [MaxLength(100, ErrorMessage = "Server maksimal 100 karakter.")]
        public string? Server { get; set; }

        [Display(Name = "SNMP Community")]
        [MaxLength(100, ErrorMessage = "Community maksimal 100 karakter.")]
        public string? Community { get; set; }

        [Display(Name = "Description")]
        [MaxLength(255, ErrorMessage = "Description maksimal 255 karakter.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Router Type harus dipilih.")]
        [Display(Name = "Router Type")]
        public EnumRouterType RouterType { get; set; } = EnumRouterType.Mikrotik;

        [Required(ErrorMessage = "Router Username tidak boleh kosong.")]
        [Display(Name = "Router Username")]
        [MaxLength(100, ErrorMessage = "Router Username maksimal 100 karakter.")]
        public string Username { get; set; } = "admin";

        [Required(ErrorMessage = "Router Password tidak boleh kosong.")]
        [Display(Name = "Router Password")]
        [MaxLength(100, ErrorMessage = "Router Password maksimal 100 karakter.")]
        public string Password { get; set; } = "admin";

        [Required(ErrorMessage = "Router Port tidak boleh kosong.")]
        [Display(Name = "Router Port")]
        [Range(1, 65535, ErrorMessage = "Router Port harus antara 1-65535.")]
        public int? RouterPorts { get; set; } = 22;
    }
}
