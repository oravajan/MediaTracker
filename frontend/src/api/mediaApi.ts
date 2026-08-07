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
}