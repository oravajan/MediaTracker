import {useNavigate} from 'react-router-dom'
import type {MediaSummaryDto} from '../../types/media'

interface Props {
    media: MediaSummaryDto
    onDelete: (media: MediaSummaryDto) => void
    onWatch: (media: MediaSummaryDto) => void
}

function WatchedStatus({media}: { media: MediaSummaryDto }) {
    if (media.type === 'Movie') {
        return media.isWatched
            ? <span className="text-accent text-sm">✓ Watched</span>
            : <span className="text-muted text-sm">—</span>
    }

    if (media.totalEpisodeCount === 0 || media.totalEpisodeCount === null) {
        return <span className="text-muted text-sm">—</span>
    }

    return (
        <span className={`text-sm tabular-nums ${media.isWatched ? 'text-accent' : 'text-muted'}`}>
            {media.watchedEpisodeCount}/{media.totalEpisodeCount}
        </span>
    )
}

export default function MediaTableRow({media, onDelete, onWatch}: Props) {
    const navigate = useNavigate()

    const handleRowClick = () => {
        if (media.type === 'Movie') navigate(`/movies/${media.id}`)
        else navigate(`/tvshows/${media.id}`)
    }

    return (
        <tr
            onClick={handleRowClick}
            className="border-b border-border cursor-pointer transition-all duration-150 hover:bg-card hover:translate-x-1"
        >
            <td className="px-4 py-3">
                <span className="text-xs text-muted uppercase tracking-widest whitespace-nowrap">
                    {media.type === 'Movie' ? '🎬 Movie' : '📺 TV Show'}
                </span>
            </td>
            <td className="px-4 py-3 font-medium text-surface w-full">
                {media.title}
            </td>
            <td className="px-4 py-3 whitespace-nowrap">
                {media.userRating !== null ? (
                    <span className="font-bold text-accent tabular-nums">
                        {media.userRating}
                        <span className="text-xs text-accent-dim font-normal">/10</span>
                    </span>
                ) : (
                    <span className="text-muted">—</span>
                )}
            </td>
            <td className="px-4 py-3 whitespace-nowrap">
                <WatchedStatus media={media}/>
            </td>
            <td className="px-4 py-3">
                <div className="flex gap-1 justify-end items-center">
                    <button
                        onClick={e => {
                            e.stopPropagation();
                            onWatch(media)
                        }}
                        className={`text-xs px-2.5 py-1 rounded border transition-colors whitespace-nowrap ${
                            media.isWatched
                                ? 'invisible'
                                : 'text-muted border-transparent hover:border-accent hover:text-accent'
                        }`}
                    >
                        Watch
                    </button>
                    <button
                        onClick={e => {
                            e.stopPropagation();
                            onDelete(media)
                        }}
                        className="text-xs text-muted px-2.5 py-1 rounded border border-transparent hover:border-danger hover:text-danger transition-colors whitespace-nowrap"
                    >
                        Delete
                    </button>
                </div>
            </td>
        </tr>
    )
}