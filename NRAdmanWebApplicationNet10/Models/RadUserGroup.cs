using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRAdmanWebApplicationNet10.Models
{
    [Table("radusergroup")]
    //[Index(nameof(UserName))]
    //[Index(nameof(GroupName))]
    public class RadUserGroup
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        [MaxLength(64)]
        public string UserName { get; set; } = "";

        [Column("groupname")]
        [MaxLength(64)]
        public string GroupName { get; set; } = "";

        [Column("priority")]
        public int Priority { get; set; }
    }

}
