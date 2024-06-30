namespace CRUDApi.DTOs
{
    public class AddGradeForAstudentTaskDto
    {
        public double? Grade { get; set; }
        public string studentId {  get; set; }
        public string taskId {  get; set; }

    }
}
