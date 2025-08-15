using BL.Contracts;
using BL.DTOConfiguration;
using BL.Services;
using DAL.UserModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IRefreshTokens _RefreshTokenService;
        public AuthController(TokenService tokenService,
                              IUserService userService,
                              IRefreshTokens refreshTokenService)
        {
            _tokenService = tokenService;
            _userService = userService;
            _RefreshTokenService = refreshTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTOUser request)
        {
            var result = await _userService.RegisterAsync(request);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DTOLogin request)
        {
            var userResult = await _userService.LoginAsync(request);
            if (!userResult.Success)
            {
                return Unauthorized("Invalid credentials");
            }


            var userData = await GetClims(request.Email);
            var claims = userData.Item1;
            DTOUser user = userData.Item2;
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var storedToken = new DTORefreshToken
            {
                Token = refreshToken,
                UserId = user.Id.ToString(),
                Expires = DateTime.UtcNow.AddDays(7),
                CurrentState = 1
            };

            _RefreshTokenService.RefreshTokenExists(storedToken);

            Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = storedToken.Expires
            });

            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
        }

        [HttpPost("refresh-access-token")]
        public async Task<IActionResult> RefreshAccessToken()
        {
            if (!Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
            {
                return Unauthorized("No refresh token found");
            }

            // Retrieve the refresh token from the database
            var storedToken = _RefreshTokenService.GetByToken(refreshToken);
            if (storedToken == null || storedToken.CurrentState == 2 || storedToken.Expires < DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            // Generate a new access token
            var claims = await GetClimsById(storedToken.UserId);

            var newAccessToken = _tokenService.GenerateAccessToken(claims);

            return Ok(new { AccessToken = newAccessToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
            {
                return Unauthorized("No refresh token found");
            }

            // Retrieve the refresh token from the database
            var storedToken = _RefreshTokenService.GetByToken(refreshToken);
            if (storedToken == null || storedToken.CurrentState == 2 || storedToken.Expires < DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            // Generate a new refresh token
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var newRefreshDto = new DTORefreshToken
            {
                Token = newRefreshToken,
                UserId = storedToken.Id.ToString(),
                Expires = DateTime.UtcNow.AddDays(7),
                CurrentState = 1
            };
            _RefreshTokenService.RefreshTokenExists(newRefreshDto);

            // Set the new refresh token in the cookies
            Response.Cookies.Append("RefreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new { RefreshToken = newRefreshToken });
        }

        async Task<(Claim[], DTOUser)> GetClims(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            var claims = new[] {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, "User")
            };

            return (claims, user);
        }

        async Task<Claim[]> GetClimsById(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            var claims = new[] {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, "User")
            };

            return claims;
        }
    }
}
