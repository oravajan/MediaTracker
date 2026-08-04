import {useNavigate} from 'react-router-dom'
import {useCreateTvShow} from '../hooks/useTvShow'
import TvShowForm from '../components/tvshow/TvShowForm'

export default function TvShowCreatePage() {
    const navigate = useNavigate()
    const {mutate: createTvShow, isPending} = useCreateTvShow()

    return (
        <div className="max-w-5xl mx-auto px-6 py-10">
            <button
                onClick={() => navigate('/')}
                className="text-sm text-muted hover:text-surface transition-colors mb-8 block"
            >
                ← Back
            </button>

            <p className="text-xs text-muted uppercase tracking-widest mb-2">📺 TV Show</p>

            <TvShowForm
                initialData={{title: '', userRating: null}}
                onSave={dto => createTvShow(dto, {
                    onSuccess: () => navigate('/')
                })}
                isSaving={isPending}
                title="Add TV Show"
            />
        </div>
    )
}