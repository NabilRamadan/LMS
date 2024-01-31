namespace CRUDApi.Entities
{
    public partial class StudentEnrollment
    {
        public string Id { get; set; } = null!;
        public string CourseCycleId { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }

        public virtual CourseSemester CourseCycle { get; set; } = null!;
        public virtual User Student { get; set; } = null!;
    }
}
