export interface MovieDto {
    id: string;
    title: string;
    userRating: number | null;
    nextMovieId: string | null;
    isWatched: boolean;
    tmdbId: number | null;
    releaseYear: number | null;
}

export interface CreateMovieDto {
    title: string;
    userRating: number | null;
    nextMovieId: string | null;
    tmdbId: number | null;
    releaseYear: number | null;
}

export interface UpdateMovieDto {
    title: string;
    userRating: number | null;
    nextMovieId: string | null;
    isWatched: boolean;
    tmdbId: number | null;
    releaseYear: number | null;
}

export interface MarkWatchedMovieDto {
    isWatched: boolean;
}