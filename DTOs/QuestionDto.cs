public class QuestionDto
{

    //public string QuestionId { get; set; }
    //public string Text { get; set; }
    ////public List<QuestionAnswerDto> Answers { get; set; }
    ///

    public string id { get; set; }
    public string text { get; set; }
    public string type { get; set; }
    public int questionNumber { get; set; }
    public double? grade { get; set; }
    public DateTime createdAt { get; set; }
    public List<AnswerDto> Answers { get; set; }


}