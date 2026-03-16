import { useState, useEffect, useCallback } from 'react'
import { useForm } from 'react-hook-form'
import { transacaoService, pessoaService, categoriaService } from '../../services'
import type { Transacao, CreateTransacaoDto, Pessoa, Categoria, TipoTransacao } from '../../types'
import {
  Button, Card, Badge, Modal, Input, Select,
  FormGrid, FormActions, PageHeader, EmptyState, Spinner, ErrorBanner,
} from '../../components/UI'

/** Formata valor monetário em BRL */
const moeda = (v: number) =>
  v.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })

/**
 * Página de Transações.
 * Lista todas as transações e permite criar novas.
 *
 * Regras aplicadas no formulário (além das validadas no backend):
 * - Ao selecionar o tipo de transação, recarrega as categorias compatíveis
 * - Ao selecionar uma pessoa menor de 18 anos, exibe aviso e força tipo=Despesa
 */
export default function TransacoesPage() {
  const [transacoes, setTransacoes] = useState<Transacao[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [modalOpen, setModalOpen] = useState(false)

  const loadTransacoes = useCallback(async () => {
    try {
      setLoading(true)
      const data = await transacaoService.getAll()
      setTransacoes(data)
    } catch {
      setError('Erro ao carregar transações.')
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => { loadTransacoes() }, [loadTransacoes])

  return (
    <div className="animate-fade-up">
      <PageHeader
        title="Transações"
        subtitle="Registre receitas e despesas"
        action={<Button onClick={() => setModalOpen(true)}>+ Nova transação</Button>}
      />

      {error && <ErrorBanner message={error} onClose={() => setError('')} />}
      {loading ? <Spinner /> : (
        transacoes.length === 0
          ? <EmptyState icon="💸" title="Nenhuma transação registrada" subtitle="Crie pessoas e categorias antes de registrar transações." />
          : (
            <div style={{ display: 'flex', flexDirection: 'column', gap: 8 }} className="stagger">
              {transacoes.map(t => (
                <Card key={t.id} className="animate-fade-up">
                  <div style={{ display: 'flex', alignItems: 'center', padding: '14px 20px', gap: 16 }}>
                    {/* Ícone de tipo */}
                    <div style={{
                      width: 38, height: 38, borderRadius: 'var(--radius)',
                      background: t.tipo === 'Receita' ? 'var(--green-bg)' : 'var(--red-bg)',
                      display: 'flex', alignItems: 'center', justifyContent: 'center',
                      fontSize: 18, flexShrink: 0,
                    }}>
                      {t.tipo === 'Receita' ? '↑' : '↓'}
                    </div>

                    {/* Info */}
                    <div style={{ flex: 1, minWidth: 0 }}>
                      <p style={{ fontWeight: 600, fontSize: 14, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
                        {t.descricao}
                      </p>
                      <p style={{ fontSize: 12, color: 'var(--ink-2)' }}>
                        {t.pessoaNome} · {t.categoriaNome}
                      </p>
                    </div>

                    {/* Badge tipo */}
                    <Badge
                      label={t.tipo}
                      color={t.tipo === 'Receita' ? 'green' : 'red'}
                    />

                    {/* Valor */}
                    <p style={{
                      fontFamily: 'var(--font-display)', fontSize: 17,
                      color: t.tipo === 'Receita' ? 'var(--green)' : 'var(--red)',
                      minWidth: 110, textAlign: 'right', flexShrink: 0,
                    }}>
                      {t.tipo === 'Receita' ? '+' : '−'} {moeda(t.valor)}
                    </p>
                  </div>
                </Card>
              ))}
            </div>
          )
      )}

      <TransacaoModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        onSaved={() => { setModalOpen(false); loadTransacoes() }}
      />
    </div>
  )
}

// ── Modal de criação de transação ─────────────────────────────────────────────

interface TransacaoModalProps {
  open: boolean
  onClose: () => void
  onSaved: () => void
}

function TransacaoModal({ open, onClose, onSaved }: TransacaoModalProps) {
  const [apiError, setApiError] = useState('')
  const [pessoas, setPessoas] = useState<Pessoa[]>([])
  const [categorias, setCategorias] = useState<Categoria[]>([])
  const [menorDeIdade, setMenorDeIdade] = useState(false)

  const { register, handleSubmit, reset, watch, setValue, formState: { errors, isSubmitting } } = useForm<CreateTransacaoDto>({
    defaultValues: { tipo: 'Despesa' },
  })

  const tipoSelecionado = watch('tipo') as TipoTransacao
  const pessoaIdSelecionada = watch('pessoaId')

  /** Carrega pessoas e categorias ao abrir o modal */
  useEffect(() => {
    if (!open) return
    reset({ descricao: '', valor: undefined, tipo: 'Despesa', categoriaId: '', pessoaId: '' })
    setApiError('')
    setMenorDeIdade(false)
    pessoaService.getAll().then(setPessoas).catch(() => {})
  }, [open, reset])

  /**
   * Recarrega as categorias sempre que o tipo de transação muda.
   * O backend filtra por compatibilidade (Despesa/Receita/Ambas).
   */
  useEffect(() => {
    if (!open) return
    setCategorias([])
    setValue('categoriaId', '')
    categoriaService.getCompatíveis(tipoSelecionado).then(setCategorias).catch(() => {})
  }, [tipoSelecionado, open, setValue])

  /**
   * Ao selecionar uma pessoa, verifica se é menor de idade.
   * Se for, força o tipo para Despesa e exibe aviso.
   */
  useEffect(() => {
    const pessoa = pessoas.find(p => p.id === pessoaIdSelecionada)
    if (pessoa && pessoa.idade < 18) {
      setMenorDeIdade(true)
      setValue('tipo', 'Despesa')
    } else {
      setMenorDeIdade(false)
    }
  }, [pessoaIdSelecionada, pessoas, setValue])

  const onSubmit = async (data: CreateTransacaoDto) => {
    setApiError('')
    try {
      await transacaoService.create(data)
      onSaved()
    } catch (err: any) {
      setApiError(err.response?.data?.message ?? 'Erro ao salvar.')
    }
  }

  return (
    <Modal open={open} title="Nova transação" onClose={onClose}>
      <form onSubmit={handleSubmit(onSubmit)}>
        <FormGrid>
          {apiError && <ErrorBanner message={apiError} onClose={() => setApiError('')} />}

          {/* Aviso de menor de idade */}
          {menorDeIdade && (
            <div style={{
              background: 'var(--amber-bg)', border: '1px solid #fcd34d',
              borderRadius: 'var(--radius)', padding: '10px 14px', fontSize: 13, color: 'var(--amber)',
            }}>
              ⚠️ Pessoa menor de 18 anos — apenas <strong>Despesas</strong> são permitidas.
            </div>
          )}

          <Input
            label="Descrição"
            placeholder="Ex.: Conta de luz — janeiro"
            error={errors.descricao?.message}
            {...register('descricao', {
              required: 'Descrição é obrigatória',
              maxLength: { value: 400, message: 'Máximo 400 caracteres' },
            })}
          />

          <Input
            label="Valor (R$)"
            type="number"
            step="0.01"
            min="0.01"
            placeholder="0,00"
            error={errors.valor?.message}
            {...register('valor', {
              required: 'Valor é obrigatório',
              valueAsNumber: true,
              min: { value: 0.01, message: 'O valor deve ser positivo' },
            })}
          />

          {/* Tipo: desabilitado se menor de idade */}
          <Select
            label="Tipo"
            error={errors.tipo?.message}
            disabled={menorDeIdade}
            {...register('tipo', { required: 'Tipo é obrigatório' })}
          >
            <option value="Despesa">Despesa</option>
            <option value="Receita">Receita</option>
          </Select>

          <Select
            label="Pessoa"
            error={errors.pessoaId?.message}
            {...register('pessoaId', { required: 'Pessoa é obrigatória' })}
          >
            <option value="">Selecione uma pessoa…</option>
            {pessoas.map(p => (
              <option key={p.id} value={p.id}>
                {p.nome} ({p.idade} anos{p.idade < 18 ? ' — menor' : ''})
              </option>
            ))}
          </Select>

          {/* Categorias filtradas por compatibilidade com o tipo selecionado */}
          <Select
            label={`Categoria (${tipoSelecionado})`}
            error={errors.categoriaId?.message}
            {...register('categoriaId', { required: 'Categoria é obrigatória' })}
          >
            <option value="">Selecione uma categoria…</option>
            {categorias.map(c => (
              <option key={c.id} value={c.id}>
                {c.descricao} ({c.finalidade})
              </option>
            ))}
          </Select>

          <FormActions>
            <Button variant="ghost" type="button" onClick={onClose}>Cancelar</Button>
            <Button type="submit" loading={isSubmitting}>Registrar transação</Button>
          </FormActions>
        </FormGrid>
      </form>
    </Modal>
  )
}
