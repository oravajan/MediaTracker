export interface TmdbMovieSearchDto {
    tmdbId: number;
    title: string;
    releaseYear: number | null;
}

export interface TmdbTvShowSearchDto {
    tmdbId: number;
    title: string;
    releaseYear: number | null;
}