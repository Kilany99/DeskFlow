
import { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { useTranslation } from 'react-i18next'
import { Search, Shield } from 'lucide-react'
import api from '../../api/axios'
import { useAuthStore } from '../../store/authStore'
import { PageWrapper } from '../../components/layout/PageWrapper'
import { Card } from '../../components/ui/Card'
import { Spinner } from '../../components/ui/Spinner'
import type { AuditLog } from '../../types'

const ACTION_COLORS: Record<string, string> = {
    AUTH_LOGIN: 'audit-tag audit-tag--blue',
    AUTH_LOGOUT: 'audit-tag audit-tag--gray',
    TICKET_CREATED: 'audit-tag audit-tag--green',
    TICKET_UPDATED: 'audit-tag audit-tag--amber',
    TICKET_ASSIGNED: 'audit-tag audit-tag--indigo',
    TICKET_REPLIED: 'audit-tag audit-tag--indigo',
    TICKET_CLOSED: 'audit-tag audit-tag--gray',
    USER_INVITED: 'audit-tag audit-tag--green',
    USER_DEACTIVATED: 'audit-tag audit-tag--red',
    USER_ACTIVATED: 'audit-tag audit-tag--green',
    USER_ROLE_CHANGED: 'audit-tag audit-tag--amber',
    TENANT_CREATED: 'audit-tag audit-tag--green',
    TENANT_SUSPENDED: 'audit-tag audit-tag--red',
    PLAN_CHANGED: 'audit-tag audit-tag--indigo',
}

export function AuditLogPage() {
    const { t } = useTranslation()
    const { user } = useAuthStore()
    const [search, setSearch] = useState('')

    const endpoint = user?.role === 'SuperAdmin' ? '/auditlogs/all' : '/auditlogs'

    const { data: logs, isLoading } = useQuery({
        queryKey: ['audit-logs', user?.role],
        queryFn: () => api.get<AuditLog[]>(endpoint).then(r => r.data),
    })

    const filtered = (logs ?? []).filter(log => {
        const q = search.toLowerCase()
        return (
            log.action.toLowerCase().includes(q) ||
            (log.userFullName ?? '').toLowerCase().includes(q) ||
            (log.details ?? '').toLowerCase().includes(q)
        )
    })

    return (
        <PageWrapper title={t('audit.title')}>
            <div className="table-toolbar">
                <div className="table-search">
                    <Search size={15} className="table-search__icon" />
                    <input
                        className="table-search__input"
                        placeholder={`${t('common.search')}...`}
                        value={search}
                        onChange={e => setSearch(e.target.value)}
                    />
                </div>
                <span className="audit-count">
                    {filtered.length} {filtered.length === 1 ? 'entry' : 'entries'}
                </span>
            </div>

            <Card padding="sm">
                {isLoading ? (
                    <div style={{ padding: '48px' }}><Spinner /></div>
                ) : filtered.length === 0 ? (
                    <div className="empty-state">
                        <Shield size={32} />
                        <p>{t('audit.noLogs')}</p>
                    </div>
                ) : (
                    <table className="data-table">
                        <thead>
                            <tr>
                                <th>{t('audit.action')}</th>
                                <th>{t('audit.user')}</th>
                                <th>{t('audit.details')}</th>
                                <th>{t('audit.time')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {filtered.map(log => (
                                <tr key={log.id} className="data-table__row" style={{ cursor: 'default' }}>
                                    <td>
                                        <span className={ACTION_COLORS[log.action] ?? 'audit-tag audit-tag--gray'}>
                                            {log.action.replace(/_/g, ' ')}
                                        </span>
                                    </td>
                                    <td>
                                        {log.userFullName ? (
                                            <div className="ticket-customer">
                                                <div className="ticket-customer__avatar">
                                                    {log.userFullName[0]}
                                                </div>
                                                <div>
                                                    <div className="ticket-customer__name">{log.userFullName}</div>
                                                </div>
                                            </div>
                                        ) : (
                                            <span className="audit-system">
                                                <Shield size={12} />
                                                {t('audit.system')}
                                            </span>
                                        )}
                                    </td>
                                    <td>
                                        <span className="audit-details">
                                            {log.details ?? '—'}
                                        </span>
                                    </td>
                                    <td>
                                        <div className="audit-time">
                                            <span>{new Date(log.createdAt).toLocaleDateString()}</span>
                                            <span className="audit-time__clock">
                                                {new Date(log.createdAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </Card>
        </PageWrapper>
    )
}