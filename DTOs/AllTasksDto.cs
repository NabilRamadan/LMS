namespace CRUDApi.DTOs
{
    public class AllTasksDto
    {
        public string taskId { get; set; }
        public string taskName { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string status { get; set; }

    }
}
