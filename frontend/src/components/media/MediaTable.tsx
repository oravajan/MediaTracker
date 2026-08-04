import type {MediaSummaryDto} from '../../types/media'
import MediaTableRow from './MediaTableRow'

interface Props {
    items: MediaSummaryDto[]
    onDelete: (media: MediaSummaryDto) => void
}

const COLUMNS = [
    {key: 'type', label: 'Type'},
    {key: 'title', label: 'Title'},
    {key: 'userRating', label: 'Rating'},
    {key: 'actions', label: ''},
] as const

export default function MediaTable({items, onDelete}: Props) {
    return (
        <table className="w-full border-collapse">
            <thead>
            <tr>
                {COLUMNS.map(col => (
                    <th
                        key={col.key}
                        className="text-left px-4 py-3 text-xs text-muted uppercase tracking-widest border-b border-border"
                    >
                        {col.label}
                    </th>
                ))}
            </tr>
            </thead>
            <tbody>
            {items.map(media => (
                <MediaTableRow
                    key={media.id}
                    media={media}
                    onDelete={onDelete}
                />
            ))}
            </tbody>
        </table>
    )
}