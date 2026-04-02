import { useState } from 'react'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { useTranslation } from 'react-i18next'
import { UserPlus, Mail, User, Shield } from 'lucide-react'
import { usersApi } from '../../api/users.api'
import { PageWrapper } from '../../components/layout/PageWrapper'
import { Button } from '../../components/ui/Button'
import { Input } from '../../components/ui/Input'
import { Badge } from '../../components/ui/Badge'
import { Card } from '../../components/ui/Card'
import { Spinner } from '../../components/ui/Spinner'
import type { UserRole } from '../../types'

export function UsersPage() {
    const { t } = useTranslation()
    const queryClient = useQueryClient()
    const [showInvite, setShowInvite] = useState(false)
    const [form, setForm] = useState({ fullName: '', email: '', role: 'Agent' as UserRole })

    const { data: users, isLoading } = useQuery({
        queryKey: ['users'],
        queryFn: () => usersApi.getAll().then(r => r.data),
    })

    const inviteMutation = useMutation({
        mutationFn: () => usersApi.invite(form),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['users'] })
            setShowInvite(false)
            setForm({ fullName: '', email: '', role: 'Agent' })
        },
    })

    const toggleActive = useMutation({
        mutationFn: ({ id, active }: { id: string; active: boolean }) =>
            active ? usersApi.deactivate(id) : usersApi.activate(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['users'] }),
    })

    return (
        <PageWrapper
            title={t('users.title')}
            actions={
                <Button icon={<UserPlus size={15} />} onClick={() => setShowInvite(true)}>
                    {t('users.invite')}
                </Button>
            }
        >
            {/* Invite modal */}
            {showInvite && (
                <div className="modal-overlay" onClick={() => setShowInvite(false)}>
                    <div className="modal" onClick={e => e.stopPropagation()}>
                        <h2 className="modal__title">{t('users.invite')}</h2>
                        <div className="modal__body">
                            <Input
                                label={t('auth.fullName')}
                                value={form.fullName}
                                onChange={e => setForm(f => ({ ...f, fullName: e.target.value }))}
                                icon={<User size={15} />}
                            />
                            <Input
                                label={t('auth.email')}
                                type="email"
                                value={form.email}
                                onChange={e => setForm(f => ({ ...f, email: e.target.value }))}
                                icon={<Mail size={15} />}
                            />
                            <div className="input-group">
                                <label className="input-label">Role</label>
                                <select
                                    className="panel-select"
                                    value={form.role}
                                    onChange={e => setForm(f => ({ ...f, role: e.target.value as UserRole }))}
                                >
                                    <option value="Agent">Agent</option>
                                    <option value="TenantAdmin">Admin</option>
                                </select>
                            </div>
                        </div>
                        <div className="modal__footer">
                            <Button variant="secondary" onClick={() => setShowInvite(false)}>
                                {t('common.cancel')}
                            </Button>
                            <Button
                                loading={inviteMutation.isPending}
                                onClick={() => inviteMutation.mutate()}
                            >
                                Send Invite
                            </Button>
                        </div>
                    </div>
                </div>
            )}

            <Card padding="sm">
                {isLoading ? <div style={{ padding: '48px' }}><Spinner /></div> : (
                    <table className="data-table">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Email</th>
                                <th>Role</th>
                                <th>Status</th>
                                <th>Joined</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {users?.map(user => (
                                <tr key={user.id} className="data-table__row">
                                    <td>
                                        <div className="ticket-customer">
                                            <div className="ticket-customer__avatar">{user.fullName[0]}</div>
                                            <span className="ticket-customer__name">{user.fullName}</span>
                                        </div>
                                    </td>
                                    <td><span className="text-sm text-muted">{user.email}</span></td>
                                    <td><Badge label={t(`users.roles.${user.role}`)} /></td>
                                    <td>
                                        <Badge
                                            label={user.isActive ? t('common.active') : t('common.inactive')}
                                            className={user.isActive ? 'badge--resolved' : 'badge--closed'}
                                        />
                                    </td>
                                    <td>
                                        <span className="text-sm text-muted">
                                            {new Date(user.createdAt).toLocaleDateString()}
                                        </span>
                                    </td>
                                    <td>
                                        <Button
                                            variant={user.isActive ? 'danger' : 'secondary'}
                                            size="sm"
                                            loading={toggleActive.isPending}
                                            onClick={() => toggleActive.mutate({ id: user.id, active: user.isActive })}
                                        >
                                            {user.isActive ? 'Deactivate' : 'Activate'}
                                        </Button>
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