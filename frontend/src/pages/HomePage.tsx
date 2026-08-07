import {useState} from 'react'
import {useNavigate} from 'react-router-dom'
import {useMedia, useDeleteMedia, useWatchMedia} from '../hooks/useMedia'
import MediaTable from '../components/media/MediaTable'
import ConfirmDialog from '../components/ui/ConfirmDialog'
import type {MediaSummaryDto} from '../types/media'

type FilterType = 'All' | 'Movie' | 'TvShow'

const FILTERS: { value: FilterType; label: string }[] = [
    {value: 'All', label: 'All'},
    {value: 'Movie', label: 'Movies'},
    {value: 'TvShow', label: 'TV Shows'},
]

export default function HomePage() {
    const navigate = useNavigate()
    const [activeFilter, setActiveFilter] = useState<FilterType>('All')
    const [pendingDelete, setPendingDelete] = useState<MediaSummaryDto | null>(null)

    const {data: media, isLoading, isError} = useMedia()
    const {mutate: deleteMedia} = useDeleteMedia()
    const {mutate: watchMedia} = useWatchMedia()

    const filtered = media?.filter(m =>
        activeFilter === 'All' ? true : m.type === activeFilter
    ) ?? []

    const handleConfirmDelete = () => {
        if (!pendingDelete) return
        deleteMedia(pendingDelete.id)
        setPendingDelete(null)
    }

    if (isLoading) return (
        <div className="flex items-center justify-center min-h-screen text-muted">
            Loading...
        </div>
    )

    if (isError) return (
        <div className="flex items-center justify-center min-h-screen text-danger">
            Failed to load media.
        </div>
    )

    return (
        <div className="max-w-5xl mx-auto px-6 py-10">
            <header className="flex justify-between items-center mb-8">
                <h1 className="text-3xl font-bold tracking-tight text-accent">
                    MediaTracker
                </h1>
                <div className="flex gap-3">
                    <button
                        onClick={() => navigate('/movies/new')}
                        className="px-4 py-2 bg-accent text-base rounded-lg text-sm font-semibold hover:bg-accent-dim transition-colors"
                    >
                        + Movie
                    </button>
                    <button
                        onClick={() => navigate('/tvshows/new')}
                        className="px-4 py-2 bg-accent text-base rounded-lg text-sm font-semibold hover:bg-accent-dim transition-colors"
                    >
                        + TV Show
                    </button>
                </div>
            </header>

            <div className="flex gap-2 mb-6">
                {FILTERS.map(f => (
                    <button
                        key={f.value}
                        onClick={() => setActiveFilter(f.value)}
                        className={`px-4 py-1.5 rounded-full text-sm border transition-all duration-150 ${
                            activeFilter === f.value
                                ? 'bg-accent text-base border-accent font-semibold'
                                : 'text-muted border-border hover:border-accent hover:text-surface'
                        }`}
                    >
                        {f.label}
                    </button>
                ))}
            </div>

            {filtered.length === 0 ? (
                <p className="text-center py-16 text-muted">
                    No entries yet. Add your first movie or TV show.
                </p>
            ) : (
                <MediaTable items={filtered} onDelete={setPendingDelete} onWatch={m => watchMedia(m.id)}/>
            )}

            {pendingDelete && (
                <ConfirmDialog
                    message={`Delete "${pendingDelete.title}"?`}
                    onConfirm={handleConfirmDelete}
                    onCancel={() => setPendingDelete(null)}
                />
            )}
        </div>
    )
}