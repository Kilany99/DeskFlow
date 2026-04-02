
import React from 'react'
import ReactDOM from 'react-dom/client'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { AppRouter } from './routes/AppRouter'
import './i18n'
import './styles/globals.css'
import i18n from './i18n'

const queryClient = new QueryClient({
    defaultOptions: { queries: { retry: 1, staleTime: 30_000 } }
})

// Apply RTL direction to <html> whenever language changes
const applyDirection = (lang: string) => {
    document.documentElement.dir = lang === 'ar' ? 'rtl' : 'ltr'
    document.documentElement.lang = lang
}

applyDirection(i18n.language)
i18n.on('languageChanged', applyDirection)

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <QueryClientProvider client={queryClient}>
            <AppRouter />
        </QueryClientProvider>
    </React.StrictMode>
)