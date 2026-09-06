import {useQuery} from '@tanstack/react-query'
import {tmdbApi} from '../api/tmdbApi'
import {useEffect, useState} from 'react'

export const useTmdbStatus = () => {
    return useQuery({
        queryKey: ['tmdb-status'],
        queryFn: tmdbApi.isConfigured,
        staleTime: Infinity,  // nepotřebuje se refetchovat
    })
}

const useDebounce = (value: string, delay: number) => {
    const [debouncedValue, setDebouncedValue] = useState(value)

    useEffect(() => {
        const timeout = setTimeout(() => setDebouncedValue(value), delay)
        return () => clearTimeout(timeout)
    }, [value, delay])

    return debouncedValue
}

export const useTmdbMovieSearch = (query: string, enabled: boolean) => {
    const debouncedQuery = useDebounce(query, 500)

    return useQuery({
        queryKey: ['tmdb-movies', debouncedQuery],
        queryFn: () => tmdbApi.searchMovies(debouncedQuery),
        enabled: enabled && debouncedQuery.trim().length >= 2,
        staleTime: 1000 * 60 * 5,
    })
}

export const useTmdbTvShowSearch = (query: string, enabled: boolean) => {
    const debouncedQuery = useDebounce(query, 500)

    return useQuery({
        queryKey: ['tmdb-tvshows', debouncedQuery],
        queryFn: () => tmdbApi.searchTvShows(debouncedQuery),
        enabled: enabled && debouncedQuery.trim().length >= 2,
        staleTime: 1000 * 60 * 5,
    })
}