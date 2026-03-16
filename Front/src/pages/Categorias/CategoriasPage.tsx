import { useState, useEffect, useCallback } from 'react'
import { useForm } from 'react-hook-form'
import { categoriaService } from '../../services'
import type { Categoria, CreateCategoriaDto, FinalidadeCategoria } from '../../types'
import {
  Button, Card, Badge, Modal, Input, Select,
  FormGrid, FormActions, PageHeader, EmptyState, Spinner, ErrorBanner,
} from '../../components/UI'

/** Mapeia a finalidade para exibição amigável e cor do badge */
const FINALIDADE_INFO: Record<FinalidadeCategoria, { label: string; color: 'green' | 'red' | 'blue' }> = {
  Despesa: { label: 'Despesa',  color: 'red' },
  Receita: { label: 'Receita',  color: 'green' },
  Ambas:   { label: 'Ambas',   color: 'blue' },
}

/**
 * Página de Categorias.
 * Exibe a lista de categorias e permite criar novas.
 * Não há edição/exclusão de categorias conforme a especificação.
 */
export default function CategoriasPage() {
  const [categorias, setCategorias] = useState<Categoria[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [modalOpen, setModalOpen] = useState(false)

  const loadCategorias = useCallback(async () => {
    try {
      setLoading(true)
      const data = await categoriaService.getAll()
      setCategorias(data)
    } catch {
      setError('Erro ao carregar categorias.')
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => { loadCategorias() }, [loadCategorias])

  return (
    <div className="animate-fade-up">
      <PageHeader
        title="Categorias"
        subtitle="Classifique transações por categoria"
        action={<Button onClick={() => setModalOpen(true)}>+ Nova categoria</Button>}
      />

      {error && <ErrorBanner message={error} onClose={() => setError('')} />}
      {loading ? <Spinner /> : (
        categorias.length === 0
          ? <EmptyState icon="🏷️" title="Nenhuma categoria cadastrada" subtitle="Crie categorias para organizar suas transações." />
          : (
            <div className="stagger" style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))', gap: 12 }}>
              {categorias.map(c => {
                const info = FINALIDADE_INFO[c.finalidade]
                return (
                  <Card key={c.id} className="animate-fade-up">
                    <div style={{ padding: '18px 20px' }}>
                      <div style={{ display: 'flex', alignItems: 'flex-start', justifyContent: 'space-between', gap: 8 }}>
                        <p style={{ fontWeight: 600, fontSize: 14, lineHeight: 1.4, flex: 1 }}>{c.descricao}</p>
                        <Badge label={info.label} color={info.color} />
                      </div>
                      <p style={{ marginTop: 8, fontSize: 12, color: 'var(--ink-3)', fontFamily: 'monospace' }}>
                        {c.id.substring(0, 8)}…
                      </p>
                    </div>
                  </Card>
                )
              })}
            </div>
          )
      )}

      <CategoriaModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        onSaved={() => { setModalOpen(false); loadCategorias() }}
      />
    </div>
  )
}

// ── Modal de criação de categoria ─────────────────────────────────────────────

interface CategoriaModalProps {
  open: boolean
  onClose: () => void
  onSaved: () => void
}

function CategoriaModal({ open, onClose, onSaved }: CategoriaModalProps) {
  const [apiError, setApiError] = useState('')
  const { register, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm<CreateCategoriaDto>()

  useEffect(() => {
    if (open) { reset({ descricao: '', finalidade: 'Despesa' }); setApiError('') }
  }, [open, reset])

  const onSubmit = async (data: CreateCategoriaDto) => {
    setApiError('')
    try {
      await categoriaService.create(data)
      onSaved()
    } catch (err: any) {
      setApiError(err.response?.data?.message ?? 'Erro ao salvar.')
    }
  }

  return (
    <Modal open={open} title="Nova categoria" onClose={onClose}>
      <form onSubmit={handleSubmit(onSubmit)}>
        <FormGrid>
          {apiError && <ErrorBanner message={apiError} onClose={() => setApiError('')} />}

          <Input
            label="Descrição"
            placeholder="Ex.: Alimentação, Salário, Transporte…"
            error={errors.descricao?.message}
            {...register('descricao', {
              required: 'Descrição é obrigatória',
              maxLength: { value: 400, message: 'Máximo 400 caracteres' },
            })}
          />

          <Select
            label="Finalidade"
            error={errors.finalidade?.message}
            {...register('finalidade', { required: 'Finalidade é obrigatória' })}
          >
            <option value="Despesa">Despesa — apenas para transações de saída</option>
            <option value="Receita">Receita — apenas para transações de entrada</option>
            <option value="Ambas">Ambas — aceita despesas e receitas</option>
          </Select>

          <FormActions>
            <Button variant="ghost" type="button" onClick={onClose}>Cancelar</Button>
            <Button type="submit" loading={isSubmitting}>Criar categoria</Button>
          </FormActions>
        </FormGrid>
      </form>
    </Modal>
  )
}
