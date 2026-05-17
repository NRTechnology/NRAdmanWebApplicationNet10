using System.ComponentModel.DataAnnotations;

namespace NRAdmanWebApplicationNet10.ViewModels
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username tidak boleh kosong.")]
        [StringLength(64, MinimumLength = 3, ErrorMessage = "Username harus antara 3-64 karakter.")]
        public string UserName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email tidak valid.")]
        [StringLength(100, ErrorMessage = "Email maksimal 100 karakter.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Nomor telepon tidak valid.")]
        [StringLength(20, ErrorMessage = "Nomor telepon maksimal 20 karakter.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(128, MinimumLength = 6, ErrorMessage = "Password harus minimal 6 karakter.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [StringLength(128, ErrorMessage = "Konfirmasi password maksimal 128 karakter.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password dan konfirmasi password tidak cocok.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
