interface Props {
    message: string
    onConfirm: () => void
    onCancel: () => void
}

export default function ConfirmDialog({message, onConfirm, onCancel}: Props) {
    return (
        <div
            className="fixed inset-0 bg-black/70 backdrop-blur-sm flex items-center justify-center z-50"
            onClick={onCancel}
        >
            <div
                className="bg-card border border-border rounded-xl p-8 max-w-sm w-full mx-4"
                onClick={e => e.stopPropagation()}
            >
                <p className="text-surface text-base leading-relaxed mb-6">{message}</p>
                <div className="flex justify-end gap-3">
                    <button
                        onClick={onCancel}
                        className="px-4 py-2 text-sm text-muted border border-border rounded-lg hover:text-surface hover:border-muted transition-colors"
                    >
                        Cancel
                    </button>
                    <button
                        onClick={onConfirm}
                        className="px-4 py-2 text-sm text-white bg-danger rounded-lg hover:opacity-85 transition-opacity"
                    >
                        Delete
                    </button>
                </div>
            </div>
        </div>
    )
}