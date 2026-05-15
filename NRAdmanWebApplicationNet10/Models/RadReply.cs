using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Table("radreply")]
    public class RadReply
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        [MaxLength(64)]
        public string UserName { get; set; } = "";

        [Column("attribute")]
        [MaxLength(64)]
        public string Attribute { get; set; } = "";

        [Column("op")]
        [MaxLength(4)]
        public string Op { get; set; } = "=";

        [Column("value")]
        [MaxLength(253)]
        public string Value { get; set; } = "";
    }
}
