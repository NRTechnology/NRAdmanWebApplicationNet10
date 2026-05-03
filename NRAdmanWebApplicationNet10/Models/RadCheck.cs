using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Table("radcheck")]
    public class RadCheck
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        public string UserName { get; set; } = "";

        [Column("attribute")]
        public string Attribute { get; set; } = "";

        [Column("op")]
        public string Op { get; set; } = "==";

        [Column("value")]
        public string Value { get; set; } = "";
    }
}
