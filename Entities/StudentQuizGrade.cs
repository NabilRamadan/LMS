namespace CRUDApi.Entities
{
    public partial class StudentQuizGrade
    {
        public string Id { get; set; } = null!;
        public string QuizId { get; set; } = null!;
        public double? Grade { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Quiz Quiz { get; set; } = null!;
        public virtual User Student { get; set; } = null!;
    }
}
