using System.ComponentModel.DataAnnotations;

namespace CRUDApi.DTOs
{
    public class UpdatePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
