import {useState} from 'react'
import type {CreateTvShowDto} from '../../types/tvshow'

interface Props {
    initialData: CreateTvShowDto
    onSave: (dto: CreateTvShowDto) => void
    onCancel?: () => void
    isSaving: boolean
    title: string
}

export default function TvShowForm({initialData, onSave, onCancel, isSaving, title}: Props) {
    const [form, setForm] = useState<CreateTvShowDto>(initialData)

    return (
        <div className="max-w-lg">
            <h1 className="text-2xl font-bold tracking-tight mb-8">{title}</h1>

            <div className="flex flex-col gap-5">
                <div className="flex flex-col gap-1.5">
                    <label className="text-xs font-medium text-muted uppercase tracking-widest">
                        Title
                    </label>
                    <input
                        className="bg-card border border-border rounded-lg px-3.5 py-2.5 text-surface text-sm outline-none focus:border-accent transition-colors"
                        value={form.title}
                        onChange={e => setForm(f => ({...f, title: e.target.value}))}
                        placeholder="TV show title"
                    />
                </div>

                <div className="flex flex-col gap-1.5">
                    <label className="text-xs font-medium text-muted uppercase tracking-widest">
                        Rating <span className="normal-case font-normal">(1–10, optional)</span>
                    </label>
                    <input
                        className="bg-card border border-border rounded-lg px-3.5 py-2.5 text-surface text-sm outline-none focus:border-accent transition-colors w-24"
                        type="number"
                        min={1}
                        max={10}
                        value={form.userRating ?? ''}
                        onChange={e => setForm(f => ({
                            ...f,
                            userRating: e.target.value ? Number(e.target.value) : null
                        }))}
                        placeholder="—"
                    />
                </div>
            </div>

            <div className="flex gap-3 mt-8">
                {onCancel && (
                    <button
                        onClick={onCancel}
                        className="px-4 py-2 text-sm text-muted border border-border rounded-lg hover:text-surface hover:border-muted transition-colors"
                    >
                        Cancel
                    </button>
                )}
                <button
                    onClick={() => onSave(form)}
                    disabled={isSaving || !form.title.trim()}
                    className="px-4 py-2 text-sm font-semibold bg-accent text-base rounded-lg hover:bg-accent-dim transition-colors disabled:opacity-40 disabled:cursor-not-allowed"
                >
                    {isSaving ? 'Saving...' : 'Save'}
                </button>
            </div>
        </div>
    )
}