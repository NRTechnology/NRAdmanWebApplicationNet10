using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Table("nas")]
    public class Nas
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nasname")]
        [Required]
        [MaxLength(19)]
        [Display(Name = "NAS Name")]
        public string NasName { get; set; } = "";

        [Column("shortname")]
        [Required]
        [MaxLength(100)]
        [Display(Name = "Short Name")]
        public string ShortName { get; set; } = "";

        [Column("type")]
        [Required]
        [MaxLength(20)]
        [Display(Name = "Type")]
        public string Type { get; set; } = "other";

        [Column("ports")]
        [Display(Name = "Port")]
        public int? Ports { get; set; }

        [Column("secret")]
        [Required]
        [MaxLength(20)]
        [Display(Name = "Secret")]
        public string Secret { get; set; } = "";

        [Column("server")]
        [MaxLength(100)]
        [Display(Name = "Server")]
        public string? Server { get; set; }

        [Column("community")]
        [MaxLength(100)]
        [Display(Name = "Community")]
        public string? Community { get; set; }

        [Column("description")]
        [MaxLength(255)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Column("routertype")] 
        [Required]
        [Display(Name = "Router Type")]
        public EnumRouterType RouterType { get; set; }

        [Column("routerusername")]
        [Required]
        [MaxLength(100)]
        [Display(Name = "Router Username")]
        public string Username { get; set; } = "admin";

        [Column("routerpassword")]
        [Required]
        [MaxLength(100)]
        [Display(Name = "Router Password")]
        public string Password { get; set; } = "admin";
    }
}
