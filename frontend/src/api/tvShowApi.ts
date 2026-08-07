import client from './client'
import type {
    TvShowDto, CreateTvShowDto, UpdateTvShowDto,
    SeasonDto, CreateSeasonDto, UpdateSeasonDto,
    EpisodeDto, CreateEpisodeDto, UpdateEpisodeDto, MarkWatchedEpisodeDto
} from '../types/tvshow'

export const tvShowApi = {
    getById: async (id: string): Promise<TvShowDto> => {
        const response = await client.get<TvShowDto>(`/api/tvshows/${id}`)
        return response.data
    },

    create: async (dto: CreateTvShowDto): Promise<TvShowDto> => {
        const response = await client.post<TvShowDto>('/api/tvshows', dto)
        return response.data
    },

    update: async (id: string, dto: UpdateTvShowDto): Promise<TvShowDto> => {
        const response = await client.put<TvShowDto>(`/api/tvshows/${id}`, dto)
        return response.data
    },

    addSeason: async (tvShowId: string, dto: CreateSeasonDto): Promise<SeasonDto> => {
        const response = await client.post<SeasonDto>(`/api/tvshows/${tvShowId}/seasons`, dto)
        return response.data
    },

    updateSeason: async (tvShowId: string, seasonId: string, dto: UpdateSeasonDto): Promise<SeasonDto> => {
        const response = await client.put<SeasonDto>(`/api/tvshows/${tvShowId}/seasons/${seasonId}`, dto)
        return response.data
    },

    deleteSeason: async (tvShowId: string, seasonId: string): Promise<void> => {
        await client.delete(`/api/tvshows/${tvShowId}/seasons/${seasonId}`)
    },

    addEpisode: async (tvShowId: string, seasonId: string, dto: CreateEpisodeDto): Promise<EpisodeDto> => {
        const response = await client.post<EpisodeDto>(
            `/api/tvshows/${tvShowId}/seasons/${seasonId}/episodes`, dto
        )
        return response.data
    },

    updateEpisode: async (tvShowId: string, seasonId: string, episodeId: string, dto: UpdateEpisodeDto): Promise<EpisodeDto> => {
        const response = await client.put<EpisodeDto>(
            `/api/tvshows/${tvShowId}/seasons/${seasonId}/episodes/${episodeId}`, dto
        )
        return response.data
    },

    deleteEpisode: async (tvShowId: string, seasonId: string, episodeId: string): Promise<void> => {
        await client.delete(`/api/tvshows/${tvShowId}/seasons/${seasonId}/episodes/${episodeId}`)
    },

    markWatchedEpisode: async (tvShowId: string, seasonId: string, episodeId: string, dto: MarkWatchedEpisodeDto): Promise<EpisodeDto> => {
        const response = await client.patch<EpisodeDto>(
            `/api/tvshows/${tvShowId}/seasons/${seasonId}/episodes/${episodeId}/watched`, dto
        )
        return response.data
    },
}