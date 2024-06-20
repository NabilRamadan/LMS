using System.ComponentModel.DataAnnotations;

namespace CRUDApi.DTOs
{
    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)] // Assuming minimum password length
        public string NewPassword { get; set; }

        public string Token { get; set; }
    }
}
