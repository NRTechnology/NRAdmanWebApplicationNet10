using System.ComponentModel.DataAnnotations;

namespace NRAdmanWebApplicationNet10.Models
{
    public enum EnumRouterType : int
    {
        [Display(Name = "Non Mikrotik")] Non = 0,
        [Display(Name = "Mikrotik")] Mikrotik = 1,
    }
}
