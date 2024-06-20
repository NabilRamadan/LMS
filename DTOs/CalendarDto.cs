namespace CRUDApi.DTOs
{
    public class CalendarDto
    {
        public string CalendarId { get; set; }
        public string UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Body { get; set; }
    }
}
