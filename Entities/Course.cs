namespace CRUDApi.Entities
{
    public partial class Course
    {
        public Course()
        {
            CourseSemesters = new HashSet<CourseSemester>();
        }

        public string CourseId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int? Hours { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ImagePath { get; set; }
        public string? FacultyId { get; set; }

        public virtual ICollection<CourseSemester> CourseSemesters { get; set; }

    }
}
