import clsx from 'clsx'

interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
    label?: string
    error?: string
    icon?: React.ReactNode
}

export function Input({ label, error, icon, className, ...props }: InputProps) {
    return (
        <div className="input-group">
            {label && <label className="input-label">{label}</label>}
            <div className="input-wrapper">
                {icon && <span className="input-icon">{icon}</span>}
                <input
                    className={clsx('input', icon && 'input--with-icon', error && 'input--error', className)}
                    {...props}
                />
            </div>
            {error && <span className="input-error">{error}</span>}
        </div>
    )
}
