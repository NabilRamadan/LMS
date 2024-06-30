namespace CRUDApi.DTOs
{
    public class QuizResultDto
    {
        public string QuestionId { get; set; }
        public bool IsCorrect { get; set; }
        public double? Grade { get; set; }
    }
}
