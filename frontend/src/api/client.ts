import axios from 'axios'

const client = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
})

client.interceptors.response.use(
    response => response,
    error => {
        const message = error.response?.data?.error ?? 'An unexpected error occurred.'
        return Promise.reject(new Error(message))
    }
)

export default client