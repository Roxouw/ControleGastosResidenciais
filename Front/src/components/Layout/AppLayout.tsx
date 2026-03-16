import { NavLink, Outlet } from 'react-router-dom'

/** Itens de navegação lateral */
const NAV_ITEMS = [
  { to: '/pessoas',     label: 'Pessoas',     icon: '👤' },
  { to: '/categorias',  label: 'Categorias',  icon: '🏷️' },
  { to: '/transacoes',  label: 'Transações',  icon: '💸' },
  { to: '/relatorios/pessoas',     label: 'Rel. Pessoas',    icon: '📊' },
  { to: '/relatorios/categorias',  label: 'Rel. Categorias', icon: '📈' },
]

/**
 * Layout principal da aplicação.
 * Composto por uma sidebar fixa à esquerda e área de conteúdo principal.
 * Utiliza o <Outlet /> do React Router para renderizar a página ativa.
 */
export function AppLayout() {
  return (
    <div style={{ display: 'flex', minHeight: '100vh', background: 'var(--bg)' }}>
      <Sidebar />
      <main style={{ flex: 1, padding: '40px 48px', overflowY: 'auto' }}>
        <div style={{ maxWidth: 960, margin: '0 auto' }}>
          <Outlet />
        </div>
      </main>
    </div>
  )
}

/**
 * Sidebar de navegação lateral.
 * Usa NavLink do React Router para marcar o item ativo automaticamente.
 */
function Sidebar() {
  return (
    <aside style={{
      width: 220, flexShrink: 0,
      background: 'var(--ink)', color: '#fff',
      display: 'flex', flexDirection: 'column',
      padding: '32px 0',
      position: 'sticky', top: 0, height: '100vh',
    }}>
      {/* Logo / nome do sistema */}
      <div style={{ padding: '0 24px 32px' }}>
        <div style={{ fontFamily: 'var(--font-display)', fontSize: 20, lineHeight: 1.2, color: '#fff' }}>
          Gastos
        </div>
        <div style={{ fontFamily: 'var(--font-display)', fontSize: 20, fontStyle: 'italic', color: 'rgba(255,255,255,0.45)' }}>
          Residenciais
        </div>
      </div>

      {/* Divisor */}
      <div style={{ height: 1, background: 'rgba(255,255,255,0.08)', marginBottom: 12 }} />

      {/* Navegação */}
      <nav style={{ flex: 1, padding: '8px 0' }}>
        {NAV_ITEMS.map(item => (
          <NavLink
            key={item.to}
            to={item.to}
            style={({ isActive }) => ({
              display: 'flex', alignItems: 'center', gap: 10,
              padding: '10px 24px', textDecoration: 'none',
              fontSize: 13.5, fontWeight: isActive ? 600 : 400,
              color: isActive ? '#fff' : 'rgba(255,255,255,0.5)',
              background: isActive ? 'rgba(255,255,255,0.10)' : 'transparent',
              borderLeft: `3px solid ${isActive ? '#fff' : 'transparent'}`,
              transition: 'all 0.15s',
            })}
          >
            <span style={{ fontSize: 15 }}>{item.icon}</span>
            {item.label}
          </NavLink>
        ))}
      </nav>

      {/* Rodapé da sidebar */}
      <div style={{ padding: '0 24px', fontSize: 11, color: 'rgba(255,255,255,0.25)' }}>
        v1.0 · .NET 8 + React
      </div>
    </aside>
  )
}
