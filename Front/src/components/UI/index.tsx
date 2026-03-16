import React from 'react'

// ── Button ────────────────────────────────────────────────────────────────────

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'ghost' | 'danger'
  size?: 'sm' | 'md'
  loading?: boolean
}

export function Button({
  variant = 'primary', size = 'md', loading, children, disabled, style, ...props
}: ButtonProps) {
  const base: React.CSSProperties = {
    display: 'inline-flex', alignItems: 'center', gap: 6,
    fontFamily: 'var(--font-body)', fontWeight: 500, cursor: 'pointer',
    border: 'none', borderRadius: 'var(--radius)', transition: 'all 0.15s',
    opacity: disabled || loading ? 0.55 : 1,
    fontSize: size === 'sm' ? 13 : 14,
    padding: size === 'sm' ? '6px 12px' : '9px 18px',
    ...(variant === 'primary' && {
      background: 'var(--ink)', color: '#fff',
    }),
    ...(variant === 'ghost' && {
      background: 'transparent', color: 'var(--ink-2)',
      border: '1px solid var(--border)',
    }),
    ...(variant === 'danger' && {
      background: 'var(--red-bg)', color: 'var(--red)',
      border: '1px solid #f5c6c2',
    }),
    ...style,
  }
  return (
    <button style={base} disabled={disabled || loading} {...props}>
      {loading && <span style={{ width: 14, height: 14, border: '2px solid currentColor', borderTopColor: 'transparent', borderRadius: '50%', display: 'inline-block', animation: 'spin 0.7s linear infinite' }} />}
      {children}
    </button>
  )
}

// ── Input ─────────────────────────────────────────────────────────────────────

interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label?: string
  error?: string
}

export const Input = React.forwardRef<HTMLInputElement, InputProps>(
  ({ label, error, style, ...props }, ref) => {
    return (
      <div style={{ display: 'flex', flexDirection: 'column', gap: 5 }}>
        {label && <label style={{ fontSize: 13, fontWeight: 500, color: 'var(--ink-2)' }}>{label}</label>}
        <input
          ref={ref}
          style={{
            fontFamily: 'var(--font-body)', fontSize: 14,
            padding: '9px 12px', borderRadius: 'var(--radius)',
            border: `1px solid ${error ? 'var(--red)' : 'var(--border)'}`,
            background: 'var(--surface)', color: 'var(--ink)',
            outline: 'none', transition: 'border-color 0.15s',
            width: '100%',
            ...style,
          }}
          onFocus={e => (e.target.style.borderColor = 'var(--ink)')}
          onBlur={e => (e.target.style.borderColor = error ? 'var(--red)' : 'var(--border)')}
          {...props}
        />
        {error && <span style={{ fontSize: 12, color: 'var(--red)' }}>{error}</span>}
      </div>
    )
  }
)

// ── Select ────────────────────────────────────────────────────────────────────

interface SelectProps extends React.SelectHTMLAttributes<HTMLSelectElement> {
  label?: string
  error?: string
}

export const Select = React.forwardRef<HTMLSelectElement, SelectProps>(
  ({ label, error, children, style, ...props }, ref) => {
    return (
      <div style={{ display: 'flex', flexDirection: 'column', gap: 5 }}>
        {label && <label style={{ fontSize: 13, fontWeight: 500, color: 'var(--ink-2)' }}>{label}</label>}
        <select
          ref={ref}
          style={{
            fontFamily: 'var(--font-body)', fontSize: 14,
            padding: '9px 12px', borderRadius: 'var(--radius)',
            border: `1px solid ${error ? 'var(--red)' : 'var(--border)'}`,
            background: 'var(--surface)', color: 'var(--ink)',
            outline: 'none', cursor: 'pointer', appearance: 'none',
            backgroundImage: `url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 24 24' fill='none' stroke='%235C5650' stroke-width='2'%3E%3Cpath d='M6 9l6 6 6-6'/%3E%3C/svg%3E")`,
            backgroundRepeat: 'no-repeat', backgroundPosition: 'right 12px center',
            paddingRight: 36, width: '100%',
            ...style,
          }}
          {...props}
        >
          {children}
        </select>
        {error && <span style={{ fontSize: 12, color: 'var(--red)' }}>{error}</span>}
      </div>
    )
  }
)

// ── Card ──────────────────────────────────────────────────────────────────────

export function Card({ children, style }: { children: React.ReactNode; style?: React.CSSProperties }) {
  return (
    <div style={{
      background: 'var(--surface)', border: '1px solid var(--border)',
      borderRadius: 'var(--radius-lg)', boxShadow: 'var(--shadow-sm)',
      ...style,
    }}>
      {children}
    </div>
  )
}

// ── Badge ─────────────────────────────────────────────────────────────────────

interface BadgeProps { label: string; color?: 'green' | 'red' | 'amber' | 'blue' | 'gray' }

export function Badge({ label, color = 'gray' }: BadgeProps) {
  const colors = {
    green: { bg: 'var(--green-bg)', color: 'var(--green)' },
    red:   { bg: 'var(--red-bg)',   color: 'var(--red)' },
    amber: { bg: 'var(--amber-bg)', color: 'var(--amber)' },
    blue:  { bg: 'var(--blue-light)', color: 'var(--blue)' },
    gray:  { bg: 'var(--border)',   color: 'var(--ink-2)' },
  }
  return (
    <span style={{
      display: 'inline-block', padding: '2px 10px', borderRadius: 99,
      fontSize: 11, fontWeight: 600, letterSpacing: 0.4,
      background: colors[color].bg, color: colors[color].color,
    }}>
      {label}
    </span>
  )
}

