import client from './client'
import type {TmdbMovieSearchDto, TmdbTvShowSearchDto} from '../types/tmdb'

export const tmdbApi = {
    isConfigured: async (): Promise<boolean> => {
        const response = await client.get<boolean>('/api/tmdb/status')
        return response.data
    },

    searchMovies: async (query: string): Promise<TmdbMovieSearchDto[]> => {
        const response = await client.get<TmdbMovieSearchDto[]>('/api/tmdb/movies/search', {
            params: {query}
        })
        return response.data
    },

    searchTvShows: async (query: string): Promise<TmdbTvShowSearchDto[]> => {
        const response = await client.get<TmdbTvShowSearchDto[]>('/api/tmdb/tvshows/search', {
            params: {query}
        })
        return response.data
    },
}