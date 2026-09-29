import {useState} from 'react'
import {useUpdateSeason, useDeleteSeason} from '../../hooks/useTvShow'
import ConfirmDialog from '../ui/ConfirmDialog'
import EpisodeTable from './EpisodeTable'
import type {SeasonDto} from '../../types/tvshow'

interface Props {
    tvShowId: string
    season: SeasonDto
}

export default function SeasonItem({tvShowId, season}: Props) {
    const [isEditing, setIsEditing] = useState(false)
    const [seasonNumber, setSeasonNumber] = useState(String(season.seasonNumber))
    const [showDeleteConfirm, setShowDeleteConfirm] = useState(false)
    const [releaseYear, setReleaseYear] = useState(season.releaseYear?.toString() ?? '')

    const {mutate: updateSeason} = useUpdateSeason(tvShowId)
    const {mutate: deleteSeason} = useDeleteSeason(tvShowId)

    const handleUpdate = () => {
        if (seasonNumber === '') return
        updateSeason(
            {
                seasonId: season.id,
                dto: {seasonNumber: Number(seasonNumber), releaseYear: releaseYear ? Number(releaseYear) : null}
            },
            {onSuccess: () => setIsEditing(false)}
        )
    }

    return (
        <div className="border border-border rounded-xl overflow-hidden mb-4">
            <div className="flex justify-between items-center px-4 py-3 bg-card">
                {isEditing ? (
                    <div className="flex gap-2 items-center">
                        <input
                            type="number"
                            className="bg-base border border-border rounded-lg px-3 py-1.5 text-surface text-sm outline-none focus:border-accent w-20"
                            value={seasonNumber}
                            onChange={e => setSeasonNumber(e.target.value)}
                        />
                        <input
                            type="number"
                            className="bg-card border border-border rounded-lg px-3 py-1.5 text-surface text-sm outline-none focus:border-accent w-24"
                            value={releaseYear}
                            onChange={e => setReleaseYear(e.target.value)}
                            placeholder="Year"
                        />
                        <button
                            onClick={handleUpdate}
                            className="px-3 py-1.5 text-sm font-semibold bg-accent text-base rounded-lg hover:bg-accent-dim transition-colors"
                        >
                            Save
                        </button>
                        <button
                            onClick={() => setIsEditing(false)}
                            className="px-3 py-1.5 text-sm text-muted border border-border rounded-lg hover:text-surface transition-colors"
                        >
                            Cancel
                        </button>
                    </div>
                ) : (
                    <>
                        <div className="flex items-baseline gap-2">
                            <span className="font-semibold text-sm">Season {season.seasonNumber}</span>
                            {season.releaseYear && <span className="text-xs text-muted">{season.releaseYear}</span>}
                        </div>
                        <div className="flex gap-1">
                            <button
                                onClick={() => {
                                    setSeasonNumber(String(season.seasonNumber))
                                    setReleaseYear(season.releaseYear?.toString() ?? '')
                                    setIsEditing(true)
                                }}
                                className="text-xs text-muted px-2.5 py-1 rounded border border-transparent hover:border-border hover:text-surface transition-colors"
                            >
                                Edit
                            </button>
                            <button
                                onClick={() => setShowDeleteConfirm(true)}
                                className="text-xs text-muted px-2.5 py-1 rounded border border-transparent hover:border-danger hover:text-danger transition-colors"
                            >
                                Delete
                            </button>
                        </div>
                    </>
                )}
            </div>

            <EpisodeTable tvShowId={tvShowId} season={season}/>

            {showDeleteConfirm && (
                <ConfirmDialog
                    message={`Delete Season ${season.seasonNumber} and all its episodes?`}
                    onConfirm={() => {
                        deleteSeason(season.id);
                        setShowDeleteConfirm(false)
                    }}
                    onCancel={() => setShowDeleteConfirm(false)}
                />
            )}
        </div>
    )
}