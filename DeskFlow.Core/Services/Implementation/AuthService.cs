using BCrypt.Net;
using DeskFlow.Application.Common;
using DeskFlow.Application.DTOs.Auth;
using DeskFlow.Application.DTOs.Tenants;
using DeskFlow.Application.Services.Interfaces;
using DeskFlow.Domain.Entities;
using DeskFlow.Domain.Enums;
using DeskFlow.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DeskFlow.Application.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;

        public AuthService(IUnitOfWork uow, IConfiguration config)
        {
            _uow = uow;
            _config = config;
        }

        public async Task<Result<AuthResponseDto>> CreateAsync(RegisterTenantDto dto)
        {
            if (await _uow.Tenants.SubdomainExistsAsync(dto.Subdomain))
                return Result<AuthResponseDto>.Failure("Subdomain already taken");

            await _uow.BeginTransactionAsync();
            try
            {
                var tenant = new Tenant
                {
                    Name = dto.CompanyName,
                    Subdomain = dto.Subdomain.ToLower(),
                    Plan = PlanType.Free
                };
                await _uow.Tenants.AddAsync(tenant);

                var admin = new User
                {
                    TenantId = tenant.Id,
                    FullName = dto.AdminFullName,
                    Email = dto.AdminEmail.ToLower(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Role = UserRole.TenantAdmin
                };
                await _uow.Users.AddAsync(admin);
                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();

                var token = GenerateAccessToken(admin, tenant.Id);
                var refresh = await SetRefreshTokenAsync(admin);

                return Result<AuthResponseDto>.Success(new AuthResponseDto
                {
                    AccessToken = token,
                    RefreshToken = refresh,
                    FullName = admin.FullName,
                    Role = admin.Role.ToString(),
                    TenantId = tenant.Id
                }, 201);
            }
            catch
            {
                await _uow.RollbackTransactionAsync();
                return Result<AuthResponseDto>.Failure("Registration failed");
            }
        }

        public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto)
        {
            var tenant = await _uow.Tenants.GetBySubdomainAsync(dto.Subdomain);
            if (tenant is null || !tenant.IsActive)
                return Result<AuthResponseDto>.Unauthorized("Invalid credentials or tenant suspended");

            var user = await _uow.Users.GetByEmailAsync(dto.Email.ToLower(), tenant.Id);
            if (user is null || !user.IsActive)
                return Result<AuthResponseDto>.Unauthorized("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Result<AuthResponseDto>.Unauthorized("Invalid credentials");

            var token = GenerateAccessToken(user, tenant.Id);
            var refresh = await SetRefreshTokenAsync(user);

            return Result<AuthResponseDto>.Success(new AuthResponseDto
            {
                AccessToken = token,
                RefreshToken = refresh,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                TenantId = tenant.Id
            });
        }

        public async Task<Result<AuthResponseDto>> RefreshTokenAsync(string refreshToken)
        {
            var user = await _uow.Users.GetByRefreshTokenAsync(refreshToken);
            if (user is null)
                return Result<AuthResponseDto>.Unauthorized("Invalid or expired refresh token");

            var token = GenerateAccessToken(user, user.TenantId);
            var newRefresh = await SetRefreshTokenAsync(user);

            return Result<AuthResponseDto>.Success(new AuthResponseDto
            {
                AccessToken = token,
                RefreshToken = newRefresh,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                TenantId = user.TenantId
            });
        }

        public async Task<Result<bool>> LogoutAsync(Guid userId)
        {
            var user = await _uow.Users.GetByIdAsync(userId);
            if (user is null) return Result<bool>.NotFound();

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _uow.Users.UpdateAsync(user);
            await _uow.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        private string GenerateAccessToken(User user, Guid tenantId)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("tenantId", tenantId.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> SetRefreshTokenAsync(User user)
        {
            var refresh = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.RefreshToken = refresh;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _uow.Users.UpdateAsync(user);
            await _uow.SaveChangesAsync();
            return refresh;
        }
    }

}
