using System.ComponentModel.DataAnnotations;

namespace CRUDApi.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string RoleOfUser { get; set; }
    }
}
