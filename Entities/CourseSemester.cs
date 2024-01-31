namespace CRUDApi.Entities
{
    public partial class CourseSemester
    {
        public CourseSemester()
        {
            Lectures = new HashSet<Lecture>();
            Quizzes = new HashSet<Quiz>();
            StudentEnrollments = new HashSet<StudentEnrollment>();
            Tasks = new HashSet<Task>();
        }

        public string Id { get; set; } = null!;
        public string SemesterId { get; set; } = null!;
        public string CourseId { get; set; } = null!;
        public string? InstructorId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual User? Instructor { get; set; }
        public virtual Semester Semester { get; set; } = null!;
        public virtual ICollection<Lecture> Lectures { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<StudentEnrollment> StudentEnrollments { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
