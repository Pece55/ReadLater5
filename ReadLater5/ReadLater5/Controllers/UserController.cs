using Microsoft.AspNetCore.Mvc;
using ReadLater5.Models;
using System.Linq;
using System.Threading.Tasks;
using Services.Interface;

namespace ReadLater5.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly ICategoryService categoryService;

        public UserController(IUserService userService, ICategoryService categoryService)
        {
            this.userService = userService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> RegisterUser(RegisterModel registerModel)
        {
            if (await userService.ValidateUsername(registerModel.Username))
                return RedirectToAction("Register", new RegisterModel { RegisterMessage = $"Username {registerModel.Username} is already taken" });

            await userService.RegisterUserAsync(DTO.AutoMapper.MapProfile.RegisterModelToUser(registerModel));

            return RedirectToAction("Login");
        }

        public IActionResult Register(RegisterModel registerModel)
        {
            return View(registerModel);
        }

        //Uncomment the seeder code in order to get some random data
        //when you first launch the web app
        //This is the initial view if user in not logged in
        public async Task<IActionResult> Login(bool wrongCredentials = false)
        {
            //var seeder = new Seed(categoryService, bookmarkService, userService);
            //await seeder.SeedDB();
            var loginModel = new LoginModel()
            {
                ExternalLogins = (await userService.GetExternalAuthenticationSchemesAsync()).ToList(),
                WrongCredentials = wrongCredentials
            };

            return View(loginModel);
        }

        public async Task<IActionResult> Logout()
        {
            await userService.LogoutAsync();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Authenticate(LoginModel loginModel)
        {
            if (ModelState.IsValid && await userService.Authenticate(loginModel.Username, loginModel.Password))
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("Login", new { WrongCredentials = true });
        }

        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "User");
            var properties = userService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallback(string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Login", new LoginModel
                {
                    ExternalLogins = (await userService.GetExternalAuthenticationSchemesAsync()).ToList()
                });
            }

            var isSuccess = await userService.HandleExternalLoginAndCallbackAsync();
            if (isSuccess)
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Error", "Home", new { ErrorMessage = "Please contact support on Pragim@PragimTech.com" });
        }
    }
}
