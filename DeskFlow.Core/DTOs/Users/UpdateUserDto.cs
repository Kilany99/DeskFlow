using DeskFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.DTOs.Users
{
    public class UpdateUserDto
    {
        public string? FullName { get; set; }
        public UserRole? Role { get; set; }
    }
}
