using Microsoft.AspNetCore.Identity;

namespace NRAdmanWebApplicationNet10.Services
{
    public class ApplicationUser : IdentityUser
    {
        public Guid WargaId { get; set; }

        public byte[]? ProfilePicture { get; set; }
    }
}
