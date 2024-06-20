namespace CRUDApi.DTOs
{
    public class StudentsWhoUploadTheTaskDto
    {
        public string studentId {  get; set; }
        public string studentName { get; set; }
        public string filePath {  get; set; }
        public DateTime? timeUploaded { get; set; }
    }
}
