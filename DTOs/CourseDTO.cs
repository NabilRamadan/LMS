namespace CRUDApi.DTOs
{
    public class CourseDTO
    {
        public string CycleId { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }
        public string ImagePath { get; set; }
        public string InstructorFullName { get; set; }
    }
}
