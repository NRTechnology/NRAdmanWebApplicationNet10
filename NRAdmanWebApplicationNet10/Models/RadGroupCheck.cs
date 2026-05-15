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
        [StringLength(64)]
        public string GroupName { get; set; } = "";

        [Column("attribute")]
        [StringLength(64)]
        public string Attribute { get; set; } = "";

        [Column("op")]
        [StringLength(4)]
        public string Op { get; set; } = "==";

        [Column("value")]
        [StringLength(253)]
        public string Value { get; set; } = "";
    }
}
