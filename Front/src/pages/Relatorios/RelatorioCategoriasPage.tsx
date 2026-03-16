import { useState, useEffect } from 'react'
import { relatorioService } from '../../services'
import type { RelatorioResumoCategoria } from '../../types'
import { Card, PageHeader, Spinner, ErrorBanner } from '../../components/UI'

const moeda = (v: number) =>
  v.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })

/**
 * Página de Relatório por Categoria.
 * Lista cada categoria com seus totais de receita, despesa e saldo.
 * Ao final, exibe o consolidado geral de todas as categorias.
 */
export default function RelatorioCategoriasPage() {
  const [data, setData] = useState<RelatorioResumoCategoria | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    relatorioService.getPorCategoria()
      .then(setData)
      .catch(() => setError('Erro ao carregar relatório.'))
      .finally(() => setLoading(false))
  }, [])

  return (
    <div className="animate-fade-up">
      <PageHeader
        title="Relatório por Categoria"
        subtitle="Totais de receitas, despesas e saldo agrupados por categoria"
      />

      {error && <ErrorBanner message={error} onClose={() => setError('')} />}
      {loading ? <Spinner /> : data && (
        <>
          <Card style={{ overflow: 'hidden', marginBottom: 16 }}>
            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
              <thead>
                <tr style={{ background: 'var(--bg)' }}>
                  {['Categoria', 'Receitas', 'Despesas', 'Saldo'].map(h => (
                    <th key={h} style={{ padding: '12px 20px', textAlign: 'left', fontSize: 11, fontWeight: 700, letterSpacing: 0.8, textTransform: 'uppercase', color: 'var(--ink-3)', borderBottom: '1px solid var(--border)' }}>
                      {h}
                    </th>
                  ))}
                </tr>
              </thead>
              <tbody>
                {data.categorias.length === 0 ? (
                  <tr>
                    <td colSpan={4} style={{ padding: '32px', textAlign: 'center', color: 'var(--ink-3)', fontSize: 14 }}>
                      Nenhuma categoria cadastrada
                    </td>
                  </tr>
                ) : data.categorias.map(c => {
                  const saldoPos = c.saldo >= 0
                  return (
                    <tr key={c.categoriaId} style={{ borderBottom: '1px solid var(--border)' }}>
                      <td style={{ padding: '14px 20px', fontSize: 14, fontWeight: 600 }}>{c.categoriaDescricao}</td>
                      <td style={{ padding: '14px 20px', fontSize: 14, color: 'var(--green)', fontWeight: 500 }}>{moeda(c.totalReceitas)}</td>
                      <td style={{ padding: '14px 20px', fontSize: 14, color: 'var(--red)', fontWeight: 500 }}>{moeda(c.totalDespesas)}</td>
                      <td style={{ padding: '14px 20px', fontFamily: 'var(--font-display)', fontSize: 16, color: saldoPos ? 'var(--green)' : 'var(--red)', fontWeight: 600 }}>
                        {saldoPos ? '+' : ''}{moeda(c.saldo)}
                      </td>
                    </tr>
                  )
                })}
              </tbody>
            </table>
          </Card>

          {/* Totais gerais */}
          <Card style={{ background: 'var(--ink)', color: '#fff', overflow: 'hidden' }}>
            <div style={{ padding: '16px 20px', borderBottom: '1px solid rgba(255,255,255,0.1)' }}>
              <p style={{ fontFamily: 'var(--font-display)', fontSize: 13, color: 'rgba(255,255,255,0.5)', letterSpacing: 1, textTransform: 'uppercase' }}>
                Totais Gerais
              </p>
            </div>
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', padding: '20px' }}>
              <div style={{ textAlign: 'center' }}>
                <p style={{ fontSize: 11, color: 'rgba(255,255,255,0.4)', textTransform: 'uppercase', letterSpacing: 1, marginBottom: 6 }}>Total Receitas</p>
                <p style={{ fontFamily: 'var(--font-display)', fontSize: 22, color: '#6ee7b7' }}>{moeda(data.totaisGerais.totalReceitas)}</p>
              </div>
              <div style={{ textAlign: 'center', borderLeft: '1px solid rgba(255,255,255,0.1)', borderRight: '1px solid rgba(255,255,255,0.1)' }}>
                <p style={{ fontSize: 11, color: 'rgba(255,255,255,0.4)', textTransform: 'uppercase', letterSpacing: 1, marginBottom: 6 }}>Total Despesas</p>
                <p style={{ fontFamily: 'var(--font-display)', fontSize: 22, color: '#fca5a5' }}>{moeda(data.totaisGerais.totalDespesas)}</p>
              </div>
              <div style={{ textAlign: 'center' }}>
                <p style={{ fontSize: 11, color: 'rgba(255,255,255,0.4)', textTransform: 'uppercase', letterSpacing: 1, marginBottom: 6 }}>Saldo Líquido</p>
                <p style={{ fontFamily: 'var(--font-display)', fontSize: 22, color: data.totaisGerais.saldo >= 0 ? '#6ee7b7' : '#fca5a5' }}>
                  {data.totaisGerais.saldo >= 0 ? '+' : ''}{moeda(data.totaisGerais.saldo)}
                </p>
              </div>
            </div>
          </Card>
        </>
      )}
    </div>
  )
}
