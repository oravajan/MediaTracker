import client from './client'
import type {MovieDto, CreateMovieDto, UpdateMovieDto, MarkWatchedMovieDto} from '../types/movie'

export const movieApi = {
    getById: async (id: string): Promise<MovieDto> => {
        const response = await client.get<MovieDto>(`/api/movies/${id}`)
        return response.data
    },

    create: async (dto: CreateMovieDto): Promise<MovieDto> => {
        const response = await client.post<MovieDto>('/api/movies', dto)
        return response.data
    },

    update: async (id: string, dto: UpdateMovieDto): Promise<MovieDto> => {
        const response = await client.put<MovieDto>(`/api/movies/${id}`, dto)
        return response.data
    },

    markWatched: async (movieId: string, dto: MarkWatchedMovieDto): Promise<MovieDto> => {
        const response = await client.patch<MovieDto>(
            `/api/movies/${movieId}/watched`, dto
        )
        return response.data
    },
}