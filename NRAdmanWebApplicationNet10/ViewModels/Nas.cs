using NRAdmanWebApplicationNet10.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.ViewModels
{
    public class Nas
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "NAS Name")]
        [MaxLength(19)]
        [StringLength(19, MinimumLength = 7, ErrorMessage = "NAS format tidak valid")]
        public string NasName { get; set; } = "";
        
        [Required]
        [Display(Name = "Short Name")]
        [MaxLength(100)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Short Name format tidak valid")]
        public string ShortName { get; set; } = "";
        
        [Required]
        [Display(Name = "Type")]
        [MaxLength(20)]
        public string Type { get; set; } = "other";

        [Required] 
        [Display(Name = "Port")] 
        public int? Ports { get; set; } = 1812;
        
        [Required]
        [Display(Name = "Secret")]
        [MaxLength(20)]
        public string Secret { get; set; } = "";
        
        
        [Display(Name = "Server")]
        [MaxLength(100)]
        public string? Server { get; set; }
        
        [Display(Name = "SNMP Community")]
        [MaxLength(100)]
        public string? Community { get; set; }

        
        [Display(Name = "Description")]
        [MaxLength(255)]
        public string? Description { get; set; }


        [Required]
        [Display(Name = "Router Type")]
        public EnumRouterType RouterType { get; set; } = EnumRouterType.Mikrotik;

        [Required]
        [Display(Name = "Router Username")]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Router Password")]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;

    }
}
