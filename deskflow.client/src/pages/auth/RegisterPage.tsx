import { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { Mail, Lock, Globe, Building2, User } from 'lucide-react'
import { authApi } from '../../api/auth.api'
import { useAuthStore } from '../../store/authStore'
import { Button } from '../../components/ui/Button'
import { Input } from '../../components/ui/Input'
import { useFormValidation, rules } from '../../hooks/useFormValidation'

export function RegisterPage() {
    const { t, i18n } = useTranslation()
    const navigate = useNavigate()
    const setUser = useAuthStore(s => s.setUser)
    const isRtl = i18n.language === 'ar'

    const [form, setForm] = useState({
        companyName: '', subdomain: '',
        adminFullName: '', adminEmail: '', password: '',
    })
    const [serverError, setServerError] = useState('')
    const [loading, setLoading] = useState(false)

    const { errors, validate, clearError } = useFormValidation(form, {
        companyName: [rules.required(t('auth.validation.companyRequired'))],
        subdomain: [
            rules.required(t('auth.validation.subdomainRequired')),
            rules.minLength(3, t('auth.validation.subdomainMin')),
            rules.pattern(/^[a-z0-9-]+$/, t('auth.validation.subdomainInvalid')),
        ],
        adminFullName: [rules.required(t('auth.validation.fullNameRequired'))],
        adminEmail: [
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
            const res = await authApi.register(form)
            setUser(res.data)
            navigate('/dashboard')
        } catch {
            setServerError(t('auth.registerFailed'))
        } finally {
            setLoading(false)
        }
    }

    return (
        <div className="auth-page">
            <div className="auth-mesh">
                <div className="auth-mesh__orb auth-mesh__orb--1" />
                <div className="auth-mesh__orb auth-mesh__orb--2" />
                <div className="auth-mesh__orb auth-mesh__orb--3" />
                <div className="auth-card-container">
                    <div className="auth-card auth-card--wide">
                        <div className="auth-card__top">
                            <div className="auth-brand">
                                <div className="auth-brand__logo">D</div>
                                <span className="auth-brand__name">DeskFlow</span>
                            </div>
                            <button
                                className="auth-lang"
                                onClick={() => i18n.changeLanguage(isRtl ? 'en' : 'ar')}
                            >
                                {isRtl ? 'English' : 'العربية'}
                            </button>
                        </div>

                        <div className="auth-card__header">
                            <h1>{t('auth.registerTitle')}</h1>
                            <p>{t('auth.registerSubtitle')}</p>
                        </div>

                        <form onSubmit={handleSubmit} className="auth-form auth-form--grid" noValidate>
                            <div className="auth-form__full">
                                <Input
                                    label={t('auth.companyName')}
                                    placeholder="Acme Corp"
                                    value={form.companyName}
                                    onChange={e => { setForm(f => ({ ...f, companyName: e.target.value })); clearError('companyName') }}
                                    icon={<Building2 size={15} />}
                                    error={errors.companyName}
                                />
                            </div>
                            <div className="auth-form__full">
                                <Input
                                    label={t('auth.subdomain')}
                                    placeholder="acme"
                                    value={form.subdomain}
                                    onChange={e => {
                                        setForm(f => ({ ...f, subdomain: e.target.value.toLowerCase().replace(/[^a-z0-9-]/g, '') }))
                                        clearError('subdomain')
                                    }}
                                    icon={<Globe size={15} />}
                                    error={errors.subdomain}
                                />
                            </div>
                            <Input
                                label={t('auth.fullName')}
                                placeholder="Alice Johnson"
                                value={form.adminFullName}
                                onChange={e => { setForm(f => ({ ...f, adminFullName: e.target.value })); clearError('adminFullName') }}
                                icon={<User size={15} />}
                                error={errors.adminFullName}
                            />
                            <Input
                                label={t('auth.email')}
                                type="email"
                                placeholder="alice@acme.com"
                                value={form.adminEmail}
                                onChange={e => { setForm(f => ({ ...f, adminEmail: e.target.value })); clearError('adminEmail') }}
                                icon={<Mail size={15} />}
                                error={errors.adminEmail}
                            />
                            <div className="auth-form__full">
                                <Input
                                    label={t('auth.password')}
                                    type="password"
                                    placeholder="••••••••"
                                    value={form.password}
                                    onChange={e => { setForm(f => ({ ...f, password: e.target.value })); clearError('password') }}
                                    icon={<Lock size={15} />}
                                    error={errors.password}
                                />
                            </div>

                            {serverError && <div className="auth-error auth-form__full">{serverError}</div>}

                            <div className="auth-form__full">
                                <Button type="submit" loading={loading} size="lg" style={{ width: '100%' }}>
                                    {t('auth.register')}
                                </Button>
                            </div>
                        </form>

                        <p className="auth-switch">
                            {t('auth.alreadyHaveAccount')}{' '}
                            <Link to="/login">{t('auth.signIn')}</Link>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    )
}