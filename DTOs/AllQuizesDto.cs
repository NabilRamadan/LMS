namespace CRUDApi.DTOs
{
    public class AllQuizesDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int numberOfQuestion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }


    }
}
