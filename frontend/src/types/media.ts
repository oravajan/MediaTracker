export interface MediaSummaryDto {
    id: string;
    title: string;
    type: 'Movie' | 'TvShow';
    userRating: number | null;
}