import {useQuery, useMutation, useQueryClient} from '@tanstack/react-query'
import {tvShowApi} from '../api/tvShowApi'
import type {
    UpdateTvShowDto,
    CreateSeasonDto,
    UpdateSeasonDto,
    CreateEpisodeDto,
    UpdateEpisodeDto,
    MarkWatchedEpisodeDto
} from '../types/tvshow'

export const useTvShow = (id: string) => {
    return useQuery({
        queryKey: ['tvshows', id],
        queryFn: () => tvShowApi.getById(id)
    })
}

export const useCreateTvShow = () => {
    const queryClient = useQueryClient()
    return useMutation({
        mutationFn: tvShowApi.create,
        onSuccess: () => queryClient.invalidateQueries({queryKey: ['media']}),
    })
}

export const useUpdateTvShow = () => {
    const queryClient = useQueryClient()
    return useMutation({
        mutationFn: ({id, dto}: { id: string; dto: UpdateTvShowDto }) =>
            tvShowApi.update(id, dto),
        onSuccess: (_, {id}) => {
            void queryClient.invalidateQueries({queryKey: ['tvshows', id]})
            void queryClient.invalidateQueries({queryKey: ['media']})
        },
    })
}

export const useAddSeason = (tvShowId: string) => {
    const queryClient = useQueryClient()
    return useMutation({
        mutationFn: (dto: CreateSeasonDto) => tvShowApi.addSeason(tvShowId, dto),
        onSuccess: () => queryClient.invalidateQueries({queryKey: ['tvshows', tvShowId]}),
    })
}

export const useUpdateSeason = (tvShowId: string) => {
    const queryClient = useQueryClient()
    return useMutation({
        mutationFn: ({seasonId, dto}: { seasonId: string; dto: UpdateSeasonDto }) =>
            tvShowApi.updateSeason(tvShowId, seasonId, dto),
        onSuccess: () => queryClient.invalidateQueries({queryKey: ['tvshows', tvShowId]}),
    })
}

export const useDeleteSeason = (tvShowId: string) => {
    const queryClient = useQueryClient()
    return useMutation({
        mutationFn: (seasonId: string) => tvShowApi.deleteSeason(tvShowId, seasonId),
        onSuccess: () => queryClient.invalidateQueries({queryKey: ['tvshows', tvShowId]}),
    })
}

export const useAddEpisode = (tvShowId: string) => {
    const queryClient = useQueryClient()
    return useMutation({
        mutationFn: ({seasonId, dto}: { seasonId: string; dto: CreateEpisodeDto }) =>
            tvShowApi.addEpisode(tvShowId, seasonId, dto),
        onSuccess: () => {
            void queryClient.invalidateQueries({queryKey: ['tvshows', tvShowId]})
            void queryClient.invalidateQueries({queryKey: ['media']})
        },
    })
}

export const useUpdateEpisode = (tvShowId: string) => {
    const queryClient = useQueryClient()
    return useMutation({
        mutationFn: ({seasonId, episodeId, dto}: { seasonId: string; episodeId: string; dto: UpdateEpisodeDto }) =>
            tvShowApi.updateEpisode(tvShowId, seasonId, episodeId, dto),
        onSuccess: () => queryClient.invalidateQueries({queryKey: ['tvshows', tvShowId]}),
    })
}

export const useDeleteEpisode = (tvShowId: string) => {
    const queryClient = useQueryClient()
    return useMutation({
        mutationFn: ({seasonId, episodeId}: { seasonId: string; episodeId: string }) =>
            tvShowApi.deleteEpisode(tvShowId, seasonId, episodeId),
        onSuccess: () => {
            void queryClient.invalidateQueries({queryKey: ['tvshows', tvShowId]})
            void queryClient.invalidateQueries({queryKey: ['media']})
        },
    })
}

export const useMarkWatchedEpisode = (tvShowId: string) => {
    const queryClient = useQueryClient()
    return useMutation({
        mutationFn: ({seasonId, episodeId, dto}: { seasonId: string; episodeId: string; dto: MarkWatchedEpisodeDto }) =>
            tvShowApi.markWatchedEpisode(tvShowId, seasonId, episodeId, dto),
        onSuccess: () => {
            void queryClient.invalidateQueries({queryKey: ['tvshows', tvShowId]})
            void queryClient.invalidateQueries({queryKey: ['media']})
        },
    })
}