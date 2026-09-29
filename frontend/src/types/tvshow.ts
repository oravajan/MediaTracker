export interface EpisodeDto {
    id: string;
    episodeNumber: number;
    title: string | null;
    isWatched: boolean;
}

export interface CreateEpisodeDto {
    episodeNumber: number;
    title: string | null;
    isWatched: boolean;
}

export interface UpdateEpisodeDto {
    episodeNumber: number;
    title: string | null;
    isWatched: boolean;
}

export interface MarkWatchedEpisodeDto {
    isWatched: boolean;
}

export interface SeasonDto {
    id: string;
    seasonNumber: number;
    episodes: EpisodeDto[];
    releaseYear: number | null;
}

export interface CreateSeasonDto {
    seasonNumber: number;
    releaseYear: number | null;
}

export interface UpdateSeasonDto {
    seasonNumber: number;
    releaseYear: number | null;
}

export interface TvShowDto {
    id: string;
    title: string;
    userRating: number | null;
    seasons: SeasonDto[];
    tmdbId: number | null;
    releaseYear: number | null;
}

export interface CreateTvShowDto {
    title: string;
    userRating: number | null;
    tmdbId: number | null;
}

export interface UpdateTvShowDto {
    title: string;
    userRating: number | null;
    tmdbId: number | null;
}
