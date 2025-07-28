using BL.Contracts;
using BL.DTOConfiguration;
using DAL.UserModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
namespace Ui.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor accessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = accessor;
        }

        public async Task<DTOUserResult> RegisterAsync(DTOUser registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return new DTOUserResult { Success = false, Errors = new[] { "Passwords do not match." } };
            }

            var user = new ApplicationUser { UserName = registerDto.Email, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            return new DTOUserResult
            {
                Success = result.Succeeded,
                Errors = result.Errors?.Select(e => e.Description)
            };
        }

        public async Task<DTOUserResult> LoginAsync(DTOUser loginDto)
        {
            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);

            if (!result.Succeeded)
            {
                return new DTOUserResult
                {
                    Success = false,
                    Errors = new[] { "Invalid login attempt." }
                };
            }

            // Generate token (if needed) or return success
            return new DTOUserResult { Success = true, Token = "DummyTokenForNow" };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<DTOUser> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return new DTOUser
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email,
            };
        }

        public async Task<IEnumerable<DTOUser>> GetAllUsersAsync()
        {
            var users = _userManager.Users;
            return users.Select(u => new DTOUser
            {
                Id = Guid.Parse(u.Id),
                Email = u.Email,
            });
        }

        public Guid GetLoggedInUser()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(userId);
        }
    }

}
