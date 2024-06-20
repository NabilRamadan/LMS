namespace CRUDApi.DTOs
{
    public class QuizAnswerRequestDto
    {
        public string QuizId { get; set; }
        public List<QuestionAnswerRequestDto> Answers { get; set; }

    }
}
