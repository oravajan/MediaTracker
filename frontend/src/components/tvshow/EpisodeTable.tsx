import {useState} from 'react'
import {useAddEpisode, useUpdateEpisode, useDeleteEpisode, useMarkWatchedEpisode} from '../../hooks/useTvShow'
import ConfirmDialog from '../ui/ConfirmDialog'
import type {EpisodeDto, SeasonDto} from '../../types/tvshow'

interface Props {
    tvShowId: string
    season: SeasonDto
}

export default function EpisodeTable({tvShowId, season}: Props) {
    const [editingId, setEditingId] = useState<string | null>(null)
    const [editForm, setEditForm] = useState({episodeNumber: '', title: ''})
    const [showAddForm, setShowAddForm] = useState(false)
    const [addForm, setAddForm] = useState({episodeNumber: '', title: ''})
    const [deletingId, setDeletingId] = useState<string | null>(null)

    const {mutate: addEpisode} = useAddEpisode(tvShowId)
    const {mutate: updateEpisode} = useUpdateEpisode(tvShowId)
    const {mutate: deleteEpisode} = useDeleteEpisode(tvShowId)
    const {mutate: markWatchedEpisode} = useMarkWatchedEpisode(tvShowId)

    const nextEpisodeNumber = season.episodes.length === 0
        ? 1
        : Math.max(...season.episodes.map(e => e.episodeNumber)) + 1

    const handleStartAdd = () => {
        setAddForm({episodeNumber: String(nextEpisodeNumber), title: ''})
        setShowAddForm(true)
    }

    const handleAdd = () => {
        const num = Number(addForm.episodeNumber)
        if (!num) return
        addEpisode(
            {seasonId: season.id, dto: {episodeNumber: num, title: addForm.title || null, isWatched: false}},
            {
                onSuccess: () => {
                    setShowAddForm(false);
                    setAddForm({episodeNumber: '', title: ''})
                }
            }
        )
    }

    const handleStartEdit = (ep: EpisodeDto) => {
        setEditingId(ep.id)
        setEditForm({episodeNumber: String(ep.episodeNumber), title: ep.title ?? ''})
    }

    const handleUpdate = (ep: EpisodeDto) => {
        const num = Number(editForm.episodeNumber)
        if (!num) return
        updateEpisode(
            {
                seasonId: season.id,
                episodeId: ep.id,
                dto: {episodeNumber: num, title: editForm.title || null, isWatched: ep.isWatched}
            },
            {onSuccess: () => setEditingId(null)}
        )
    }

    const handleDelete = (episodeId: string) => {
        deleteEpisode(
            {seasonId: season.id, episodeId},
            {onSuccess: () => setDeletingId(null)}
        )
    }

    const deletingEpisode = season.episodes.find(e => e.id === deletingId)

    return (
        <div>
            <table className="w-full border-collapse">
                <thead>
                <tr>
                    <th className="text-left px-4 py-2 text-xs text-muted uppercase tracking-widest w-16">#</th>
                    <th className="text-left px-4 py-2 text-xs text-muted uppercase tracking-widest">Title</th>
                    <th className="text-left px-4 py-2 text-xs text-muted uppercase tracking-widest w-24">Watched</th>
                    <th className="w-28"></th>
                </tr>
                </thead>
                <tbody>
                {season.episodes.map(ep => (
                    <tr key={ep.id} className="border-t border-border">
                        {editingId === ep.id ? (
                            <td colSpan={4} className="px-4 py-2">
                                <div className="flex gap-2 items-center">
                                    <input
                                        type="number"
                                        className="bg-base border border-border rounded-lg px-3 py-1.5 text-surface text-sm outline-none focus:border-accent w-20"
                                        value={editForm.episodeNumber}
                                        onChange={e => setEditForm(f => ({...f, episodeNumber: e.target.value}))}
                                    />
                                    <input
                                        className="bg-base border border-border rounded-lg px-3 py-1.5 text-surface text-sm outline-none focus:border-accent flex-1"
                                        value={editForm.title}
                                        onChange={e => setEditForm(f => ({...f, title: e.target.value}))}
                                        placeholder="Title (optional)"
                                    />
                                    <button
                                        onClick={() => handleUpdate(ep)}
                                        className="px-3 py-1.5 text-sm font-semibold bg-accent text-base rounded-lg hover:bg-accent-dim transition-colors"
                                    >
                                        Save
                                    </button>
                                    <button
                                        onClick={() => setEditingId(null)}
                                        className="px-3 py-1.5 text-sm text-muted border border-border rounded-lg hover:text-surface transition-colors"
                                    >
                                        Cancel
                                    </button>
                                </div>
                            </td>
                        ) : (
                            <>
                                <td className="px-4 py-2.5 text-muted text-sm">E{ep.episodeNumber}</td>
                                <td className="px-4 py-2.5 text-surface text-sm">
                                    {ep.title ?? <span className="text-muted italic">Untitled</span>}
                                </td>
                                <td className="px-4 py-2.5">
                                    <input
                                        type="checkbox"
                                        checked={ep.isWatched}
                                        onChange={e => markWatchedEpisode({
                                            seasonId: season.id,
                                            episodeId: ep.id,
                                            dto: {isWatched: e.target.checked}
                                        })}
                                        className="w-4 h-4 accent-accent cursor-pointer"
                                    />
                                </td>
                                <td className="px-4 py-2.5">
                                    <div className="flex gap-1 justify-end">
                                        <button
                                            onClick={() => handleStartEdit(ep)}
                                            className="text-xs text-muted px-2.5 py-1 rounded border border-transparent hover:border-border hover:text-surface transition-colors"
                                        >
                                            Edit
                                        </button>
                                        <button
                                            onClick={() => setDeletingId(ep.id)}
                                            className="text-xs text-muted px-2.5 py-1 rounded border border-transparent hover:border-danger hover:text-danger transition-colors"
                                        >
                                            Delete
                                        </button>
                                    </div>
                                </td>
                            </>
                        )}
                    </tr>
                ))}

                {showAddForm ? (
                    <tr className="border-t border-border">
                        <td colSpan={4} className="px-4 py-2">
                            <div className="flex gap-2 items-center">
                                <input
                                    type="number"
                                    className="bg-base border border-border rounded-lg px-3 py-1.5 text-surface text-sm outline-none focus:border-accent w-20"
                                    value={addForm.episodeNumber}
                                    onChange={e => setAddForm(f => ({...f, episodeNumber: e.target.value}))}
                                />
                                <input
                                    className="bg-base border border-border rounded-lg px-3 py-1.5 text-surface text-sm outline-none focus:border-accent flex-1"
                                    value={addForm.title}
                                    onChange={e => setAddForm(f => ({...f, title: e.target.value}))}
                                    placeholder="Title (optional)"
                                />
                                <button
                                    onClick={handleAdd}
                                    className="px-3 py-1.5 text-sm font-semibold bg-accent text-base rounded-lg hover:bg-accent-dim transition-colors"
                                >
                                    Add
                                </button>
                                <button
                                    onClick={() => setShowAddForm(false)}
                                    className="px-3 py-1.5 text-sm text-muted border border-border rounded-lg hover:text-surface transition-colors"
                                >
                                    Cancel
                                </button>
                            </div>
                        </td>
                    </tr>
                ) : (
                    <tr className="border-t border-border">
                        <td colSpan={4} className="px-4 py-1.5">
                            <button
                                onClick={handleStartAdd}
                                className="text-xs text-muted hover:text-accent transition-colors py-1"
                            >
                                + Add Episode
                            </button>
                        </td>
                    </tr>
                )}
                </tbody>
            </table>

            {deletingEpisode && (
                <ConfirmDialog
                    message={`Delete E${deletingEpisode.episodeNumber}${deletingEpisode.title ? ` "${deletingEpisode.title}"` : ''}?`}
                    onConfirm={() => handleDelete(deletingEpisode.id)}
                    onCancel={() => setDeletingId(null)}
                />
            )}
        </div>
    )
}