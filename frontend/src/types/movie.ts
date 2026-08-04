export interface MovieDto {
    id: string;
    title: string;
    userRating: number | null;
    nextMovieId: string | null;
    nextMovieTitle: string | null;
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
}