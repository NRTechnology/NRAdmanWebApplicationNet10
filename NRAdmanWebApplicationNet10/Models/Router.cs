using NRAdmanWebApplicationNet10.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    public class Router
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
                
        [Required]
        [Display(Name = "Router Type")]
        public EnumRouterType RouterType { get; set; }

        [Required]
        [MaxLength(19)]        
        [IpAddressValidation(ErrorMessage = "Format IP Address atau CIDR tidak valid.")]
        public string IpAddress { get; set; } = "";

        [Required]
        [MaxLength(100)]
        [Display(Name = "Router Username")]
        public string Username { get; set; } = "admin";

        [Required]
        [MaxLength(100)]
        [Display(Name = "Router Password")]
        public string Password { get; set; } = "admin";

        [Required]
        [Display(Name = "Router Port")]
        public int Ports { get; set; } = 22;
    }
}
