import {useParams, useNavigate} from 'react-router-dom'
import {useMovie, useUpdateMovie} from '../hooks/useMovie'
import MovieForm from '../components/movie/MovieForm'

export default function MovieDetailPage() {
    const {id} = useParams<{ id: string }>()
    const navigate = useNavigate()

    const {data: movie, isLoading} = useMovie(id!)
    const {mutate: updateMovie, isPending} = useUpdateMovie()

    if (isLoading) return (
        <div className="flex items-center justify-center min-h-screen text-muted">
            Loading...
        </div>
    )

    if (!movie) return (
        <div className="flex items-center justify-center min-h-screen text-danger">
            Movie not found.
        </div>
    )

    return (
        <div className="max-w-5xl mx-auto px-6 py-10">
            <button
                onClick={() => navigate('/')}
                className="text-sm text-muted hover:text-surface transition-colors mb-8 block"
            >
                ← Back
            </button>

            <p className="text-xs text-muted uppercase tracking-widest mb-2">🎬 Movie</p>

            <MovieForm
                key={movie.id}
                initialData={{
                    title: movie.title,
                    userRating: movie.userRating,
                    nextMovieId: movie.nextMovieId
                }}
                excludeId={movie.id}
                onSave={dto => updateMovie({id: movie.id, dto}, {
                    onSuccess: () => navigate('/')
                })}
                isSaving={isPending}
                title={movie.title}
            />
        </div>
    )
}