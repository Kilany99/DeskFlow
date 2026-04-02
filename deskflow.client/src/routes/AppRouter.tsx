import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { ProtectedRoute } from './ProtectedRoute'
import { RoleRoute } from './RoleRoute'
import { LoginPage } from '../pages/auth/LoginPage'
import { RegisterPage } from '../pages/auth/RegisterPage'
import { DashboardPage } from '../pages/agent/DashboardPage'
import { TicketDetailPage } from '../pages/agent/TicketDetailPage'
import { AdminDashboardPage } from '../pages/admin/AdminDashboardPage'
import { UsersPage } from '../pages/admin/UsersPage'
import { TenantsPage } from '../pages/admin/TenantsPage'
import { AuditLogPage } from '../pages/admin/AuditLogPage'
import { LandingPage } from '../pages/LandingPage'

export function AppRouter() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<LandingPage />} />

                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />

                <Route path="/app" element={
                    <ProtectedRoute><Navigate to="/dashboard" /></ProtectedRoute>
                } />
                <Route path="/dashboard" element={
                    <ProtectedRoute><DashboardPage /></ProtectedRoute>
                } />

                <Route path="/tickets" element={
                    <ProtectedRoute><DashboardPage /></ProtectedRoute>
                } />

                <Route path="/tickets/:id" element={
                    <ProtectedRoute><TicketDetailPage /></ProtectedRoute>
                } />

                <Route path="/users" element={
                    <ProtectedRoute>
                        <RoleRoute roles={['TenantAdmin', 'SuperAdmin']}>
                            <UsersPage />
                        </RoleRoute>
                    </ProtectedRoute>
                } />

                <Route path="/tenants" element={
                    <ProtectedRoute>
                        <RoleRoute roles={['SuperAdmin']}>
                            <TenantsPage />
                        </RoleRoute>
                    </ProtectedRoute>
                } />

                <Route path="/admin" element={
                    <ProtectedRoute>
                        <RoleRoute roles={['TenantAdmin', 'SuperAdmin']}>
                            <AdminDashboardPage />
                        </RoleRoute>
                    </ProtectedRoute>
                } />
                <Route path="/audit" element={
                    <ProtectedRoute>
                        <RoleRoute roles={['TenantAdmin', 'SuperAdmin']}>
                            <AuditLogPage />
                        </RoleRoute>
                    </ProtectedRoute>
                } />

            </Routes>
        </BrowserRouter>
    )
}
