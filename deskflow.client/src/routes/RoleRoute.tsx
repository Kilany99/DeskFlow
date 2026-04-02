import { Navigate } from 'react-router-dom'
import { useAuthStore } from '../store/authStore'
import type { UserRole } from '../types'

interface RoleRouteProps {
    children: React.ReactNode
    roles: UserRole[]
}

export function RoleRoute({ children, roles }: RoleRouteProps) {
    const { user } = useAuthStore()
    if (!user || !roles.includes(user.role)) return <Navigate to="/dashboard" replace />
    return <>{children}</>
}
