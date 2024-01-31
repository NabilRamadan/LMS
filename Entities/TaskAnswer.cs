namespace CRUDApi.Entities
{
    public partial class TaskAnswer
    {
        public string TaskAnswerId { get; set; } = null!;
        public string? TaskId { get; set; }
        public string? StudentId { get; set; }
        public string FilePath { get; set; } = null!;
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual User? Student { get; set; }
        public virtual Task? Task { get; set; }
    }
}
