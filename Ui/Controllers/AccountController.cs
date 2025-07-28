using BL.Contracts;
using BL.DTOConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ui.Services;

namespace Ui.Controllers
{
    public class AccountController : Controller
    {
        IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(DTOUser user)
        {
            var result = await _userService.LoginAsync(user);
            if (result.Success)
                return RedirectToRoute(new { area = "admin", controller = "Home", action = "Index" });
            else
                return View();
        }
    }
}
