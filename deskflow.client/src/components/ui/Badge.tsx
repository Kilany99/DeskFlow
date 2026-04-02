import clsx from 'clsx'
import type { TicketStatus, TicketPriority } from '../../types'

const statusStyles: Record<TicketStatus, string> = {
    Open: 'badge--open',
    InProgress: 'badge--progress',
    Resolved: 'badge--resolved',
    Closed: 'badge--closed',
}

const priorityStyles: Record<TicketPriority, string> = {
    Low: 'badge--low',
    Medium: 'badge--medium',
    High: 'badge--high',
    Critical: 'badge--critical',
}

interface BadgeProps {
    label: string
    variant?: 'status' | 'priority' | 'plan'
    value?: string
    className?: string
}

export function Badge({ label, variant, value, className }: BadgeProps) {
    const variantClass =
        variant === 'status' && value ? statusStyles[value as TicketStatus] :
            variant === 'priority' && value ? priorityStyles[value as TicketPriority] : ''

    return (
        <span className={clsx('badge', variantClass, className)}>
            {label}
        </span>
    )
}

