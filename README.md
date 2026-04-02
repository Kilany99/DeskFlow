# DeskFlow

A multi-tenant helpdesk platform built with .NET 10 and React 18. Companies sign up, their agents manage support tickets, and customers submit issues through a public form — all isolated per tenant.

This project was built as a full-stack learning exercise covering onion architecture, JWT authentication, multi-tenancy, and a React frontend wired to a proper API.

---

## What it does

- Companies register and get their own isolated helpdesk workspace
- Customers submit tickets without needing an account
- Agents pick up tickets, reply, and move them through a status workflow
- Tenant admins manage their team and monitor activity
- A super admin has visibility across all tenants on the platform

---

## Tech stack

| Layer | Technology |
|---|---|
| Backend | .NET 10 Web API |
| Frontend | React 18 + TypeScript (Vite) |
| Database | PostgreSQL |
| ORM | Entity Framework Core 10 |
| Auth | JWT + Refresh Tokens |
| Architecture | Onion Architecture |

---

## Project structure

```
DeskFlow.sln
├── deskflow.client           React + Vite frontend
├── DeskFlow.Domain           Entities, enums, repository interfaces
├── DeskFlow.Core             DTOs, service interfaces, service implementations
├── DeskFlow.Infrastructure   EF Core, repositories, unit of work, seeders
└── DeskFlow.Server           Controllers, middleware, program entry point
```

The dependency flow goes inward: `Server → Core → Domain`, with `Infrastructure` implementing the domain interfaces and never being referenced by `Core` directly.

---

## Getting started

### Prerequisites

- .NET 10 SDK
- Node.js 18+
- PostgreSQL running locally (or via Docker)

### 1. Clone the repo

```bash
git clone https://github.com/yourname/deskflow.git
cd deskflow
```

### 2. Set up the database connection

Edit `DeskFlow.Server/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=deskflow;Username=postgres;Password=yourpassword"
  },
  "Jwt": {
    "Key": "your-secret-key-minimum-32-characters-long",
    "Issuer": "DeskFlow",
    "Audience": "DeskFlowUsers"
  }
}
```

### 3. Run migrations and seed data

```bash
dotnet ef migrations add InitialCreate \
  --project DeskFlow.Infrastructure \
  --startup-project DeskFlow.Server

dotnet ef database update \
  --project DeskFlow.Infrastructure \
  --startup-project DeskFlow.Server
```

Seed data runs automatically on first startup via `DatabaseSeeder`.

### 4. Run the project

```bash
# From the solution root
dotnet run --project DeskFlow.Server
```

The React dev server starts alongside the API via the SPA proxy.

To test the API directly without the frontend:

```bash
cd DeskFlow.Server
dotnet run
# Then open https://localhost:{port}/swagger
```

---

## Seeded accounts

These are available immediately after running the project for the first time.

| Role | Email | Password | Subdomain |
|---|---|---|---|
| Super Admin | superadmin@deskflow.com | Admin@123 | acme |
| Tenant Admin (Acme) | alice@acme.com | Acme@123 | acme |
| Agent (Acme) | bob@acme.com | Acme@123 | acme |
| Agent (Acme) | carol@acme.com | Acme@123 | acme |
| Tenant Admin (Beta) | david@beta.com | Beta@123 | beta |
| Agent (Beta) | eva@beta.com | Beta@123 | beta |

Two tenants are pre-seeded (Acme Corp and Beta LLC) with tickets and reply threads so you can explore the full workflow without setting anything up manually.

---

## API overview

Authentication endpoints are open. Everything else requires a Bearer token in the `Authorization` header.

```
POST   /api/auth/register          Register a new company + admin
POST   /api/auth/login             Login and receive JWT + refresh token
POST   /api/auth/refresh           Exchange refresh token for new access token
POST   /api/auth/logout            Invalidate refresh token

GET    /api/tickets                All tickets for the current tenant (paged)
POST   /api/tickets                Submit a new ticket (public, no auth)
GET    /api/tickets/{id}           Get ticket by ID
GET    /api/tickets/track/{ref}    Track ticket by reference number (public)
PUT    /api/tickets/{id}           Update status, priority, or assignment
POST   /api/tickets/{id}/reply     Add a reply to a ticket
PATCH  /api/tickets/{id}/assign/{agentId}   Assign to an agent

GET    /api/users                  All users in the tenant
POST   /api/users/invite           Invite a new agent
PUT    /api/users/{id}             Update user details
PATCH  /api/users/{id}/role        Change user role
PATCH  /api/users/{id}/deactivate  Deactivate a user

GET    /api/tenants                All tenants (super admin only)
PATCH  /api/tenants/{id}/plan      Change a tenant's plan
PATCH  /api/tenants/{id}/suspend   Suspend a tenant

GET    /api/auditlogs              Logs for the current tenant
GET    /api/auditlogs/all          All logs across all tenants (super admin only)
```

---

## Roles and access

| Role | What they can do |
|---|---|
| SuperAdmin | View and manage all tenants, suspend accounts, change plans, view all logs |
| TenantAdmin | Manage agents, assign tickets, view all tenant tickets and logs |
| Agent | View and reply to assigned tickets |

Tenant isolation is enforced at the middleware level. Every request from an authenticated user is scoped to their `tenantId` extracted from the JWT — no data from another tenant is ever reachable regardless of what ID is passed in the request.

---

## Testing the API with Swagger

1. Run the server and navigate to `/swagger`
2. Call `POST /api/auth/login` with one of the seeded accounts
3. Copy the `accessToken` from the response
4. Click the **Authorize** button at the top of Swagger and enter `Bearer {your token}`
5. All subsequent requests will be authenticated as that user

A good first test for tenant isolation: log in as an Acme user, fetch tickets, note the results. Then log in as a Beta user and confirm the data is completely separate.

---

## Architecture notes

**Why onion architecture?**
The domain and business logic have zero dependency on infrastructure concerns like EF Core or HTTP. Swapping the database or exposing a different API surface doesn't touch the core logic.

**Why interface segregation on services?**
Service interfaces are split by capability (`ICreatableService`, `IReadableService`, `IActivatableService`, etc.) so consumers only depend on what they actually use. A controller that only reads data doesn't need to know about create or delete operations.

**Why unit of work?**
All repository access goes through a single `IUnitOfWork` so transaction boundaries are explicit and `SaveChanges` is called once per operation, not buried inside individual repositories.

---

## What's next

- React frontend (in progress)
- Docker Compose setup
- Email notifications via SMTP
- Real-time ticket updates via SignalR
