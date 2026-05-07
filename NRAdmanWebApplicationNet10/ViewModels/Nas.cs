using NRAdmanWebApplicationNet10.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.ViewModels
{
    public class Nas
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Nama Desa")]
        public string Nama { get; set; } = string.Empty;
        
        
        [Required]
        [Display(Name = "NAS Name")]
        public string NasName { get; set; } = "";
        
        [Required]
        [Display(Name = "Short Name")]
        public string ShortName { get; set; } = "";
        
        [Required]
        [Display(Name = "Type")]
        public string Type { get; set; } = "other";

        [Required] 
        [Display(Name = "Port")] 
        public int? Ports { get; set; } = 1812;
        
        [Required]
        [Display(Name = "Secret")]
        public string Secret { get; set; } = "";
        
        
        [Display(Name = "Server")]
        public string? Server { get; set; }
        
        [Display(Name = "SNMP Community")]
        public string? Community { get; set; }

        
        [Display(Name = "Description")]
        public string? Description { get; set; }


        [Required]
        [Display(Name = "Router Type")]
        public EnumRouterType RouterType { get; set; } = EnumRouterType.Mikrotik;

        [Required]
        [Display(Name = "Router Username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Router Password")]
        public string Password { get; set; } = string.Empty;

    }
}
