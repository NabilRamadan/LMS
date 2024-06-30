namespace CRUDApi.DTOs
{
    public class CreateCourseDto
    {
        //public string CourseId { get; set; }
        public string Name { get; set; }
        public int? Hours { get; set; }
        public string ?ImagePath { get; set; }

    }
}
