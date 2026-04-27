using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace NRAdmanWebApplicationNet10.Models
{
    [Table("nas")]
    [Index(nameof(NasName), Name = "nasname")]
    public class Nas
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, MaxLength(128)]
        [Column("nasname")]
        public string NasName { get; set; } = string.Empty;

        [MaxLength(32)]
        [Column("shortname")]
        public string? ShortName { get; set; }

        [MaxLength(30)]
        [Column("type")]
        public string? Type { get; set; } = "other";

        [Column("ports")]
        public int? Ports { get; set; }

        [Required, MaxLength(60)]
        [Column("secret")]
        public string Secret { get; set; } = "secret";

        [MaxLength(64)]
        [Column("server")]
        public string? Server { get; set; }

        [MaxLength(50)]
        [Column("community")]
        public string? Community { get; set; }

        [MaxLength(200)]
        [Column("description")]
        public string? Description { get; set; } = "RADIUS Client";
    }
}
