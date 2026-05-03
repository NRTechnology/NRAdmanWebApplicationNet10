using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Table("radgroupcheck")]
    public class RadGroupCheck
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("groupname")]
        public string GroupName { get; set; } = "";

        [Column("attribute")]
        public string Attribute { get; set; } = "";

        [Column("op")]
        public string Op { get; set; } = "==";

        [Column("value")]
        public string Value { get; set; } = "";
    }
}
