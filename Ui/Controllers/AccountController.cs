using BL.Contracts;
using BL.DTOConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ui.Models;
using Ui.Services;

namespace Ui.Controllers
{
    public class AccountController : Controller
    {
        IUserService _userService;
        private readonly GenericApiClient _apiClient;
        
        public AccountController(IUserService userService, GenericApiClient apiClient)
        {
            _userService = userService;
            _apiClient = apiClient;
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

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                // First try local authentication
                var localResult = await _userService.LoginAsync(user);
                if (localResult.Success)
                {
                    // If local auth succeeds, redirect to admin
                    return RedirectToRoute(new { area = "admin", controller = "Home", action = "Index" });
                }
                else
                {
                    // If local auth fails, try API
                    try
                    {
                        LoginApiModel apiResult = await _apiClient.PostAsync<LoginApiModel>("api/auth/login", user);

                        if (apiResult != null && !string.IsNullOrEmpty(apiResult.AccessToken))
                        {
                            // Store the access token in the cookie
                            Response.Cookies.Append("AccessToken", apiResult.AccessToken, new CookieOptions
                            {
                                HttpOnly = false,
                                Secure = true,
                                Expires = DateTime.UtcNow.AddMinutes(15)
                            });

                            return RedirectToRoute(new { area = "admin", controller = "Home", action = "Index" });
                        }
                    }
                    catch (HttpRequestException ex) when (ex.Message.Contains("Unable to connect"))
                    {
                        // API is not available, fall back to local auth error
                        ModelState.AddModelError(string.Empty, "Invalid email or password.");
                        return View(user);
                    }
                    
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
                return View(user);
            }
        }

        [HttpGet]
        public async Task<IActionResult> TestApiConnection()
        {
            try
            {
                var testUser = new DTOUser { Email = "test@test.com", Password = "test" };
                await _apiClient.PostAsync<LoginApiModel>("api/auth/login", testUser);
                return Json(new { success = false, message = "API is reachable but login failed (expected for test credentials)" });
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("Unable to connect"))
            {
                return Json(new { success = false, message = $"Cannot connect to API server: {ex.Message}" });
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("401") || ex.Message.Contains("Unauthorized"))
            {
                return Json(new { success = true, message = "API is reachable and responding correctly" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Unexpected error: {ex.Message}" });
            }
        }
    }
}
