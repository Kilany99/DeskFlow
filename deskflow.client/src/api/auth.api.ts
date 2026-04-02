import api from './axios'
import type { AuthUser } from '../types/index'

export const authApi = {    
    login: (data: { email: string; password: string; subdomain: string }) =>
        api.post<AuthUser>('/auth/login', data),

    register: (data: {
        companyName: string
        subdomain: string
        adminEmail: string
        adminFullName: string
        password: string
    }) => api.post<AuthUser>('/auth/register', data),

    logout: () => api.post('/auth/logout'),

    refresh: (refreshToken: string) =>
        api.post<AuthUser>('/auth/refresh', { refreshToken }),
}

