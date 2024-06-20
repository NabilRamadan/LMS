namespace CRUDApi.DTOs
{
    public class UploadAssignmentDto
    {
        public string? TaskName { get; set; }
        public double? TaskGrade { get; set; } // Nullable for tasks without grades
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CourseCycleId { get; set; }
    }
}
