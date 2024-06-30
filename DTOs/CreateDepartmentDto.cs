namespace CRUDApi.DTOs
{
    public class CreateDepartmentDto
    {
        //public string DepartmentId { get; set; }
        public string FacultyId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
