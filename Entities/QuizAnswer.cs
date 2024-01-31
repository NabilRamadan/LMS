namespace CRUDApi.Entities
{
    public partial class QuizAnswer
    {
        public int Id { get; set; }
        public string? StudentId { get; set; }
        public string? QuestionAnswersId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual QuestionAnswer? QuestionAnswers { get; set; }
        public virtual User? Student { get; set; }
    }
}
