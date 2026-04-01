using DeskFlow.Application.DTOs.Auth;
using DeskFlow.Application.DTOs.Tenants;
using DeskFlow.Application.Services.Interfaces;
using DeskFlow.Server.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.Server.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
            => _authService = authService;

        /// <summary>Register a new company and its first admin</summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterTenantDto dto)
            => HandleResult(await _authService.CreateAsync(dto));

        /// <summary>Login with email, password and subdomain</summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
            => HandleResult(await _authService.LoginAsync(dto));

        /// <summary>Get new access token using refresh token</summary>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
            => HandleResult(await _authService.RefreshTokenAsync(dto.RefreshToken));

        /// <summary>Logout and invalidate refresh token</summary>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
            => HandleResult(await _authService.LogoutAsync(CurrentUserId));
    }
}
