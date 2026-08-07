import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import {QueryClient, QueryClientProvider} from '@tanstack/react-query'
import './index.css'
import App from './App.tsx'
import {Toaster} from 'react-hot-toast'

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            retry: 1,
            staleTime: 1000 * 30,
        },
    },
})

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <QueryClientProvider client={queryClient}>
            <App/>
            <Toaster
                position="top-center"
                toastOptions={{
                    style: {
                        background: '#1A1D27',
                        color: '#E8E8E8',
                        border: '1px solid #2A2D3A',
                        fontSize: '0.875rem',
                    },
                    error: {
                        iconTheme: {
                            primary: '#E25555',
                            secondary: '#1A1D27',
                        },
                    },
                    success: {
                        iconTheme: {
                            primary: '#E2B94B',
                            secondary: '#1A1D27',
                        },
                    },
                }}
            />
        </QueryClientProvider>
    </StrictMode>
)