namespace CRUDApi.DTOs
{
    public class QuizSubmissionResponseDto
    {
        public List<QuizResultDto> results { get; set; }
        public double? totalGrade { get; set; }
        public double? totalStudentGrade { get; set;}
    }

}
