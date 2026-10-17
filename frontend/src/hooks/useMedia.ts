import {useQuery, useMutation, useQueryClient} from '@tanstack/react-query'
import {mediaApi} from '../api/mediaApi'

export const useMedia = () => {
    return useQuery({
        queryKey: ['media'],
        queryFn: mediaApi.getAll,
    })
}

export const useDeleteMedia = () => {
    const queryClient = useQueryClient()

    return useMutation({
        mutationFn: mediaApi.deleteById,
        onSuccess: () => queryClient.invalidateQueries({queryKey: ['media']}),
    })
}

export const useWatchMedia = () => {
    const queryClient = useQueryClient()
    return useMutation({
        mutationFn: mediaApi.watch,
        onSuccess: (media) => {
            const detailKey = media.type === 'Movie' ? 'movies' : 'tvshows'
            queryClient.removeQueries({queryKey: [detailKey, media.id]})
            void queryClient.invalidateQueries({queryKey: ['media']})
        },
    })
}