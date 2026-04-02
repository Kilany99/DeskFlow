import axios from 'axios'
import { useAuthStore } from '../store/authStore'

const api = axios.create({
    baseURL: '/api',
    headers: { 'Content-Type': 'application/json' },
})

// Attach token to every request
api.interceptors.request.use((config) => {
    const token = useAuthStore.getState().user?.accessToken
    if (token) config.headers.Authorization = `Bearer ${token}`
    return config
})

// Handle 401 — clear auth and redirect to login
api.interceptors.response.use(
    (res) => res,
    (error) => {
        if (error.response?.status === 401) {
            useAuthStore.getState().clearUser()
            window.location.href = '/login'
        }
        return Promise.reject(error)
    }
)

export default api