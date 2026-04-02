import api from './axios'
import type { Ticket, PagedResult, TicketStatus, TicketPriority } from '../types'

export const ticketsApi = {
    getAll: (page = 1, pageSize = 10) =>
        api.get<PagedResult<Ticket>>(`/tickets?page=${page}&pageSize=${pageSize}`),

    getById: (id: string) =>
        api.get<Ticket>(`/tickets/${id}`),

    getByStatus: (status: TicketStatus) =>
        api.get<Ticket[]>(`/tickets/status/${status}`),

    getMyTickets: () =>
        api.get<Ticket[]>('/tickets/my'),

    create: (data: {
        tenantId: string
        customerName: string
        customerEmail: string
        subject: string
        description: string
        priority: TicketPriority
    }) => api.post<Ticket>('/tickets', data),

    update: (id: string, data: {
        status?: TicketStatus
        priority?: TicketPriority
        assignedToUserId?: string
    }) => api.put<Ticket>(`/tickets/${id}`, data),

    reply: (id: string, data: { message: string; isFromCustomer: boolean }) =>
        api.post<Ticket>(`/tickets/${id}/reply`, data),

    assign: (ticketId: string, agentId: string) =>
        api.patch<Ticket>(`/tickets/${ticketId}/assign/${agentId}`),
}
