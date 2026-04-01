using DeskFlow.Application.Common;
using DeskFlow.Application.DTOs.Users;
using DeskFlow.Application.Services.Interfaces;
using DeskFlow.Domain.Entities;
using DeskFlow.Domain.Enums;
using DeskFlow.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DeskFlow.Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;

        public UserService(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<UserResponseDto>> InviteUserAsync(InviteUserDto dto, Guid tenantId)
        {
            if (await _uow.Users.EmailExistsInTenantAsync(dto.Email, tenantId))
                return Result<UserResponseDto>.Failure("Email already exists in this tenant");

            var tempPassword = Convert.ToBase64String(RandomNumberGenerator.GetBytes(12));

            var user = new User
            {
                TenantId = tenantId,
                FullName = dto.FullName,
                Email = dto.Email.ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(tempPassword),
                Role = dto.Role
            };

            await _uow.Users.AddAsync(user);
            await _uow.SaveChangesAsync();

            // TODO: send invite email with tempPassword

            return Result<UserResponseDto>.Success(MapToDto(user), 201);
        }

        public async Task<Result<UserResponseDto>> UpdateAsync(Guid id, UpdateUserDto dto)
        {
            var user = await _uow.Users.GetByIdAsync(id);
            if (user is null) return Result<UserResponseDto>.NotFound();

            if (!string.IsNullOrEmpty(dto.FullName)) user.FullName = dto.FullName;
            if (dto.Role.HasValue) user.Role = dto.Role.Value;

            await _uow.Users.UpdateAsync(user);
            await _uow.SaveChangesAsync();

            return Result<UserResponseDto>.Success(MapToDto(user));
        }

        public async Task<Result<bool>> ChangeRoleAsync(Guid userId, UserRole role, Guid tenantId)
        {
            var user = await _uow.Users.GetByIdAndTenantAsync(userId, tenantId);
            if (user is null) return Result<bool>.NotFound();

            user.Role = role;
            await _uow.Users.UpdateAsync(user);
            await _uow.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task ActivateAsync(Guid id)
        {
            var user = await _uow.Users.GetByIdAsync(id);
            if (user is null) return;
            user.IsActive = true;
            await _uow.Users.UpdateAsync(user);
            await _uow.SaveChangesAsync();
        }

        public async Task DeactivateAsync(Guid id)
        {
            var user = await _uow.Users.GetByIdAsync(id);
            if (user is null) return;
            user.IsActive = false;
            await _uow.Users.UpdateAsync(user);
            await _uow.SaveChangesAsync();
        }

        public async Task<Result<IEnumerable<UserResponseDto>>> GetAgentsAsync(Guid tenantId)
        {
            var agents = await _uow.Users.GetAgentsByTenantAsync(tenantId);
            return Result<IEnumerable<UserResponseDto>>.Success(agents.Select(MapToDto));
        }

        public async Task<UserResponseDto?> GetByIdAsync(Guid id)
        {
            var user = await _uow.Users.GetByIdAsync(id);
            return user is null ? null : MapToDto(user);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var users = await _uow.Users.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllByTenantAsync(Guid tenantId)
        {
            var users = await _uow.Users.GetAllByTenantAsync(tenantId);
            return users.Select(MapToDto);
        }

        public async Task<UserResponseDto?> GetByIdAndTenantAsync(Guid id, Guid tenantId)
        {
            var user = await _uow.Users.GetByIdAndTenantAsync(id, tenantId);
            return user is null ? null : MapToDto(user);
        }

        private static UserResponseDto MapToDto(User u) => new()
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            Role = u.Role.ToString(),
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt
        };
    }
}
