using System.ComponentModel.DataAnnotations;

namespace LibaryAPI.Models
{
    public class RegisterDto
    {
        [Required]
        public string Login { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
