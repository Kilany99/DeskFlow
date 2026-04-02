import clsx from 'clsx'

interface CardProps {
    children: React.ReactNode
    className?: string
    padding?: 'sm' | 'md' | 'lg'
    hoverable?: boolean
}

export function Card({ children, className, padding = 'md', hoverable }: CardProps) {
    return (
        <div className={clsx('card', `card--${padding}`, hoverable && 'card--hoverable', className)}>
            {children}
        </div>
    )
}