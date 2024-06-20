using CRUDApi.Entities;
using Microsoft.AspNetCore.Identity;

namespace CRUDApi.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager);

    }
}
