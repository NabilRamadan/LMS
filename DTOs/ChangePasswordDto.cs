using System.ComponentModel.DataAnnotations;

namespace CRUDApi.DTOs
{
    public class ChangePasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(6)] // Adjust the minimum length as needed
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}
