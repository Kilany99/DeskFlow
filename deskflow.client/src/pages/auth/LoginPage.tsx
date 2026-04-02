
import { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { Mail, Lock, Globe } from 'lucide-react'
import { authApi } from '../../api/auth.api'
import { useAuthStore } from '../../store/authStore'
import { Button } from '../../components/ui/Button'
import { Input } from '../../components/ui/Input'
import { useFormValidation, rules } from '../../hooks/useFormValidation'

export function LoginPage() {
    const { t, i18n } = useTranslation()
    const navigate = useNavigate()
    const setUser = useAuthStore(s => s.setUser)
    const isRtl = i18n.language === 'ar'

    const [form, setForm] = useState({ email: '', password: '', subdomain: '' })
    const [serverError, setServerError] = useState('')
    const [loading, setLoading] = useState(false)

    const { errors, validate, clearError } = useFormValidation(form, {
        subdomain: [
            rules.required(t('auth.validation.subdomainRequired')),
            rules.minLength(3, t('auth.validation.subdomainMin')),
        ],
        email: [
            rules.required(t('auth.validation.emailRequired')),
            rules.email(t('auth.validation.emailInvalid')),
        ],
        password: [
            rules.required(t('auth.validation.passwordRequired')),
            rules.minLength(8, t('auth.validation.passwordMin')),
        ],
    })

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault()
        if (!validate()) return
        setServerError('')
        setLoading(true)
        try {
            const res = await authApi.login(form)
            setUser(res.data)
            navigate('/dashboard')
        } catch {
            setServerError(t('auth.invalidCredentials'))
        } finally {
            setLoading(false)
        }
    }

    const toggleLang = () =>
        i18n.changeLanguage(isRtl ? 'en' : 'ar')

    return (
        <div className="auth-page">
            <div className="auth-mesh">
                <div className="auth-mesh__orb auth-mesh__orb--1" />
                <div className="auth-mesh__orb auth-mesh__orb--2" />
                <div className="auth-mesh__orb auth-mesh__orb--3" />
                <div className="auth-card-container">
                    <div className="auth-card">
                        <div className="auth-card__top">
                            <div className="auth-brand">
                                <div className="auth-brand__logo">D</div>
                                <span className="auth-brand__name">DeskFlow</span>
                            </div>
                            <button className="auth-lang" onClick={toggleLang}>
                                {isRtl ? 'English' : 'العربية'}
                            </button>
                        </div>

                        <div className="auth-card__header">
                            <h1>{t('auth.loginTitle')}</h1>
                            <p>{t('auth.loginSubtitle')}</p>
                        </div>

                        <form onSubmit={handleSubmit} className="auth-form" noValidate>
                            <Input
                                label={t('auth.subdomain')}
                                placeholder="acme"
                                value={form.subdomain}
                                onChange={e => {
                                    setForm(f => ({ ...f, subdomain: e.target.value }))
                                    clearError('subdomain')
                                }}
                                icon={<Globe size={15} />}
                                error={errors.subdomain}
                            />
                            <Input
                                label={t('auth.email')}
                                type="email"
                                placeholder="you@company.com"
                                value={form.email}
                                onChange={e => {
                                    setForm(f => ({ ...f, email: e.target.value }))
                                    clearError('email')
                                }}
                                icon={<Mail size={15} />}
                                error={errors.email}
                            />
                            <Input
                                label={t('auth.password')}
                                type="password"
                                placeholder="••••••••"
                                value={form.password}
                                onChange={e => {
                                    setForm(f => ({ ...f, password: e.target.value }))
                                    clearError('password')
                                }}
                                icon={<Lock size={15} />}
                                error={errors.password}
                            />

                            {serverError && <div className="auth-error">{serverError}</div>}

                            <Button type="submit" loading={loading} size="lg" style={{ width: '100%', marginTop: '4px' }}>
                                {t('auth.login')}
                            </Button>
                        </form>

                        <p className="auth-switch">
                            {t('auth.dontHaveAccount')}{' '}
                            <Link to="/register">{t('auth.createOne')}</Link>
                        </p>

                        <div className="auth-card__footer">
                            <div className="auth-demo">
                                <span className="auth-demo__label">Quick login</span>
                                <button
                                    className="auth-demo__btn"
                                    type="button"
                                    onClick={() => setForm({
                                        subdomain: 'acme',
                                        email: 'alice@acme.com',
                                        password: 'Acme@123'
                                    })}
                                >
                                    Acme Admin
                                </button>
                                <button
                                    className="auth-demo__btn"
                                    type="button"
                                    onClick={() => setForm({
                                        subdomain: 'acme',
                                        email: 'bob@acme.com',
                                        password: 'Acme@123'
                                    })}
                                >
                                    Agent
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}