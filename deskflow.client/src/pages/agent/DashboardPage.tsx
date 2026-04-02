
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useQuery } from '@tanstack/react-query'
import { useTranslation } from 'react-i18next'
import { Plus, Search, Filter, Ticket, Clock, CheckCircle, AlertCircle } from 'lucide-react'
import { ticketsApi } from '../../api/tickets.api'
import { useAuthStore } from '../../store/authStore'
import { PageWrapper } from '../../components/layout/PageWrapper'
import { Button } from '../../components/ui/Button'
import { Badge } from '../../components/ui/Badge'
import { Card } from '../../components/ui/Card'
import { Spinner } from '../../components/ui/Spinner'
import type { Ticket as TicketType, TicketStatus } from '../../types'

const STATUS_FILTERS: { label: string; value: TicketStatus | 'All' }[] = [
    { label: 'All', value: 'All' },
    { label: 'Open', value: 'Open' },
    { label: 'In Progress', value: 'InProgress' },
    { label: 'Resolved', value: 'Resolved' },
    { label: 'Closed', value: 'Closed' },
]

export function DashboardPage() {
    const { t } = useTranslation()
    const navigate = useNavigate()
    const { user } = useAuthStore()
    const [search, setSearch] = useState('')
    const [statusFilter, setStatusFilter] = useState<TicketStatus | 'All'>('All')
    const [page, setPage] = useState(1)

    const { data, isLoading } = useQuery({
        queryKey: ['tickets', page],
        queryFn: () => ticketsApi.getAll(page, 10).then(r => r.data),
    })

    const tickets = data?.items ?? []

    const filtered = tickets.filter(t => {
        const matchSearch =
            t.subject.toLowerCase().includes(search.toLowerCase()) ||
            t.referenceNumber.toLowerCase().includes(search.toLowerCase()) ||
            t.customerName.toLowerCase().includes(search.toLowerCase())
        const matchStatus = statusFilter === 'All' || t.status === statusFilter
        return matchSearch && matchStatus
    })

    const stats = {
        total: data?.totalCount ?? 0,
        open: tickets.filter(t => t.status === 'Open').length,
        inProgress: tickets.filter(t => t.status === 'InProgress').length,
        resolved: tickets.filter(t => t.status === 'Resolved').length,
    }

    return (
        <PageWrapper
            title={t('nav.dashboard')}
            actions={
                user?.role !== 'Agent' ? (
                    <Button icon={<Plus size={15} />} onClick={() => navigate('/tickets/new')}>
                        {t('tickets.new')}
                    </Button>
                ) : undefined
            }
        >
            {/* Stats row */}
            <div className="stats-grid">
                <StatCard icon={<Ticket size={18} />} label={t('dashboard.totalTickets')} value={stats.total} color="indigo" />
                <StatCard icon={<AlertCircle size={18} />} label={t('dashboard.open')} value={stats.open} color="blue" />
                <StatCard icon={<Clock size={18} />} label={t('dashboard.inProgress')} value={stats.inProgress} color="amber" />
                <StatCard icon={<CheckCircle size={18} />} label={t('dashboard.resolved')} value={stats.resolved} color="green" />
            </div>

            {/* Filters */}
            <div className="table-toolbar">
                <div className="table-search">
                    <Search size={15} className="table-search__icon" />
                    <input
                        className="table-search__input"
                        placeholder={t('common.search') + '...'}
                        value={search}
                        onChange={e => setSearch(e.target.value)}
                    />
                </div>
                <div className="filter-tabs">
                    {STATUS_FILTERS.map(f => (
                        <button
                            key={f.value}
                            className={`filter-tab${statusFilter === f.value ? ' filter-tab--active' : ''}`}
                            onClick={() => setStatusFilter(f.value)}
                        >
                            {f.label}
                        </button>
                    ))}
                </div>
            </div>

            {/* Ticket table */}
            <Card padding="sm">
                {isLoading ? (
                    <div style={{ padding: '48px' }}><Spinner /></div>
                ) : filtered.length === 0 ? (
                    <div className="empty-state">
                        <Ticket size={32} />
                        <p>{t('common.noData')}</p>
                    </div>
                ) : (
                    <table className="data-table">
                        <thead>
                            <tr>
                                <th>{t('tickets.reference')}</th>
                                <th>{t('tickets.subject')}</th>
                                <th>{t('tickets.customer')}</th>
                                <th>{t('tickets.fields.priority')}</th>
                                <th>{t('tickets.fields.status')}</th>
                                <th>{t('tickets.assignedTo')}</th>
                                <th>{t('tickets.created')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {filtered.map(ticket => (
                                <TicketRow
                                    key={ticket.id}
                                    ticket={ticket}
                                    onClick={() => navigate(`/tickets/${ticket.id}`)}
                                />
                            ))}
                        </tbody>
                    </table>
                )}
            </Card>

            {/* Pagination */}
            {data && data.totalPages > 1 && (
                <div className="pagination">
                    <Button
                        variant="secondary" size="sm"
                        disabled={!data.hasPrevious}
                        onClick={() => setPage(p => p - 1)}
                    >
                        Previous
                    </Button>
                    <span className="pagination__info">
                        {t('common.page')} {data.page} {t('common.of')} {data.totalPages}
                    </span>
                    <Button
                        variant="secondary" size="sm"
                        disabled={!data.hasNext}
                        onClick={() => setPage(p => p + 1)}
                    >
                        Next
                    </Button>
                </div>
            )}
        </PageWrapper>
    )
}

function StatCard({ icon, label, value, color }: {
    icon: React.ReactNode
    label: string
    value: number
    color: 'indigo' | 'blue' | 'amber' | 'green'
}) {
    return (
        <div className={`stat-card stat-card--${color}`}>
            <div className="stat-card__icon">{icon}</div>
            <div>
                <div className="stat-card__value">{value}</div>
                <div className="stat-card__label">{label}</div>
            </div>
        </div>
    )
}

function TicketRow({ ticket, onClick }: { ticket: TicketType; onClick: () => void }) {
    const { t } = useTranslation()
    return (
        <tr className="data-table__row" onClick={onClick}>
            <td>
                <span className="ticket-ref">{ticket.referenceNumber}</span>
            </td>
            <td>
                <span className="ticket-subject">{ticket.subject}</span>
            </td>
            <td>
                <div className="ticket-customer">
                    <div className="ticket-customer__avatar">
                        {ticket.customerName[0].toUpperCase()}
                    </div>
                    <div>
                        <div className="ticket-customer__name">{ticket.customerName}</div>
                        <div className="ticket-customer__email">{ticket.customerEmail}</div>
                    </div>
                </div>
            </td>
            <td>
                <Badge
                    label={t(`tickets.priority.${ticket.priority}`)}
                    variant="priority"
                    value={ticket.priority}
                />
            </td>
            <td>
                <Badge
                    label={t(`tickets.status.${ticket.status}`)}
                    variant="status"
                    value={ticket.status}
                />
            </td>
            <td>
                <span className="text-sm text-muted">
                    {ticket.assignedToName ?? '—'}
                </span>
            </td>
            <td>
                <span className="text-sm text-muted">
                    {new Date(ticket.createdAt).toLocaleDateString()}
                </span>
            </td>
        </tr>
    )
}