using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Table("radreply")]
    public class RadReply
    {
        [Key]
        [Column("id")]
        public uint Id { get; set; }

        [Required, MaxLength(64)]
        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [Required, MaxLength(64)]
        [Column("attribute")]
        public string Attribute { get; set; } = string.Empty;

        [Required, MaxLength(2)]
        [Column("op")]
        public string Op { get; set; } = "=";

        [Required, MaxLength(253)]
        [Column("value")]
        public string Value { get; set; } = string.Empty;
    }
}
