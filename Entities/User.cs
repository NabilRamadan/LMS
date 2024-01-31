namespace CRUDApi.Entities
{
    public partial class User
    {
        public User()
        {
            CourseSemesters = new HashSet<CourseSemester>();
            QuizAnswers = new HashSet<QuizAnswer>();
            Quizzes = new HashSet<Quiz>();
            StudentEnrollments = new HashSet<StudentEnrollment>();
            StudentInfos = new HashSet<StudentInfo>();
            StudentQuizGrades = new HashSet<StudentQuizGrade>();
            TaskAnswers = new HashSet<TaskAnswer>();
            Tasks = new HashSet<Task>();
            Roles = new HashSet<Role>();
        }

        public string UserId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Phone { get; set; }
        public string? ImagePath { get; set; }
        public string? FacultyId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<CourseSemester> CourseSemesters { get; set; }
        public virtual ICollection<QuizAnswer> QuizAnswers { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<StudentEnrollment> StudentEnrollments { get; set; }
        public virtual ICollection<StudentInfo> StudentInfos { get; set; }
        public virtual ICollection<StudentQuizGrade> StudentQuizGrades { get; set; }
        public virtual ICollection<TaskAnswer> TaskAnswers { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
