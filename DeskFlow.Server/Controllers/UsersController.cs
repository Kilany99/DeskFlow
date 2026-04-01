using DeskFlow.Application.Common;
using DeskFlow.Application.DTOs.Users;
using DeskFlow.Application.Services.Interfaces;
using DeskFlow.Server.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.Server.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Authorize]
    public class UsersController : TenantScopedController
    {
        private readonly IUserService _userService;
        private readonly ITenantService _tenantService;
        private readonly IAuditLogService _auditLogService;

        public UsersController(
            IUserService userService,
            ITenantService tenantService,
            IAuditLogService auditLogService)
        {
            _userService = userService;
            _tenantService = tenantService;
            _auditLogService = auditLogService;
        }

        /// <summary>Get all users in the current tenant</summary>
        [HttpGet]
        [Authorize(Roles = "TenantAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllByTenantAsync(ResolveTenantId());
            return Ok(users);
        }

        /// <summary>Get all agents in the current tenant</summary>
        [HttpGet("agents")]
        [Authorize(Roles = "TenantAdmin")]
        public async Task<IActionResult> GetAgents()
            => HandleResult(await _userService.GetAgentsAsync(ResolveTenantId()));

        /// <summary>Get a user by id scoped to tenant</summary>
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "TenantAdmin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAndTenantAsync(id, ResolveTenantId());
            return user is null ? NotFound() : Ok(user);
        }

        /// <summary>Invite a new user to the tenant</summary>
        [HttpPost("invite")]
        [Authorize(Roles = "TenantAdmin")]
        public async Task<IActionResult> Invite([FromBody] InviteUserDto dto)
        {
            var canInvite = await _tenantService.CanInviteMoreUsersAsync(ResolveTenantId());
            if (!canInvite.IsSuccess || canInvite.Data == false)
                return HandleResult(canInvite);

            var result = await _userService.InviteUserAsync(dto, ResolveTenantId());

            if (result.IsSuccess)
                await _auditLogService.LogAsync(
                    ResolveTenantId(),
                    CurrentUserId,
                    AuditActions.UserInvited,
                    $"User {dto.Email} invited as {dto.Role}");

            return HandleResult(result);
        }

        /// <summary>Update a user's details</summary>
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "TenantAdmin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto)
            => HandleResult(await _userService.UpdateAsync(id, dto));

        /// <summary>Change a user's role</summary>
        [HttpPatch("{id:guid}/role")]
        [Authorize(Roles = "TenantAdmin")]
        public async Task<IActionResult> ChangeRole(Guid id, [FromBody] ChangeRoleDto dto)
        {
            var result = await _userService.ChangeRoleAsync(id, dto.Role, ResolveTenantId());

            if (result.IsSuccess)
                await _auditLogService.LogAsync(
                    ResolveTenantId(),
                    CurrentUserId,
                    AuditActions.UserRoleChanged,
                    $"User {id} role changed to {dto.Role}");

            return HandleResult(result);
        }

        /// <summary>Deactivate a user</summary>
        [HttpPatch("{id:guid}/deactivate")]
        [Authorize(Roles = "TenantAdmin")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            await _userService.DeactivateAsync(id);

            await _auditLogService.LogAsync(
                ResolveTenantId(),
                CurrentUserId,
                AuditActions.UserDeactivated,
                $"User {id} deactivated");

            return NoContent();
        }

        /// <summary>Reactivate a user</summary>
        [HttpPatch("{id:guid}/activate")]
        [Authorize(Roles = "TenantAdmin")]
        public async Task<IActionResult> Activate(Guid id)
        {
            await _userService.ActivateAsync(id);

            await _auditLogService.LogAsync(
                ResolveTenantId(),
                CurrentUserId,
                AuditActions.UserActivated,
                $"User {id} activated");

            return NoContent();
        }
    }
}
