using Data;
using Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Interface;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly ReadLaterDataContext context;
        private readonly SignInManager<User> signInManager;

        public UserService(UserManager<User> userManager, ReadLaterDataContext context, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.context = context;
            this.signInManager = signInManager;
        }

        public Task<List<User>> GetAll()
        {
            return userManager.Users.ToListAsync();
        }

        public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return await signInManager.GetExternalAuthenticationSchemesAsync();
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            return signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public async Task<IdentityResult> RegisterUserAsync(User user)
        {
            return await userManager.CreateAsync(user, user.PasswordHash);
        }

        public async Task<bool> ValidateUsername(string userName)
        {
            return await context.Users.AnyAsync(x => x.UserName == userName);
        }

        public async Task<bool> Authenticate(string userName, string password)
        {
            var user = await userManager.FindByNameAsync(userName);
            var canAuthenticate = await userManager.CheckPasswordAsync(user, password);

            if (canAuthenticate)
            {
                await signInManager.SignInAsync(user, true);
                return canAuthenticate;
            }

            return canAuthenticate;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<bool> HandleExternalLoginAndCallbackAsync()
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return false;
            }

            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            if (signInResult.Succeeded)
            {
                return true;
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new User
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };
                        await userManager.CreateAsync(user);
                    }

                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, false);

                    return true;
                }

                return false;
            }
        }
    }
}
