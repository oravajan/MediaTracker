import {useQuery, useMutation, useQueryClient} from '@tanstack/react-query'
import {movieApi} from '../api/movieApi'
import type {UpdateMovieDto} from '../types/movie'

export const useMovie = (id: string) => {
    return useQuery({
        queryKey: ['movies', id],
        queryFn: () => movieApi.getById(id)
    })
}

export const useCreateMovie = () => {
    const queryClient = useQueryClient()
    return useMutation({
        mutationFn: movieApi.create,
        onSuccess: () => queryClient.invalidateQueries({queryKey: ['media']}),
    })
}

export const useUpdateMovie = () => {
    const queryClient = useQueryClient()
    return useMutation({
        mutationFn: ({id, dto}: { id: string; dto: UpdateMovieDto }) =>
            movieApi.update(id, dto),
        onSuccess: (_, {id}) => {
            queryClient.invalidateQueries({queryKey: ['movies', id]})
            queryClient.invalidateQueries({queryKey: ['media']})
        },
    })
}