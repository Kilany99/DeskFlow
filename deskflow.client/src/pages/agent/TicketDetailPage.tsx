
import { useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { useTranslation } from 'react-i18next'
import { ArrowLeft, Send, User, Headphones } from 'lucide-react'
import { ticketsApi } from '../../api/tickets.api'
import { useAuthStore } from '../../store/authStore'
import { PageWrapper } from '../../components/layout/PageWrapper'
import { Button } from '../../components/ui/Button'
import { Badge } from '../../components/ui/Badge'
import { Card } from '../../components/ui/Card'
import { Spinner } from '../../components/ui/Spinner'
import type { TicketStatus, TicketPriority } from '../../types'

export function TicketDetailPage() {
    const { id } = useParams<{ id: string }>()
    const navigate = useNavigate()
    const { t } = useTranslation()
    const { user } = useAuthStore()
    const queryClient = useQueryClient()
    const [reply, setReply] = useState('')

    const { data: ticket, isLoading } = useQuery({
        queryKey: ['ticket', id],
        queryFn: () => ticketsApi.getById(id!).then(r => r.data),
        enabled: !!id,
    })

    const replyMutation = useMutation({
        mutationFn: () => ticketsApi.reply(id!, { message: reply, isFromCustomer: false }),
        onSuccess: () => {
            setReply('')
            queryClient.invalidateQueries({ queryKey: ['ticket', id] })
        },
    })

    const updateMutation = useMutation({
        mutationFn: (data: { status?: TicketStatus; priority?: TicketPriority }) =>
            ticketsApi.update(id!, data),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['ticket', id] }),
    })

    if (isLoading) return (
        <PageWrapper title="Ticket">
            <div style={{ padding: '80px' }}><Spinner /></div>
        </PageWrapper>
    )

    if (!ticket) return (
        <PageWrapper title="Ticket">
            <div className="empty-state"><p>Ticket not found.</p></div>
        </PageWrapper>
    )

    return (
        <PageWrapper
            title={ticket.referenceNumber}
            actions={
                <Button variant="ghost" icon={<ArrowLeft size={15} />} onClick={() => navigate(-1)}>
                    Back
                </Button>
            }
        >
            <div className="ticket-detail">

                {/* Left — thread */}
                <div className="ticket-detail__main">
                    <Card padding="md">
                        <div className="ticket-detail__header">
                            <div>
                                <h2 className="ticket-detail__subject">{ticket.subject}</h2>
                                <p className="ticket-detail__meta">
                                    From <strong>{ticket.customerName}</strong> · {ticket.customerEmail}
                                </p>
                            </div>
                            <div className="ticket-detail__badges">
                                <Badge label={t(`tickets.status.${ticket.status}`)} variant="status" value={ticket.status} />
                                <Badge label={t(`tickets.priority.${ticket.priority}`)} variant="priority" value={ticket.priority} />
                            </div>
                        </div>

                        <div className="ticket-description">
                            {ticket.description}
                        </div>
                    </Card>

                    {/* Replies thread */}
                    <div className="reply-thread">
                        {ticket.replies.map(r => (
                            <div key={r.id} className={`reply-bubble ${r.isFromCustomer ? 'reply-bubble--customer' : 'reply-bubble--agent'}`}>
                                <div className="reply-bubble__avatar">
                                    {r.isFromCustomer
                                        ? <User size={14} />
                                        : <Headphones size={14} />
                                    }
                                </div>
                                <div className="reply-bubble__body">
                                    <div className="reply-bubble__meta">
                                        <span className="reply-bubble__author">
                                            {r.isFromCustomer ? ticket.customerName : (r.agentName ?? 'Agent')}
                                        </span>
                                        <span className="reply-bubble__time">
                                            {new Date(r.createdAt).toLocaleString()}
                                        </span>
                                    </div>
                                    <p className="reply-bubble__message">{r.message}</p>
                                </div>
                            </div>
                        ))}
                    </div>

                    {/* Reply box */}
                    <Card padding="md">
                        <div className="reply-box">
                            <textarea
                                className="reply-box__input"
                                placeholder={t('tickets.reply.placeholder')}
                                value={reply}
                                rows={4}
                                onChange={e => setReply(e.target.value)}
                            />
                            <div className="reply-box__footer">
                                <Button
                                    icon={<Send size={14} />}
                                    loading={replyMutation.isPending}
                                    disabled={!reply.trim()}
                                    onClick={() => replyMutation.mutate()}
                                >
                                    {t('tickets.reply.send')}
                                </Button>
                            </div>
                        </div>
                    </Card>
                </div>

                {/* Right — meta panel */}
                <div className="ticket-detail__sidebar">
                    <Card padding="md">
                        <h3 className="panel-title">Details</h3>
                        <div className="panel-fields">

                            <div className="panel-field">
                                <span className="panel-field__label">{t('tickets.fields.status')}</span>
                                <select
                                    className="panel-select"
                                    value={ticket.status}
                                    onChange={e => updateMutation.mutate({ status: e.target.value as TicketStatus })}
                                >
                                    {(['Open', 'InProgress', 'Resolved', 'Closed'] as TicketStatus[]).map(s => (
                                        <option key={s} value={s}>{t(`tickets.status.${s}`)}</option>
                                    ))}
                                </select>
                            </div>

                            <div className="panel-field">
                                <span className="panel-field__label">{t('tickets.fields.priority')}</span>
                                <select
                                    className="panel-select"
                                    value={ticket.priority}
                                    onChange={e => updateMutation.mutate({ priority: e.target.value as TicketPriority })}
                                >
                                    {(['Low', 'Medium', 'High', 'Critical'] as TicketPriority[]).map(p => (
                                        <option key={p} value={p}>{t(`tickets.priority.${p}`)}</option>
                                    ))}
                                </select>
                            </div>

                            <div className="panel-field">
                                <span className="panel-field__label">{t('tickets.fields.assignedTo')}</span>
                                <span className="panel-field__value">{ticket.assignedToName ?? '—'}</span>
                            </div>

                            <div className="panel-field">
                                <span className="panel-field__label">{t('tickets.fields.reference')}</span>
                                <span className="panel-field__value mono">{ticket.referenceNumber}</span>
                            </div>

                            <div className="panel-field">
                                <span className="panel-field__label">{t('tickets.fields.created')}</span>
                                <span className="panel-field__value">
                                    {new Date(ticket.createdAt).toLocaleDateString()}
                                </span>
                            </div>
                        </div>
                    </Card>
                </div>

            </div>
        </PageWrapper>
    )
}