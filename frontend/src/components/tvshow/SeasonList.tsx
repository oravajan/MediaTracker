import {useState} from 'react'
import {useAddSeason} from '../../hooks/useTvShow'
import SeasonItem from './SeasonItem'
import type {SeasonDto} from '../../types/tvshow'

interface Props {
    tvShowId: string
    seasons: SeasonDto[]
}

export default function SeasonList({tvShowId, seasons}: Props) {
    const [showAddForm, setShowAddForm] = useState(false)
    const [seasonNumber, setSeasonNumber] = useState('')

    const {mutate: addSeason} = useAddSeason(tvShowId)

    const nextSeasonNumber = seasons.length === 0
        ? 1
        : Math.max(...seasons.map(s => s.seasonNumber)) + 1

    const handleOpenAdd = () => {
        setSeasonNumber(String(nextSeasonNumber))
        setShowAddForm(true)
    }

    const handleAdd = () => {
        const num = Number(seasonNumber)
        if (!num) return
        addSeason(
            {seasonNumber: num},
            {onSuccess: () => setShowAddForm(false)}
        )
    }

    return (
        <div className="mt-8 pt-8 border-t border-border">
            <h2 className="text-xs text-muted uppercase tracking-widest mb-4">Seasons</h2>

            {seasons.length === 0 && !showAddForm && (
                <p className="text-muted text-sm mb-4">No seasons yet.</p>
            )}

            {seasons.map(season => (
                <SeasonItem key={season.id} tvShowId={tvShowId} season={season}/>
            ))}

            {showAddForm ? (
                <div className="flex gap-2 items-center mt-2">
                    <input
                        type="number"
                        className="bg-card border border-border rounded-lg px-3 py-1.5 text-surface text-sm outline-none focus:border-accent w-24"
                        value={seasonNumber}
                        onChange={e => setSeasonNumber(e.target.value)}
                        placeholder="Season #"
                    />
                    <button
                        onClick={handleAdd}
                        className="px-4 py-1.5 text-sm font-semibold bg-accent text-base rounded-lg hover:bg-accent-dim transition-colors"
                    >
                        Add Season
                    </button>
                    <button
                        onClick={() => setShowAddForm(false)}
                        className="px-4 py-1.5 text-sm text-muted border border-border rounded-lg hover:text-surface transition-colors"
                    >
                        Cancel
                    </button>
                </div>
            ) : (
                <button
                    onClick={handleOpenAdd}
                    className="text-sm text-muted hover:text-accent border border-border hover:border-accent rounded-lg px-4 py-2 transition-colors mt-2"
                >
                    + Add Season
                </button>
            )}
        </div>
    )
}