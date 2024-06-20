namespace CRUDApi.DTOs
{
    public class GetCourseDataDto
    {
        public string CourseId { get; set; }
        public string Name { get; set; }
        public int? Hours { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string ImagePath { get; set; }
        public string FacultyId { get; set; }
    }
}
