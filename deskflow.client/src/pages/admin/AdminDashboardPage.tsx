
import { useQuery } from '@tanstack/react-query'
import { useTranslation } from 'react-i18next'
import { Users, Ticket, Building2, Activity } from 'lucide-react'
import { ticketsApi } from '../../api/tickets.api'
import { usersApi } from '../../api/users.api'
import { PageWrapper } from '../../components/layout/PageWrapper'
import { Card } from '../../components/ui/Card'
import { Badge } from '../../components/ui/Badge'
import { Spinner } from '../../components/ui/Spinner'
import { useNavigate } from 'react-router-dom'

export function AdminDashboardPage() {
    const { t } = useTranslation()
    const navigate = useNavigate()

    const { data: ticketData, isLoading: loadingTickets } = useQuery({
        queryKey: ['tickets-admin'],
        queryFn: () => ticketsApi.getAll(1, 5).then(r => r.data),
    })

    const { data: users, isLoading: loadingUsers } = useQuery({
        queryKey: ['users'],
        queryFn: () => usersApi.getAll().then(r => r.data),
    })

    const tickets = ticketData?.items ?? []
    const activeAgents = users?.filter(u => u.role === 'Agent' && u.isActive).length ?? 0

    return (
        <PageWrapper title="Admin Dashboard">
            <div className="stats-grid">
                <div className="stat-card stat-card--indigo">
                    <div className="stat-card__icon"><Ticket size={18} /></div>
                    <div>
                        <div className="stat-card__value">{ticketData?.totalCount ?? 0}</div>
                        <div className="stat-card__label">Total Tickets</div>
                    </div>
                </div>
                <div className="stat-card stat-card--blue">
                    <div className="stat-card__icon"><Users size={18} /></div>
                    <div>
                        <div className="stat-card__value">{activeAgents}</div>
                        <div className="stat-card__label">Active Agents</div>
                    </div>
                </div>
                <div className="stat-card stat-card--amber">
                    <div className="stat-card__icon"><Activity size={18} /></div>
                    <div>
                        <div className="stat-card__value">
                            {tickets.filter(t => t.status === 'Open').length}
                        </div>
                        <div className="stat-card__label">Open Tickets</div>
                    </div>
                </div>
                <div className="stat-card stat-card--green">
                    <div className="stat-card__icon"><Building2 size={18} /></div>
                    <div>
                        <div className="stat-card__value">{users?.length ?? 0}</div>
                        <div className="stat-card__label">Team Members</div>
                    </div>
                </div>
            </div>

            <div className="admin-grid">
                <Card padding="md">
                    <div className="section-header">
                        <h3 className="section-title">Recent Tickets</h3>
                        <button className="link-btn" onClick={() => navigate('/tickets')}>View all</button>
                    </div>
                    {loadingTickets ? <Spinner /> : (
                        <div className="recent-list">
                            {tickets.map(ticket => (
                                <div
                                    key={ticket.id}
                                    className="recent-item"
                                    onClick={() => navigate(`/tickets/${ticket.id}`)}
                                >
                                    <div className="recent-item__left">
                                        <span className="ticket-ref">{ticket.referenceNumber}</span>
                                        <span className="recent-item__subject">{ticket.subject}</span>
                                    </div>
                                    <Badge
                                        label={t(`tickets.status.${ticket.status}`)}
                                        variant="status"
                                        value={ticket.status}
                                    />
                                </div>
                            ))}
                        </div>
                    )}
                </Card>

                <Card padding="md">
                    <div className="section-header">
                        <h3 className="section-title">Team</h3>
                        <button className="link-btn" onClick={() => navigate('/users')}>Manage</button>
                    </div>
                    {loadingUsers ? <Spinner /> : (
                        <div className="recent-list">
                            {users?.slice(0, 5).map(user => (
                                <div key={user.id} className="recent-item">
                                    <div className="ticket-customer">
                                        <div className="ticket-customer__avatar">{user.fullName[0]}</div>
                                        <div>
                                            <div className="ticket-customer__name">{user.fullName}</div>
                                            <div className="ticket-customer__email">{user.email}</div>
                                        </div>
                                    </div>
                                    <Badge label={t(`users.roles.${user.role}`)} />
                                </div>
                            ))}
                        </div>
                    )}
                </Card>
            </div>
        </PageWrapper>
    )
}