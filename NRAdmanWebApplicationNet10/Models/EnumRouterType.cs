using System.ComponentModel.DataAnnotations;

namespace NRAdmanWebApplicationNet10.Models
{
    public enum EnumRouterType : int
    {
        [Display(Name = "Non Mikrotik")] Non = 0,
        [Display(Name = "Mikrotik")] Mikrotik = 1,
        [Display(Name = "Cisco")] Cisco = 2,
        [Display(Name = "Juniper")] Juniper = 3,
        [Display(Name = "Fortigate")] Fortigate = 4,
    }
}
