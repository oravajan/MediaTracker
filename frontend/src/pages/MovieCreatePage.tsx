import {useNavigate} from 'react-router-dom'
import {useCreateMovie} from '../hooks/useMovie'
import MovieForm from '../components/movie/MovieForm'
import toast from 'react-hot-toast'

export default function MovieCreatePage() {
    const navigate = useNavigate()
    const {mutate: createMovie, isPending} = useCreateMovie()

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
                initialData={{title: '', userRating: null, nextMovieId: null}}
                onSave={data => createMovie({
                    title: data.title,
                    userRating: data.userRating,
                    nextMovieId: data.nextMovieId
                }, {
                    onSuccess: () => {
                        toast.success('Movie saved successfully.')
                        navigate(`/`)
                    }
                })}
                isSaving={isPending}
                title="Add Movie"
            />
        </div>
    )
}