using System.ComponentModel.DataAnnotations;

namespace CRUDApi.DTOs
{
    public class StudentRigesterDto
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
        //public double? Gpa { get; set; }
        public int Level { get; set; }
        public string DepartmentId { get; set; }
        public string AcademicId { get; set; }
    }
}
