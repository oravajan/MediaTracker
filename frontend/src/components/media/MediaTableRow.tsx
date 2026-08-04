import {useNavigate} from 'react-router-dom'
import type {MediaSummaryDto} from '../../types/media'

interface Props {
    media: MediaSummaryDto
    onDelete: (media: MediaSummaryDto) => void
}

export default function MediaTableRow({media, onDelete}: Props) {
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
            <td className="px-4 py-3 text-right">
                <button
                    onClick={e => {
                        e.stopPropagation();
                        onDelete(media)
                    }}
                    className="text-xs text-muted px-3 py-1.5 rounded border border-transparent hover:border-danger hover:text-danger transition-colors duration-150"
                >
                    Delete
                </button>
            </td>
        </tr>
    )
}