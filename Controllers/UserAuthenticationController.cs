using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MovieStoreWeb.Models.DTO;
using MovieStoreWeb.Repositories.Abstract;

namespace MovieStoreWeb.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private IUserAuthenticationService _authenticationService;

        public UserAuthenticationController(IUserAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<IActionResult> Register()
        {
            var model = new RegistrationModel
            {
                Email = "admin@gmail.com",
                Username = "admin",
                Name = "Amin",
                Password = "Admin@123",
                PasswordConfirm = "Admin@123",
                Role = "Admin"
            };
            var result = await _authenticationService.RegisterAsync(model);
            return View(result.Message);
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authenticationService.LoginAsync(model);
            if (result.StatusCode == 1)
                return RedirectToAction("Index", "Home");
            else
            {
                TempData["msg"] = "Could not logged in..";
                return RedirectToAction(nameof(Login));
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
