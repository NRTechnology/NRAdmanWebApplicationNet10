using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Table("radusergroup")]
    public class RadUserGroup
    {
        [Key]
        [Column("id")]
        public uint Id { get; set; }

        [Required, MaxLength(64)]
        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [Required, MaxLength(64)]
        [Column("groupname")]
        public string GroupName { get; set; } = string.Empty;

        [Column("priority")]
        public int Priority { get; set; } = 1;
    }

}
