using Microsoft.AspNetCore.Identity;

namespace CRUDApi.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string CurrentUserRole { get; set; }
        public UserAddress Address { get; set; }
    }
}
