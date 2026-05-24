using NRAdmanWebApplicationNet10.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    public class Router
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Display(Name = "Router Type")]
        public EnumRouterType RouterType { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Router Name")]
        public string Name { get; set; } = "";

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

        [MaxLength(255)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Last Modified Date")]
        public DateTime? LastModifiedDate { get; set; }
    }
}
