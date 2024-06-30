namespace CRUDApi.DTOs
{
    public class GetCurrentCourseTasksDto
    {
        public string taskId { get; set; }
        public string taskName { get; set; }
        public string filePath { get; set; }
        public double? grade {  get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int numberOfAllStudents {  get; set; }
        public int numberOfStudentsUploads { get; set;}
    }
}
