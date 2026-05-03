using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Table("radusergroup")]
    public class RadUserGroup
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        public string UserName { get; set; } = "";

        [Column("groupname")]
        public string GroupName { get; set; } = "";

        [Column("priority")]
        public int Priority { get; set; }
    }

}
