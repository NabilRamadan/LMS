namespace CRUDApi.Entities
{
    public partial class Semester
    {
        public Semester()
        {
            CourseSemesters = new HashSet<CourseSemester>();
        }

        public string SemesterId { get; set; } = null!;
        public string? FacultyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Years { get; set; } = null!;
        public int Number { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<CourseSemester> CourseSemesters { get; set; }
    }
}
