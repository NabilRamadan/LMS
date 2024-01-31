namespace CRUDApi.Entities
{
    public partial class QuestionAnswer
    {
        public QuestionAnswer()
        {
            QuizAnswers = new HashSet<QuizAnswer>();
        }

        public string Id { get; set; } = null!;
        public string? QuestionId { get; set; }
        public string? Text { get; set; }
        public bool IsCorrect { get; set; }
        public int AnswerNumber { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Question? Question { get; set; }
        public virtual ICollection<QuizAnswer> QuizAnswers { get; set; }
    }
}
