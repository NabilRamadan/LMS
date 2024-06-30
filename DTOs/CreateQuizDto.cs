namespace CRUDApi.DTOs
{
    public class CreateQuizDto
    {
        public string title { get; set; }
        public string notes { get; set; }
        //  public string Duration { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        //public double? grade { get; set; }
        public string courseCycleId { get; set; }
        //public string Status { get; set; }
        //public List<CreateQuestionDto> Questions { get; set; }

        public List<CreateQuistionDto> Questions { get; set; }


    }
}
