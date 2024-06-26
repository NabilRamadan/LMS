namespace CRUDApi.DTOs
{
    public class CreateSemesterDto
    {
        public string semesterId { get; set; }
        public string facultyId { get; set; } = "FAC001";
        public DateOnly startDate { get; set; }
        public DateOnly endDate { get; set; }
        public string years { get; set; }
        public int number { get; set; }
        public DateTime? createdAt { get; set; }

    }
}
