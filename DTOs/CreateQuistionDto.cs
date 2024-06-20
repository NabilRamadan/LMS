namespace CRUDApi.DTOs
{
    public class CreateQuistionDto
    {
        

            //public string QuestionId { get; set; }
            //public string Text { get; set; }
            ////public List<QuestionAnswerDto> Answers { get; set; }
            ///

            public string text { get; set; }
            public string type { get; set; }
            public int questionNumber { get; set; }
            public double? grade { get; set; }
            public List<CreateAnswerDto> Answers { get; set; }


        
    }
}
