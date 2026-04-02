import clsx from 'clsx'
import { Loader2 } from 'lucide-react'

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
    variant?: 'primary' | 'secondary' | 'ghost' | 'danger'
    size?: 'sm' | 'md' | 'lg'
    loading?: boolean
    icon?: React.ReactNode
}

export function Button({
    variant = 'primary',
    size = 'md',
    loading,
    icon,
    children,
    className,
    disabled,
    ...props
}: ButtonProps) {
    return (
        <button
            className={clsx('btn', `btn--${variant}`, `btn--${size}`, className)}
            disabled={disabled || loading}
            {...props}
        >
            {loading ? <Loader2 size={14} className="spin" /> : icon}
            {children}
        </button>
    )
}