namespace CRUDApi.Entities
{
    public partial class Lecture
    {
        public Lecture()
        {
            LectureFiles = new HashSet<LectureFile>();
        }

        public string LectureId { get; set; } = null!;
        public string CourseCycleId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }

        public virtual CourseSemester CourseCycle { get; set; } = null!;
        public virtual ICollection<LectureFile> LectureFiles { get; set; }
    }
}
