import { useState, useEffect } from 'react'
import { relatorioService } from '../../services'
import type { RelatorioResumoPessoas } from '../../types'
import { Card, PageHeader, Spinner, ErrorBanner } from '../../components/UI'

const moeda = (v: number) =>
  v.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })

/**
 * Página de Relatório por Pessoa.
 * Lista cada pessoa com seus totais de receita, despesa e saldo.
 * Ao final, exibe o consolidado geral de todas as pessoas.
 */
export default function RelatorioPessoasPage() {
  const [data, setData] = useState<RelatorioResumoPessoas | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    relatorioService.getPorPessoa()
      .then(setData)
      .catch(() => setError('Erro ao carregar relatório.'))
      .finally(() => setLoading(false))
  }, [])

  return (
    <div className="animate-fade-up">
      <PageHeader
        title="Relatório por Pessoa"
        subtitle="Totais de receitas, despesas e saldo de cada pessoa"
      />

      {error && <ErrorBanner message={error} onClose={() => setError('')} />}
      {loading ? <Spinner /> : data && (
        <>
          {/* Tabela de pessoas */}
          <Card style={{ overflow: 'hidden', marginBottom: 16 }}>
            <TabelaRelatorio
              headers={['Pessoa', 'Idade', 'Receitas', 'Despesas', 'Saldo']}
            >
              {data.pessoas.length === 0 ? (
                <tr>
                  <td colSpan={5} style={{ padding: '32px', textAlign: 'center', color: 'var(--ink-3)', fontSize: 14 }}>
                    Nenhuma pessoa cadastrada
                  </td>
                </tr>
              ) : data.pessoas.map(p => (
                <tr key={p.pessoaId} style={{ borderBottom: '1px solid var(--border)' }}>
                  <td style={tdStyle}>
                    <span style={{ fontWeight: 600 }}>{p.pessoaNome}</span>
                    {p.pessoaIdade < 18 && (
                      <span style={{ marginLeft: 8, fontSize: 11, background: 'var(--amber-bg)', color: 'var(--amber)', padding: '1px 7px', borderRadius: 99, fontWeight: 600 }}>menor</span>
                    )}
                  </td>
                  <td style={{ ...tdStyle, color: 'var(--ink-2)' }}>{p.pessoaIdade} anos</td>
                  <ValorCell valor={p.totalReceitas} tipo="receita" />
                  <ValorCell valor={p.totalDespesas} tipo="despesa" />
                  <SaldoCell saldo={p.saldo} />
                </tr>
              ))}
            </TabelaRelatorio>
          </Card>

          {/* Totais gerais */}
          <TotaisGerais totais={data.totaisGerais} />
        </>
      )}
    </div>
  )
}

// ── Componentes auxiliares ────────────────────────────────────────────────────

function TabelaRelatorio({ headers, children }: { headers: string[]; children: React.ReactNode }) {
  return (
    <table style={{ width: '100%', borderCollapse: 'collapse' }}>
      <thead>
        <tr style={{ background: 'var(--bg)' }}>
          {headers.map(h => (
            <th key={h} style={{ padding: '12px 20px', textAlign: 'left', fontSize: 11, fontWeight: 700, letterSpacing: 0.8, textTransform: 'uppercase', color: 'var(--ink-3)', borderBottom: '1px solid var(--border)' }}>
              {h}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>{children}</tbody>
    </table>
  )
}

const tdStyle: React.CSSProperties = { padding: '14px 20px', fontSize: 14 }

function ValorCell({ valor, tipo }: { valor: number; tipo: 'receita' | 'despesa' }) {
  return (
    <td style={{ ...tdStyle, color: tipo === 'receita' ? 'var(--green)' : 'var(--red)', fontWeight: 500 }}>
      {moeda(valor)}
    </td>
  )
}

function SaldoCell({ saldo }: { saldo: number }) {
  const positive = saldo >= 0
  return (
    <td style={{ ...tdStyle, fontFamily: 'var(--font-display)', fontSize: 16, color: positive ? 'var(--green)' : 'var(--red)', fontWeight: 600 }}>
      {positive ? '+' : ''}{moeda(saldo)}
    </td>
  )
}

function TotaisGerais({ totais }: { totais: { totalReceitas: number; totalDespesas: number; saldo: number } }) {
  const saldoPositivo = totais.saldo >= 0
  return (
    <Card style={{ background: 'var(--ink)', color: '#fff', overflow: 'hidden' }}>
      <div style={{ padding: '16px 20px', borderBottom: '1px solid rgba(255,255,255,0.1)' }}>
        <p style={{ fontFamily: 'var(--font-display)', fontSize: 13, color: 'rgba(255,255,255,0.5)', letterSpacing: 1, textTransform: 'uppercase' }}>
          Totais Gerais
        </p>
      </div>
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', padding: '20px' }}>
        <div style={{ textAlign: 'center' }}>
          <p style={{ fontSize: 11, color: 'rgba(255,255,255,0.4)', textTransform: 'uppercase', letterSpacing: 1, marginBottom: 6 }}>Total Receitas</p>
          <p style={{ fontFamily: 'var(--font-display)', fontSize: 22, color: '#6ee7b7' }}>{moeda(totais.totalReceitas)}</p>
        </div>
        <div style={{ textAlign: 'center', borderLeft: '1px solid rgba(255,255,255,0.1)', borderRight: '1px solid rgba(255,255,255,0.1)' }}>
          <p style={{ fontSize: 11, color: 'rgba(255,255,255,0.4)', textTransform: 'uppercase', letterSpacing: 1, marginBottom: 6 }}>Total Despesas</p>
          <p style={{ fontFamily: 'var(--font-display)', fontSize: 22, color: '#fca5a5' }}>{moeda(totais.totalDespesas)}</p>
        </div>
        <div style={{ textAlign: 'center' }}>
          <p style={{ fontSize: 11, color: 'rgba(255,255,255,0.4)', textTransform: 'uppercase', letterSpacing: 1, marginBottom: 6 }}>Saldo Líquido</p>
          <p style={{ fontFamily: 'var(--font-display)', fontSize: 22, color: saldoPositivo ? '#6ee7b7' : '#fca5a5' }}>
            {saldoPositivo ? '+' : ''}{moeda(totais.saldo)}
          </p>
        </div>
      </div>
    </Card>
  )
}
