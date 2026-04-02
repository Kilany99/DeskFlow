import { useState } from 'react'

type Rules<T> = {
    [K in keyof T]?: ((value: string) => string | undefined)[]
}

export function useFormValidation<T extends Record<string, string>>(
    form: T,
    rules: Rules<T>
) {
    const [errors, setErrors] = useState<Partial<Record<keyof T, string>>>({})

    const validate = (): boolean => {
        const newErrors: Partial<Record<keyof T, string>> = {}
        for (const field in rules) {
            const fieldRules = rules[field] ?? []
            for (const rule of fieldRules) {
                const error = rule(form[field] ?? '')
                if (error) { newErrors[field] = error; break }
            }
        }
        setErrors(newErrors)
        return Object.keys(newErrors).length === 0
    }

    const clearError = (field: keyof T) =>
        setErrors(e => ({ ...e, [field]: undefined }))

    return { errors, validate, clearError }
}

// Validation rules
export const rules = {
    required: (msg: string) => (v: string) => !v.trim() ? msg : undefined,
    email: (msg: string) => (v: string) =>
        !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v) ? msg : undefined,
    minLength: (n: number, msg: string) => (v: string) =>
        v.length < n ? msg : undefined,
    pattern: (regex: RegExp, msg: string) => (v: string) =>
        !regex.test(v) ? msg : undefined,
}