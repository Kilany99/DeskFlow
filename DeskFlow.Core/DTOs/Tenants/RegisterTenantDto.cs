using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.DTOs.Tenants
{
    public class RegisterTenantDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Subdomain { get; set; } = string.Empty;
        public string AdminEmail { get; set; } = string.Empty;
        public string AdminFullName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
