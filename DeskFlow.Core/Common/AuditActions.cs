using System;
using System.Collections.Generic;
using System.Text;

namespace DeskFlow.Application.Common
{
    //Avoid magic strings when calling LogAsync
    public static class AuditActions
    {
        // Auth
        public const string Login = "AUTH_LOGIN";
        public const string Logout = "AUTH_LOGOUT";
        public const string TokenRefreshed = "AUTH_TOKEN_REFRESHED";

        // Tickets
        public const string TicketCreated = "TICKET_CREATED";
        public const string TicketUpdated = "TICKET_UPDATED";
        public const string TicketAssigned = "TICKET_ASSIGNED";
        public const string TicketReplied = "TICKET_REPLIED";
        public const string TicketClosed = "TICKET_CLOSED";

        // Users
        public const string UserInvited = "USER_INVITED";
        public const string UserDeactivated = "USER_DEACTIVATED";
        public const string UserActivated = "USER_ACTIVATED";
        public const string UserRoleChanged = "USER_ROLE_CHANGED";

        // Tenant
        public const string TenantCreated = "TENANT_CREATED";
        public const string TenantSuspended = "TENANT_SUSPENDED";
        public const string PlanChanged = "PLAN_CHANGED";
    }
}
