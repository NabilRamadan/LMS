namespace CRUDApi.DTOs
{
    public class CourseMaterialDto
    {
        public int LectureFileId { get; set; }

        public string fileName { get; set; }
        public string FilePath { get; set; }
        public DateTime? CreatedAt { get; set; }


    }
}
