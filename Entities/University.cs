namespace CRUDApi.Entities
{
    public partial class University
    {
        public University()
        {
        }

        public string UniversityId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string LogoPath { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }

    }
}
