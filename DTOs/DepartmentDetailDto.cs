namespace CRUDApi.DTOs
{
    public class DepartmentDetailDto
    {
        public string DepartmentId { get; set; }
        public string FacultyId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<StudentDepartmentInfoDto> Students { get; set; }

    }
}
