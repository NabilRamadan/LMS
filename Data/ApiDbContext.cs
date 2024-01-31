using CRUDApi.Entities;
using Microsoft.EntityFrameworkCore;
using Task = CRUDApi.Entities.Task;

namespace CRUDApi.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) 
        {

        }

       


        public DbSet<Course> Courses { get; set; } 
        public  DbSet<CourseSemester> CourseSemesters { get; set; } 
        public  DbSet<Department> Departments { get; set; } 
        public  DbSet<Lecture> Lectures { get; set; }
        public  DbSet<LectureFile> LectureFiles { get; set; } 
        public  DbSet<Question> Questions { get; set; } 
        public  DbSet<QuestionAnswer> QuestionAnswers { get; set; } 
        public  DbSet<Quiz> Quizzes { get; set; } 
        public  DbSet<QuizAnswer> QuizAnswers { get; set; } 
        public  DbSet<Role> Roles { get; set; }
        public  DbSet<Semester> Semesters { get; set; }
        public  DbSet<Student> Students { get; set; } 
        public  DbSet<StudentEnrollment> StudentEnrollments { get; set; } 
        public  DbSet<StudentInfo> StudentInfos { get; set; } 
        public  DbSet<StudentQuizGrade> StudentQuizGrades { get; set; } 
        public  DbSet<Task> Tasks { get; set; }
        public  DbSet<TaskAnswer> TaskAnswers { get; set; } 
        public  DbSet<University> Universities { get; set; } 
        public  DbSet<User> Users { get; set; } 
    }
}
