public class QuizDto
{

    public string Id { get; set; }
    public string Title { get; set; }
    public string Notes { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double? Grade { get; set; }
    public string CourseId { get; set; }
    public string InstructorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
    public List<QuestionDto> Questions { get; set; }



}