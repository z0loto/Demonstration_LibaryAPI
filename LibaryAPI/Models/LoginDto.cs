using System.ComponentModel.DataAnnotations;

namespace LibaryAPI.Models
{
    public class LoginDto
    {
        [Required]
        public string Login { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
