import { useTranslation } from 'react-i18next'

interface TopBarProps {
    title: string
    actions?: React.ReactNode
}

export function TopBar({ title, actions }: TopBarProps) {
    const { i18n } = useTranslation()

    const toggleLang = () =>
        i18n.changeLanguage(i18n.language === 'en' ? 'ar' : 'en')

    return (
        <header className="topbar">
            <h1 className="topbar__title">{title}</h1>
            <div className="topbar__right">
                {actions}
                <button className="topbar__lang" onClick={toggleLang}>
                    {i18n.language === 'en' ? 'ع' : 'EN'}
                </button>
            </div>
        </header>
    )
}