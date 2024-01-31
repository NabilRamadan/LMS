namespace CRUDApi.Entities
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public string RoleId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
