namespace CRUDApi.Entities
{
    public partial class LectureFile
    {
        public int LectureFileId { get; set; }
        public string LectureId { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }

        public virtual Lecture Lecture { get; set; } = null!;
    }
}
