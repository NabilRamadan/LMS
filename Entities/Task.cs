namespace CRUDApi.Entities
{
    public partial class Task
    {
        public Task()
        {
            TaskAnswers = new HashSet<TaskAnswer>();
        }

        public string TaskId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double? Grade { get; set; }
        public string FilePath { get; set; } = null!;
        public string? CourseCycleId { get; set; }
        public string? InstructorId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual CourseSemester? CourseCycle { get; set; }
        public virtual User? Instructor { get; set; }
        public virtual ICollection<TaskAnswer> TaskAnswers { get; set; }
    }
}
