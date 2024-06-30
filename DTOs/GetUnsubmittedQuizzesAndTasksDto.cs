namespace CRUDApi.DTOs
{
    public class GetUnsubmittedQuizzesAndTasksDto
    {
            public string title {  get; set; }

            public DateTime? startDate { get; set; }
            public DateTime? endDate { get; set; }
            public double? grade { get; set; }
            public DateTime? createdAt { get; set; }
    }
}
