import client from './client'
import type {MediaSummaryDto} from '../types/media'

export const mediaApi = {
    getAll: async (): Promise<MediaSummaryDto[]> => {
        const response = await client.get<MediaSummaryDto[]>('/api/media')
        return response.data
    },

    deleteById: async (id: string): Promise<void> => {
        await client.delete(`/api/media/${id}`)
    },

    watch: async (id: string): Promise<MediaSummaryDto> => {
        const response = await client.patch<MediaSummaryDto>(`/api/media/${id}/watch`)
        return response.data
    },

    startUpdate: async (): Promise<void> => {
        await client.post('/api/media/update')
    },

    getUpdateStatus: async (): Promise<boolean> => {
        const response = await client.get<boolean>('/api/media/update/status')
        return response.data
    },
}