export interface MovieFormData {
    title: string;
    userRating: number | null;
    nextMovieId: string | null;
    isWatched: boolean;
    tmdbId: number | null;
    releaseYear: number | null;
}

export interface TvShowFormData {
    title: string;
    userRating: number | null;
    tmdbId: number | null;
    releaseYear: number | null;
}