using DeskFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.DTOs.Users
{
    public class InviteUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Agent;
    }
}
