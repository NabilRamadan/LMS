namespace CRUDApi.Entities
{
    public partial class Question
    {
        public Question()
        {
            QuestionAnswers = new HashSet<QuestionAnswer>();
        }

        public string QuestionId { get; set; } = null!;
        public string? QuizId { get; set; }
        public string Text { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int QuestionNumber { get; set; }
        public double? Grade { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Quiz? Quiz { get; set; }
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
    }
}
