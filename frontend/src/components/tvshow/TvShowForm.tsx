import {useState} from 'react'
import {useTmdbTvShowSearch, useTmdbStatus} from '../../hooks/useTmdb'
import type {TmdbTvShowSearchDto} from '../../types/tmdb'
import type {TvShowFormData} from '../../types/forms'

interface Props {
    initialData: TvShowFormData
    onSave: (dto: TvShowFormData) => void
    onCancel?: () => void
    isSaving: boolean
    title: string
}

export default function TvShowForm({initialData, onSave, onCancel, isSaving, title}: Props) {
    const [form, setForm] = useState<TvShowFormData>(initialData)
    const [showResults, setShowResults] = useState(false)
    const {data: tmdbStatus} = useTmdbStatus()
    const {data: searchResults, isFetching} = useTmdbTvShowSearch(form.title, !!tmdbStatus && showResults)

    const handleTitleChange = (value: string) => {
        setForm(f => ({...f, title: value, tmdbId: null}))
        setShowResults(true)
    }

    const handleSelectResult = (result: TmdbTvShowSearchDto) => {
        setForm(f => ({...f, title: result.title, tmdbId: result.tmdbId}))
        setShowResults(false)
    }

    return (
        <div className="max-w-lg">
            <h1 className="text-2xl font-bold tracking-tight mb-8">{title}{form.releaseYear ? ` · ${form.releaseYear}` : ''}</h1>

            <div className="flex flex-col gap-5">
                <div className="flex flex-col gap-1.5 relative">
                    <label className="text-xs font-medium text-muted uppercase tracking-widest">
                        Title
                    </label>
                    <input
                        className={`bg-card border rounded-lg px-3.5 py-2.5 text-surface text-sm outline-none transition-colors ${
                            form.tmdbId
                                ? 'border-green-500 focus:border-green-400'
                                : 'border-border focus:border-accent'
                        }`}
                        value={form.title}
                        onChange={e => handleTitleChange(e.target.value)}
                        onFocus={() => setShowResults(true)}
                        placeholder="TV show title"
                    />

                    {tmdbStatus && showResults && form.title.length >= 2 && (
                        <>
                            <div
                                className="fixed inset-0 z-9"
                                onMouseDown={() => setShowResults(false)}
                            />
                            <div
                                className="absolute top-full left-0 right-0 mt-1 bg-card border border-border rounded-lg overflow-hidden z-10 shadow-lg">
                                {isFetching ? (
                                    <div className="px-4 py-3 text-sm text-muted">Searching...</div>
                                ) : searchResults && searchResults.length > 0 ? (
                                    searchResults.map(result => (
                                        <button
                                            key={result.tmdbId}
                                            className="w-full text-left px-4 py-2.5 text-sm hover:bg-card-hover transition-colors flex justify-between items-center"
                                            onMouseDown={() => handleSelectResult(result)}
                                        >
                                            <span className="text-surface">{result.title}</span>
                                            {result.releaseYear && (
                                                <span className="text-muted text-xs">{result.releaseYear}</span>
                                            )}
                                        </button>
                                    ))
                                ) : (
                                    <div className="px-4 py-3 text-sm text-muted">No results found.</div>
                                )}
                            </div>
                        </>
                    )}
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