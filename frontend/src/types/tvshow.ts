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
}

export interface CreateSeasonDto {
    seasonNumber: number;
}

export interface UpdateSeasonDto {
    seasonNumber: number;
}

export interface TvShowDto {
    id: string;
    title: string;
    userRating: number | null;
    seasons: SeasonDto[];
}

export interface CreateTvShowDto {
    title: string;
    userRating: number | null;
}

export interface UpdateTvShowDto {
    title: string;
    userRating: number | null;
}
