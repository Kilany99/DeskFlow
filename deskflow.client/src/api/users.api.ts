import api from './axios'
import type { User, UserRole } from '../types'

export const usersApi = {
    getAll: () => api.get<User[]>('/users'),
    getAgents: () => api.get<User[]>('/users/agents'),
    invite: (data: { email: string; fullName: string; role: UserRole }) =>
        api.post<User>('/users/invite', data),
    update: (id: string, data: { fullName?: string; role?: UserRole }) =>
        api.put<User>(`/users/${id}`, data),
    changeRole: (id: string, role: UserRole) =>
        api.patch(`/users/${id}/role`, { role }),
    activate: (id: string) => api.patch(`/users/${id}/activate`),
    deactivate: (id: string) => api.patch(`/users/${id}/deactivate`),
}