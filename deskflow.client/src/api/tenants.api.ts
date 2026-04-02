import api from './axios'
import type { Tenant, PlanType } from '../types'

export const tenantsApi = {
    getAll: () => api.get<Tenant[]>('/tenants'),
    getById: (id: string) => api.get<Tenant>(`/tenants/${id}`),
    changePlan: (id: string, plan: PlanType) =>
        api.patch(`/tenants/${id}/plan`, { plan }),
    suspend: (id: string) => api.patch(`/tenants/${id}/suspend`),
    unsuspend: (id: string) => api.patch(`/tenants/${id}/unsuspend`),
}