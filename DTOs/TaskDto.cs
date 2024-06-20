namespace CRUDApi.DTOs
{
    public class TaskDto
    {
        public string taskId { get; set; }
        public string taskName { get; set; }
        public double? taskGrade { get; set; } // Nullable for tasks without grades
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string status { get; set; }
        public string courseName { get; set; }
        public string filePath { get; set; }
        public string instructorName { get; set; }
        public DateTime ?createdAt { get; set; }  // Set default value to DateTime.Now

    }

}