// ── Spinner ───────────────────────────────────────────────────────────────────

export function Spinner() {
  return (
    <div style={{ display: 'flex', justifyContent: 'center', padding: 48 }}>
      <div style={{
        width: 32, height: 32, border: '3px solid var(--border)',
        borderTopColor: 'var(--ink)', borderRadius: '50%',
        animation: 'spin 0.7s linear infinite',
      }} />
      <style>{`@keyframes spin { to { transform: rotate(360deg); } }`}</style>
    </div>
  )
}

// ── EmptyState ────────────────────────────────────────────────────────────────

export function EmptyState({ icon, title, subtitle }: { icon: string; title: string; subtitle?: string }) {
  return (
    <div style={{ textAlign: 'center', padding: '56px 24px', color: 'var(--ink-3)' }}>
      <div style={{ fontSize: 40, marginBottom: 12 }}>{icon}</div>
      <p style={{ fontSize: 15, fontWeight: 500, color: 'var(--ink-2)', marginBottom: 4 }}>{title}</p>
      {subtitle && <p style={{ fontSize: 13 }}>{subtitle}</p>}
    </div>
  )
}

// ── Modal ─────────────────────────────────────────────────────────────────────

interface ModalProps {
  open: boolean
  title: string
  onClose: () => void
  children: React.ReactNode
}

export function Modal({ open, title, onClose, children }: ModalProps) {
  if (!open) return null
  return (
    <div
      style={{
        position: 'fixed', inset: 0, zIndex: 50,
        background: 'rgba(26,23,20,0.45)', backdropFilter: 'blur(4px)',
        display: 'flex', alignItems: 'center', justifyContent: 'center',
        padding: 24, animation: 'fadeIn 0.2s ease',
      }}
      onClick={e => e.target === e.currentTarget && onClose()}
    >
      <div style={{
        background: 'var(--surface)', borderRadius: 'var(--radius-lg)',
        boxShadow: 'var(--shadow-lg)', width: '100%', maxWidth: 480,
        animation: 'fadeUp 0.25s ease',
      }}>
        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', padding: '20px 24px', borderBottom: '1px solid var(--border)' }}>
          <h2 style={{ fontFamily: 'var(--font-display)', fontSize: 20 }}>{title}</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', cursor: 'pointer', fontSize: 20, color: 'var(--ink-3)', lineHeight: 1, padding: 4 }}>×</button>
        </div>
        <div style={{ padding: 24 }}>{children}</div>
      </div>
    </div>
  )
}

// ── ConfirmModal ──────────────────────────────────────────────────────────────

interface ConfirmModalProps {
  open: boolean
  title: string
  message: string
  onConfirm: () => void
  onCancel: () => void
  loading?: boolean
}

export function ConfirmModal({ open, title, message, onConfirm, onCancel, loading }: ConfirmModalProps) {
  if (!open) return null
  return (
    <Modal open={open} title={title} onClose={onCancel}>
      <p style={{ color: 'var(--ink-2)', marginBottom: 24, lineHeight: 1.6 }}>{message}</p>
      <div style={{ display: 'flex', gap: 10, justifyContent: 'flex-end' }}>
        <Button variant="ghost" onClick={onCancel}>Cancelar</Button>
        <Button variant="danger" onClick={onConfirm} loading={loading}>Confirmar exclusão</Button>
      </div>
    </Modal>
  )
}

// ── PageHeader ────────────────────────────────────────────────────────────────

export function PageHeader({ title, subtitle, action }: { title: string; subtitle?: string; action?: React.ReactNode }) {
  return (
    <div style={{ display: 'flex', alignItems: 'flex-start', justifyContent: 'space-between', marginBottom: 28, flexWrap: 'wrap', gap: 12 }}>
      <div>
        <h1 style={{ fontFamily: 'var(--font-display)', fontSize: 32, lineHeight: 1.1, color: 'var(--ink)' }}>{title}</h1>
        {subtitle && <p style={{ marginTop: 4, fontSize: 14, color: 'var(--ink-2)' }}>{subtitle}</p>}
      </div>
      {action}
    </div>
  )
}

// ── FormGrid ──────────────────────────────────────────────────────────────────

export function FormGrid({ children }: { children: React.ReactNode }) {
  return <div style={{ display: 'flex', flexDirection: 'column', gap: 16 }}>{children}</div>
}

export function FormActions({ children }: { children: React.ReactNode }) {
  return <div style={{ display: 'flex', gap: 10, justifyContent: 'flex-end', marginTop: 8 }}>{children}</div>
}

// ── ErrorBanner ───────────────────────────────────────────────────────────────

export function ErrorBanner({ message, onClose }: { message: string; onClose: () => void }) {
  return (
    <div style={{
      background: 'var(--red-bg)', border: '1px solid #f5c6c2',
      borderRadius: 'var(--radius)', padding: '12px 16px',
      display: 'flex', alignItems: 'flex-start', gap: 10, marginBottom: 16,
      animation: 'fadeUp 0.2s ease',
    }}>
      <span style={{ fontSize: 16 }}>⚠️</span>
      <p style={{ flex: 1, fontSize: 13, color: 'var(--red)', lineHeight: 1.5 }}>{message}</p>
      <button onClick={onClose} style={{ background: 'none', border: 'none', cursor: 'pointer', color: 'var(--red)', fontSize: 16 }}>×</button>
    </div>
  )
}