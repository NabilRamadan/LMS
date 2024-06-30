namespace CRUDApi.DTOs
{
    public class StatsDto
    {
        public int activeStudents { get; set; }
        public int activeInstructors { get; set; }
        public int activeCourses { get; set; }
        public List<string>newComers { get; set; }=new List<string>();

    }
}
