namespace CRUDApi.DTOs
{
    public class QuizGradeDto
    {
        public string Title { get; set; }
        public double ?studentGrade { get; set; }
        public double ?fullGrade { get; set; }
        public DateTime Date { get; set; }
    }
}
