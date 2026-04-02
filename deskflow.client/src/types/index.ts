
export type UserRole = 'SuperAdmin' | 'TenantAdmin' | 'Agent'
export type TicketStatus = 'Open' | 'InProgress' | 'Resolved' | 'Closed'
export type TicketPriority = 'Low' | 'Medium' | 'High' | 'Critical'
export type PlanType = 'Free' | 'Pro' | 'Enterprise'

export interface AuthUser {
    userId: string
    tenantId: string
    role: UserRole
    fullName: string
    email: string
    accessToken: string
    refreshToken: string
}

export interface TicketReply {
    id: string
    message: string
    isFromCustomer: boolean
    agentName?: string
    createdAt: string
}

export interface Ticket {
    id: string
    referenceNumber: string
    customerName: string
    customerEmail: string
    subject: string
    description: string
    status: TicketStatus
    priority: TicketPriority
    assignedToName?: string
    createdAt: string
    replies: TicketReply[]
}

export interface PagedResult<T> {
    items: T[]
    totalCount: number
    page: number
    pageSize: number
    totalPages: number
    hasNext: boolean
    hasPrevious: boolean
}

export interface User {
    id: string
    fullName: string
    email: string
    role: UserRole
    isActive: boolean
    createdAt: string
}

export interface Tenant {
    id: string
    name: string
    subdomain: string
    plan: PlanType
    isActive: boolean
    memberCount: number
    createdAt: string
}

export interface AuditLog {
    id: string
    tenantId: string
    userId?: string
    userFullName?: string
    action: string
    details?: string
    ipAddress?: string
    createdAt: string
}