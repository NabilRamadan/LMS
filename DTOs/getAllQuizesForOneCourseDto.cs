namespace CRUDApi.DTOs
{
    public class getAllQuizesForOneCourseDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public double? grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public int numberOfAllStudents { get; set; }
        public int numberOfStudentsSolve { get; set; }
    }
}
