import { NavLink, useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { LayoutDashboard, Ticket, Users, Building2, ScrollText, LogOut } from 'lucide-react'
import { useAuthStore } from '../../store/authStore'
import { authApi } from '../../api/auth.api'
import clsx from 'clsx'

const navItems = [
    { to: '/dashboard', icon: LayoutDashboard, key: 'nav.dashboard', roles: ['TenantAdmin', 'Agent', 'SuperAdmin'] },
    { to: '/tickets', icon: Ticket, key: 'nav.tickets', roles: ['TenantAdmin', 'Agent', 'SuperAdmin'] },
    { to: '/users', icon: Users, key: 'nav.users', roles: ['TenantAdmin', 'SuperAdmin'] },
    { to: '/tenants', icon: Building2, key: 'nav.tenants', roles: ['SuperAdmin'] },
    { to: '/audit', icon: ScrollText, key: 'nav.auditLogs', roles: ['TenantAdmin', 'SuperAdmin'] },
]

export function Sidebar() {
    const { t } = useTranslation()
    const { user, clearUser } = useAuthStore()
    const navigate = useNavigate()

    const handleLogout = async () => {
        await authApi.logout()
        clearUser()
        navigate('/login')
    }

    const visibleItems = navItems.filter(item =>
        user?.role && item.roles.includes(user.role)
    )

    return (
        <aside className="sidebar">
            <div className="sidebar__brand">
                <div className="sidebar__logo">D</div>
                <span className="sidebar__name">DeskFlow</span>
            </div>

            <nav className="sidebar__nav">
                {visibleItems.map(({ to, icon: Icon, key }) => (
                    <NavLink
                        key={to}
                        to={to}
                        className={({ isActive }) =>
                            clsx('sidebar__link', isActive && 'sidebar__link--active')
                        }
                    >
                        <Icon size={17} />
                        <span>{t(key)}</span>
                    </NavLink>
                ))}
            </nav>

            <div className="sidebar__footer">
                <div className="sidebar__user">
                    <div className="sidebar__avatar">
                        {user?.fullName?.[0]?.toUpperCase()}
                    </div>
                    <div className="sidebar__user-info">
                        <span className="sidebar__user-name">{user?.fullName}</span>
                        <span className="sidebar__user-role">{user?.role}</span>
                    </div>
                </div>
                <button className="sidebar__logout" onClick={handleLogout} title={t('nav.logout')}>
                    <LogOut size={16} />
                </button>
            </div>
        </aside>
    )
}