import { Sidebar } from './Sidebar'
import { TopBar } from './TopBar'

interface PageWrapperProps {
    children: React.ReactNode
    title: string
    actions?: React.ReactNode
}

export function PageWrapper({ children, title, actions }: PageWrapperProps) {
    return (
        <div className="layout">
            <Sidebar />
            <div className="layout__main">
                <TopBar title={title} actions={actions} />
                <main className="layout__content">{children}</main>
            </div>
        </div>
    )
}