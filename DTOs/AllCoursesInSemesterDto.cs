namespace CRUDApi.DTOs
{
    public class AllCoursesInSemesterDto
    {
        public string semesterId { get; set; }
        public int semesterNumber { get; set; }

        public List<CourseInSemesterDto> courses { get; set; }

    }
}
