using Microsoft.AspNetCore.Identity;

namespace CRUDApi.Entities
{
    public class ApplicationUserRole : IdentityRole
    {

        public ApplicationUserRole() : base()
        {
        }
        public ApplicationUserRole(string roleName) : base(roleName)
        {
        }

    }
}
