namespace CRUDApi.Entities
{
    public class UserAddress
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser Users { get; set; }
    }
}
