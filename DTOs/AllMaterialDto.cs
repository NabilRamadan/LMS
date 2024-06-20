namespace CRUDApi.DTOs
{
    public class AllMaterialDto
    {
        public string LectureId { get; set; }

        public string LectureName { get; set; }
        public string SemesterName { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string path {  get; set; }

    }
}
