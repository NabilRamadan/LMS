namespace CRUDApi.Entities
{
    public partial class StudentInfo
    {
        public string Id { get; set; } = null!;
        public string? UserId { get; set; }
        public string? DepartmentId { get; set; }
        public int Level { get; set; }
        public double? Gpa { get; set; }

        public virtual Department? Department { get; set; }
        public virtual User? User { get; set; }
    }
}
