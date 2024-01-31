namespace CRUDApi.Entities
{
    public partial class Department
    {
        public Department()
        {
            StudentInfos = new HashSet<StudentInfo>();
        }

        public string DepartmentId { get; set; } = null!;
        public string? FacultyId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<StudentInfo> StudentInfos { get; set; }
    }
}
