namespace CRUDApi.Entities
{
    public partial class Quiz
    {
        public Quiz()
        {
            Questions = new HashSet<Question>();
            StudentQuizGrades = new HashSet<StudentQuizGrade>();
        }

        public string QuizId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double? Grade { get; set; }
        public string? CourseCycleId { get; set; }
        public string? InstructorId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual CourseSemester? CourseCycle { get; set; }
        public virtual User? Instructor { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<StudentQuizGrade> StudentQuizGrades { get; set; }
    }
}
