namespace CRUDApi.DTOs
{
    public class GetSemestersDto
    {
        public string SemesterId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Years { get; set; }
        public int Number { get; set; }

    }
}
