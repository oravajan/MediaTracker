export interface TmdbMovieSearchDto {
    tmdbId: number;
    title: string;
    releaseYear: number | null;
}

export interface TmdbTvShowSearchDto {
    tmdbId: number;
    title: string;
}

export interface TmdbTvShowSeasonsDto {
    tmdbId: number;
    title: string;
    seasons: TmdbSeasonInfoDto[];
}

export interface TmdbSeasonInfoDto {
    seasonNumber: number;
    releaseYear: number | null;
}