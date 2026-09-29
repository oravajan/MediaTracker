import {useState} from 'react'
import {useParams, useNavigate} from 'react-router-dom'
import {useTvShow, useUpdateTvShow} from '../hooks/useTvShow'
import {useSyncTvShow} from '../hooks/useTvShow'
import TvShowForm from '../components/tvshow/TvShowForm'
import SeasonList from '../components/tvshow/SeasonList'
import toast from "react-hot-toast";
import ConfirmDialog from "../components/ui/ConfirmDialog.tsx";

export default function TvShowDetailPage() {
    const {id} = useParams<{ id: string }>()
    const navigate = useNavigate()

    const {data: tvShow, isLoading} = useTvShow(id!)
    const {mutate: updateTvShow, isPending} = useUpdateTvShow()
    const [showSyncConfirm, setShowSyncConfirm] = useState(false)
    const {mutate: syncTvShow, isPending: isSyncing} = useSyncTvShow(id!)

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
            <div className="flex justify-between items-center mb-8">
                <button
                    onClick={() => navigate('/')}
                    className="text-sm text-muted hover:text-surface transition-colors"
                >
                    ← Back
                </button>

                <button
                    onClick={() => setShowSyncConfirm(true)}
                    disabled={isSyncing || !tvShow.tmdbId}
                    className="text-sm text-muted border border-border rounded-lg px-4 py-2 hover:border-accent hover:text-accent transition-colors disabled:opacity-40 disabled:cursor-not-allowed"
                >
                    {isSyncing ? 'Syncing...' : 'Sync with TMDB'}
                </button>
            </div>

            <p className="text-xs text-muted uppercase tracking-widest mb-2">📺 TV Show</p>

            <TvShowForm
                key={tvShow.id}
                initialData={{
                    title: tvShow.title,
                    userRating: tvShow.userRating,
                    tmdbId: tvShow.tmdbId,
                    releaseYear: tvShow.releaseYear
                }}
                onSave={data => updateTvShow({
                    id: tvShow.id, dto: {
                        title: data.title,
                        userRating: data.userRating,
                        tmdbId: data.tmdbId,
                    }
                }, {
                    onSuccess: () => {
                        toast.success('TV Show saved successfully.')
                        navigate('/')
                    }
                })}
                isSaving={isPending}
                title={tvShow.title}
            />

            <SeasonList tvShowId={tvShow.id} seasons={tvShow.seasons}/>

            {showSyncConfirm && (
                <ConfirmDialog
                    message="This will reset all seasons and episodes. Watched progress will be lost. Continue?"
                    onConfirm={() => {
                        syncTvShow(undefined, {
                            onSuccess: () => {
                                toast.success('Synced with TMDB successfully.')
                                setShowSyncConfirm(false)
                            },
                            onError: () => {
                                setShowSyncConfirm(false)
                            }
                        })
                    }}
                    onCancel={() => setShowSyncConfirm(false)}
                />
            )}
        </div>
    )
}