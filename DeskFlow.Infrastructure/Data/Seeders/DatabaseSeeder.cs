using DeskFlow.Domain.Entities;
using DeskFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Infrastructure.Data.Seeders
{
    // DeskFlow.Infrastructure/Data/Seeders/DatabaseSeeder.cs
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            await SeedTenantsAsync(context);
            await SeedUsersAsync(context);
            await SeedTicketsAsync(context);
            await SeedRepliesAsync(context);
        }

        // ─────────────────────────────────────────
        // Tenants
        // ─────────────────────────────────────────
        private static async Task SeedTenantsAsync(AppDbContext context)
        {
            if (context.Tenants.Any()) return;

            var tenants = new List<Tenant>
        {
            new()
            {
                Id        = SeedIds.AcmeTenantId,
                Name      = "Acme Corp",
                Subdomain = "acme",
                Plan      = PlanType.Pro,
                IsActive  = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id        = SeedIds.BetaTenantId,
                Name      = "Beta LLC",
                Subdomain = "beta",
                Plan      = PlanType.Free,
                IsActive  = true,
                CreatedAt = DateTime.UtcNow
            }
        };

            await context.Tenants.AddRangeAsync(tenants);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────
        // Users
        // ─────────────────────────────────────────
        private static async Task SeedUsersAsync(AppDbContext context)
        {
            if (context.Users.Any()) return;

            var users = new List<User>
        {
            // ── Super Admin (no tenant) ──────────
            new()
            {
                Id           = SeedIds.SuperAdminId,
                TenantId     = SeedIds.AcmeTenantId, // required FK — assign to Acme
                FullName     = "Super Admin",
                Email        = "superadmin@deskflow.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role         = UserRole.SuperAdmin,
                IsActive     = true,
                CreatedAt    = DateTime.UtcNow
            },

            // ── Acme Corp ────────────────────────
            new()
            {
                Id           = SeedIds.AcmeAdminId,
                TenantId     = SeedIds.AcmeTenantId,
                FullName     = "Alice Johnson",
                Email        = "alice@acme.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Acme@123"),
                Role         = UserRole.TenantAdmin,
                IsActive     = true,
                CreatedAt    = DateTime.UtcNow
            },
            new()
            {
                Id           = SeedIds.AcmeAgent1Id,
                TenantId     = SeedIds.AcmeTenantId,
                FullName     = "Bob Smith",
                Email        = "bob@acme.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Acme@123"),
                Role         = UserRole.Agent,
                IsActive     = true,
                CreatedAt    = DateTime.UtcNow
            },
            new()
            {
                Id           = SeedIds.AcmeAgent2Id,
                TenantId     = SeedIds.AcmeTenantId,
                FullName     = "Carol White",
                Email        = "carol@acme.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Acme@123"),
                Role         = UserRole.Agent,
                IsActive     = true,
                CreatedAt    = DateTime.UtcNow
            },

            // ── Beta LLC ─────────────────────────
            new()
            {
                Id           = SeedIds.BetaAdminId,
                TenantId     = SeedIds.BetaTenantId,
                FullName     = "David Lee",
                Email        = "david@beta.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Beta@123"),
                Role         = UserRole.TenantAdmin,
                IsActive     = true,
                CreatedAt    = DateTime.UtcNow
            },
            new()
            {
                Id           = SeedIds.BetaAgent1Id,
                TenantId     = SeedIds.BetaTenantId,
                FullName     = "Eva Green",
                Email        = "eva@beta.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Beta@123"),
                Role         = UserRole.Agent,
                IsActive     = true,
                CreatedAt    = DateTime.UtcNow
            }
        };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────
        // Tickets
        // ─────────────────────────────────────────
        private static async Task SeedTicketsAsync(AppDbContext context)
        {
            if (context.Tickets.Any()) return;

            var tickets = new List<Ticket>
        {
            // ── Acme Tickets ─────────────────────
            new()
            {
                Id              = SeedIds.AcmeTicket1Id,
                TenantId        = SeedIds.AcmeTenantId,
                AssignedToUserId= SeedIds.AcmeAgent1Id,
                ReferenceNumber = "TKT-00001",
                CustomerName    = "John Customer",
                CustomerEmail   = "john@customer.com",
                Subject         = "Cannot login to my account",
                Description     = "I have been trying to login since this morning but keep getting an error.",
                Status          = TicketStatus.InProgress,
                Priority        = TicketPriority.High,
                CreatedAt       = DateTime.UtcNow.AddDays(-3)
            },
            new()
            {
                Id              = SeedIds.AcmeTicket2Id,
                TenantId        = SeedIds.AcmeTenantId,
                AssignedToUserId= SeedIds.AcmeAgent2Id,
                ReferenceNumber = "TKT-00002",
                CustomerName    = "Sara Client",
                CustomerEmail   = "sara@client.com",
                Subject         = "Billing invoice is incorrect",
                Description     = "My last invoice shows double the amount I should be charged.",
                Status          = TicketStatus.Open,
                Priority        = TicketPriority.Critical,
                CreatedAt       = DateTime.UtcNow.AddDays(-1)
            },
            new()
            {
                Id              = SeedIds.AcmeTicket3Id,
                TenantId        = SeedIds.AcmeTenantId,
                ReferenceNumber = "TKT-00003",
                CustomerName    = "Mike User",
                CustomerEmail   = "mike@user.com",
                Subject         = "Feature request: dark mode",
                Description     = "Would love to have a dark mode option in the dashboard.",
                Status          = TicketStatus.Open,
                Priority        = TicketPriority.Low,
                CreatedAt       = DateTime.UtcNow.AddHours(-5)
            },

            // ── Beta Tickets ──────────────────────
            new()
            {
                Id              = SeedIds.BetaTicket1Id,
                TenantId        = SeedIds.BetaTenantId,
                AssignedToUserId= SeedIds.BetaAgent1Id,
                ReferenceNumber = "TKT-00004",
                CustomerName    = "Tom Beta",
                CustomerEmail   = "tom@beta-customer.com",
                Subject         = "App crashes on startup",
                Description     = "The mobile app crashes immediately after the splash screen.",
                Status          = TicketStatus.Open,
                Priority        = TicketPriority.Critical,
                CreatedAt       = DateTime.UtcNow.AddDays(-2)
            },
            new()
            {
                Id              = SeedIds.BetaTicket2Id,
                TenantId        = SeedIds.BetaTenantId,
                ReferenceNumber = "TKT-00005",
                CustomerName    = "Lisa Beta",
                CustomerEmail   = "lisa@beta-customer.com",
                Subject         = "Password reset email not arriving",
                Description     = "I requested a password reset 3 times but no email arrives.",
                Status          = TicketStatus.Resolved,
                Priority        = TicketPriority.Medium,
                CreatedAt       = DateTime.UtcNow.AddDays(-5)
            }
        };

            await context.Tickets.AddRangeAsync(tickets);
            await context.SaveChangesAsync();
        }

        // ─────────────────────────────────────────
        // Replies
        // ─────────────────────────────────────────
        private static async Task SeedRepliesAsync(AppDbContext context)
        {
            if (context.TicketReplies.Any()) return;

            var replies = new List<TicketReply>
        {
            // Acme Ticket 1 thread
            new()
            {
                TicketId       = SeedIds.AcmeTicket1Id,
                AgentId        = null,
                IsFromCustomer = true,
                Message        = "I keep getting 'Invalid credentials' even though my password is correct.",
                CreatedAt      = DateTime.UtcNow.AddDays(-3).AddHours(1)
            },
            new()
            {
                TicketId       = SeedIds.AcmeTicket1Id,
                AgentId        = SeedIds.AcmeAgent1Id,
                IsFromCustomer = false,
                Message        = "Hi John, I can see your account. Let me reset your session and try again.",
                CreatedAt      = DateTime.UtcNow.AddDays(-3).AddHours(2)
            },
            new()
            {
                TicketId       = SeedIds.AcmeTicket1Id,
                AgentId        = null,
                IsFromCustomer = true,
                Message        = "Still not working after the reset. Same error message.",
                CreatedAt      = DateTime.UtcNow.AddDays(-2)
            },

            // Acme Ticket 2 thread
            new()
            {
                TicketId       = SeedIds.AcmeTicket2Id,
                AgentId        = SeedIds.AcmeAgent2Id,
                IsFromCustomer = false,
                Message        = "Hi Sara, I've escalated this to our billing team. Expect a response within 24h.",
                CreatedAt      = DateTime.UtcNow.AddHours(-12)
            },

            // Beta Ticket 2 thread (resolved)
            new()
            {
                TicketId       = SeedIds.BetaTicket2Id,
                AgentId        = SeedIds.BetaAgent1Id,
                IsFromCustomer = false,
                Message        = "Hi Lisa, we found the issue — your emails were going to spam. Please check your spam folder.",
                CreatedAt      = DateTime.UtcNow.AddDays(-4)
            },
            new()
            {
                TicketId       = SeedIds.BetaTicket2Id,
                AgentId        = null,
                IsFromCustomer = true,
                Message        = "Found it in spam! Thank you so much, issue resolved.",
                CreatedAt      = DateTime.UtcNow.AddDays(-4).AddHours(2)
            }
        };

            await context.TicketReplies.AddRangeAsync(replies);
            await context.SaveChangesAsync();
        }
    }
}
