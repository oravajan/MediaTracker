export interface MovieDto {
    id: string;
    title: string;
    userRating: number | null;
    nextMovieId: string | null;
    isWatched: boolean;
}

export interface CreateMovieDto {
    title: string;
    userRating: number | null;
    nextMovieId: string | null;
}

export interface UpdateMovieDto {
    title: string;
    userRating: number | null;
    nextMovieId: string | null;
    isWatched: boolean;
}

export interface MarkWatchedMovieDto {
    isWatched: boolean;
}