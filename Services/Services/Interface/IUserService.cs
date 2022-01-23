using Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(User user);
        Task<bool> ValidateUsername(string userName);
        Task<bool> Authenticate(string userName, string password);
        Task LogoutAsync();
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
        Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
        Task<bool> HandleExternalLoginAndCallbackAsync();
        Task<List<User>> GetAll();
    }
}
