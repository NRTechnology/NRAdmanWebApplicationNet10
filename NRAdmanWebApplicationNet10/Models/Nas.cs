using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace NRAdmanWebApplicationNet10.Models
{
    [Table("nas")]
    public class Nas
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nasname")]
        public string NasName { get; set; } = "";

        [Column("shortname")]
        public string ShortName { get; set; } = "";

        [Column("type")]
        public string Type { get; set; } = "other";

        [Column("ports")]
        public int? Ports { get; set; }

        [Column("secret")]
        public string Secret { get; set; } = "";

        [Column("server")]
        public string? Server { get; set; }

        [Column("community")]
        public string? Community { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("routertype")] 
        public EnumRouterType RouterType { get; set; }

        [Column("routerusername")]
        public string Username { get; set; }

        [Column("routerpassword")]
        public string Password { get; set; }
    }
}
