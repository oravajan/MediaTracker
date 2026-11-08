import {useQuery, useMutation, useQueryClient} from '@tanstack/react-query'
import toast from 'react-hot-toast'
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

export const useStartMediaUpdate = () => {
    const queryClient = useQueryClient()
    return useMutation({
        mutationFn: mediaApi.startUpdate,
        onSuccess: () => queryClient.setQueryData(['media-update-status'], true),
    })
}

export const useMediaUpdateStatus = () => {
    const queryClient = useQueryClient()
    return useQuery({
        queryKey: ['media-update-status'],
        queryFn: async () => {
            const wasRunning = queryClient.getQueryData<boolean>(['media-update-status'])
            const isRunning = await mediaApi.getUpdateStatus()

            if (wasRunning && !isRunning) {
                queryClient.removeQueries({queryKey: ['movies']})
                void queryClient.invalidateQueries({queryKey: ['tvshows']})
                void queryClient.invalidateQueries({queryKey: ['media']})
                toast.success('Update finished.')
            }

            return isRunning
        },
        refetchInterval: query => (query.state.data ? 2000 : false),
        staleTime: 0,
    })
}