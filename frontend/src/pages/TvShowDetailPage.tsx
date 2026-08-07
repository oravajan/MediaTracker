import {useParams, useNavigate} from 'react-router-dom'
import {useTvShow, useUpdateTvShow} from '../hooks/useTvShow'
import TvShowForm from '../components/tvshow/TvShowForm'
import SeasonList from '../components/tvshow/SeasonList'
import toast from "react-hot-toast";

export default function TvShowDetailPage() {
    const {id} = useParams<{ id: string }>()
    const navigate = useNavigate()

    const {data: tvShow, isLoading} = useTvShow(id!)
    const {mutate: updateTvShow, isPending} = useUpdateTvShow()

    if (isLoading) return (
        <div className="flex items-center justify-center min-h-screen text-muted">
            Loading...
        </div>
    )

    if (!tvShow) return (
        <div className="flex items-center justify-center min-h-screen text-danger">
            TV Show not found.
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

            <p className="text-xs text-muted uppercase tracking-widest mb-2">📺 TV Show</p>

            <TvShowForm
                key={tvShow.id}
                initialData={{title: tvShow.title, userRating: tvShow.userRating}}
                onSave={dto => updateTvShow({id: tvShow.id, dto}, {
                    onSuccess: () => {
                        toast.success('TV Show saved successfully.')
                        navigate('/')
                    }
                })}
                isSaving={isPending}
                title={tvShow.title}
            />

            <SeasonList tvShowId={tvShow.id} seasons={tvShow.seasons}/>
        </div>
    )
}