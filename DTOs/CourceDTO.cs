using CRUDApi.Entities;

namespace CRUDApi.DTOs
{
    public partial class CourceDTO
    {
        public string CourseId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int? Hours { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ImagePath { get; set; }
        public string? FacultyId { get; set; }
    }
}
