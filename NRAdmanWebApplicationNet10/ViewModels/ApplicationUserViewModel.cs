using System.ComponentModel.DataAnnotations;

namespace NRAdmanWebApplicationNet10.ViewModels
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(64)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        public bool EmailConfirmed { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        [Required(ErrorMessage = "Password wajib diisi.")]
        [DataType(DataType.Password)]
        [StringLength(128, MinimumLength = 6, ErrorMessage = "Password minimal 6 karakter dan maksimal 128 karakter.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Konfirmasi password wajib diisi.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password dan konfirmasi password tidak cocok.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new();
    }
}
