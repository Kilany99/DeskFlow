import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { useTranslation } from 'react-i18next'
import { Building2 } from 'lucide-react'
import { tenantsApi } from '../../api/tenants.api'
import { PageWrapper } from '../../components/layout/PageWrapper'
import { Button } from '../../components/ui/Button'
import { Badge } from '../../components/ui/Badge'
import { Card } from '../../components/ui/Card'
import { Spinner } from '../../components/ui/Spinner'
import type { PlanType } from '../../types'

const PLAN_COLORS: Record<PlanType, string> = {
    Free: 'badge--closed',
    Pro: 'badge--open',
    Enterprise: 'badge--resolved',
}

export function TenantsPage() {
    const { t } = useTranslation()
    const queryClient = useQueryClient()

    const { data: tenants, isLoading } = useQuery({
        queryKey: ['tenants'],
        queryFn: () => tenantsApi.getAll().then(r => r.data),
    })

    const suspendMutation = useMutation({
        mutationFn: ({ id, active }: { id: string; active: boolean }) =>
            active ? tenantsApi.suspend(id) : tenantsApi.unsuspend(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['tenants'] }),
    })

    const planMutation = useMutation({
        mutationFn: ({ id, plan }: { id: string; plan: PlanType }) =>
            tenantsApi.changePlan(id, plan),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['tenants'] }),
    })

    return (
        <PageWrapper title={t('tenants.title')}>
            <Card padding="sm">
                {isLoading ? <div style={{ padding: '48px' }}><Spinner /></div> : (
                    <table className="data-table">
                        <thead>
                            <tr>
                                <th>Company</th>
                                <th>Subdomain</th>
                                <th>Plan</th>
                                <th>Members</th>
                                <th>Status</th>
                                <th>Created</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {tenants?.map(tenant => (
                                <tr key={tenant.id} className="data-table__row">
                                    <td>
                                        <div className="ticket-customer">
                                            <div className="ticket-customer__avatar">
                                                <Building2 size={13} />
                                            </div>
                                            <span className="ticket-customer__name">{tenant.name}</span>
                                        </div>
                                    </td>
                                    <td>
                                        <span className="mono text-sm">{tenant.subdomain}</span>
                                    </td>
                                    <td>
                                        <select
                                            className="panel-select panel-select--sm"
                                            value={tenant.plan}
                                            onChange={e => planMutation.mutate({
                                                id: tenant.id,
                                                plan: e.target.value as PlanType
                                            })}
                                        >
                                            <option value="Free">Free</option>
                                            <option value="Pro">Pro</option>
                                            <option value="Enterprise">Enterprise</option>
                                        </select>
                                    </td>
                                    <td>
                                        <span className="text-sm">{tenant.memberCount}</span>
                                    </td>
                                    <td>
                                        <Badge
                                            label={tenant.isActive ? t('common.active') : 'Suspended'}
                                            className={tenant.isActive ? 'badge--resolved' : 'badge--critical'}
                                        />
                                    </td>
                                    <td>
                                        <span className="text-sm text-muted">
                                            {new Date(tenant.createdAt).toLocaleDateString()}
                                        </span>
                                    </td>
                                    <td>
                                        <Button
                                            variant={tenant.isActive ? 'danger' : 'secondary'}
                                            size="sm"
                                            loading={suspendMutation.isPending}
                                            onClick={() => suspendMutation.mutate({
                                                id: tenant.id,
                                                active: tenant.isActive
                                            })}
                                        >
                                            {tenant.isActive ? t('tenants.suspend') : t('tenants.unsuspend')}
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