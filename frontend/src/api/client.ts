import axios from 'axios'
import toast from 'react-hot-toast'

const client = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
})

client.interceptors.response.use(
    response => response,
    error => {
        const message = error.response?.data?.error
            ?? error.response?.data?.title
            ?? 'An unexpected error occurred.'

        toast.error(message)

        return Promise.reject(new Error(message))
    }
)

export default client