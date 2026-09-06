export interface TmdbMovieSearchDto {
    tmdbId: number;
    title: string;
    releaseYear: number | null;
}

export interface TmdbTvShowSearchDto {
    tmdbId: number;
    name: string;
}

export interface TmdbSeasonInfoDto {
    seasonNumber: number;
    episodeCount: number;
    releaseYear: number | null;
}

export interface TmdbTvShowSeasonsDto {
    tmdbId: number;
    name: string;
    seasons: TmdbSeasonInfoDto[];
}