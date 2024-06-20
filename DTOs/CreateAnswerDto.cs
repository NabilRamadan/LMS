namespace CRUDApi.DTOs
{
    public class CreateAnswerDto
    {
            public string text { get; set; }
            public bool isCorrect { get; set; }
            public int answerNumber { get; set; }
    }
}
