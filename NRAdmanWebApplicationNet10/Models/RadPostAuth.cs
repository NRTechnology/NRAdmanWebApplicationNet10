using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Table("radpostauth")]
    public class RadPostAuth
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, MaxLength(64)]
        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [Required, MaxLength(128)]
        [Column("pass")]
        public string Pass { get; set; } = string.Empty;

        [Required, MaxLength(32)]
        [Column("reply")]
        public string Reply { get; set; } = string.Empty;

        [Column("authdate")]
        public DateTime AuthDate { get; set; }

        [MaxLength(64)]
        [Column("class")]
        public string? Class { get; set; }
    }
}
