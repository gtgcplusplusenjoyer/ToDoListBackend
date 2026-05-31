using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoList.Application.Dto.User;
using ToDoList.Application.Interfaces;

namespace ToDoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService service)
        {
            _authService = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto registerUserDto, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterAsync(registerUserDto, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(new {message = result.ErrorMessage});
            }

            return Ok(new
            {
                accessToken = result.TokenPair!.AccessToken,
                refreshToken = result.TokenPair!.RefreshToken,
                userId = result.UserId
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto loginUserDto, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(loginUserDto, cancellationToken);

            if (!result.IsSuccess)
            {
                return Unauthorized(new { message = result.ErrorMessage });
            }

            return Ok(new
            {
                accessToken = result.TokenPair!.AccessToken,
                refreshToken = result.TokenPair!.RefreshToken,
                userId = result.UserId
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
        {
            var result = await _authService.RefreshToken(refreshTokenRequest.RefreshToken, cancellationToken);

            if (!result.IsSuccess)
            {
                return Unauthorized(new { message = result.ErrorMessage });
            }

            return Ok(new
            {
                accessToken = result.TokenPair!.AccessToken,
                refreshToken = result.TokenPair!.RefreshToken
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest(new { message = "Invalid user identifier in token" });
            }

            var result = await _authService.LogoutAsync(userId, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(new {message = "Logged out successfully"});
        }

    }
}
