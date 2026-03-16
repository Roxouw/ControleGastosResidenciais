import { useState, useEffect, useCallback } from 'react'
import { useForm } from 'react-hook-form'
import { pessoaService } from '../../services'
import type { Pessoa, CreatePessoaDto } from '../../types'
import {
  Button, Card, Badge, Modal, ConfirmModal,
  Input, FormGrid, FormActions, PageHeader, EmptyState, Spinner, ErrorBanner,
} from '../../components/UI'

/**
 * Página de Pessoas.
 * Exibe a lista de pessoas e permite criar, editar e deletar.
 * Ao deletar, o backend remove todas as transações da pessoa em cascata.
 */
export default function PessoasPage() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  // Estado do modal de criar/editar
  const [modalOpen, setModalOpen] = useState(false)
  const [editing, setEditing] = useState<Pessoa | null>(null)

  // Estado do modal de confirmação de exclusão
  const [deleteTarget, setDeleteTarget] = useState<Pessoa | null>(null)
  const [deleting, setDeleting] = useState(false)

  /** Busca a lista de pessoas da API. */
  const loadPessoas = useCallback(async () => {
    try {
      setLoading(true)
      const data = await pessoaService.getAll()
      setPessoas(data)
    } catch {
      setError('Erro ao carregar pessoas.')
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => { loadPessoas() }, [loadPessoas])

  /** Abre o modal de criação (sem dados pré-preenchidos). */
  const handleNew = () => { setEditing(null); setModalOpen(true) }

  /** Abre o modal de edição com os dados da pessoa selecionada. */
  const handleEdit = (p: Pessoa) => { setEditing(p); setModalOpen(true) }

  /** Executa a exclusão confirmada pelo usuário. */
  const handleDelete = async () => {
    if (!deleteTarget) return
    setDeleting(true)
    try {
      await pessoaService.delete(deleteTarget.id)
      setDeleteTarget(null)
      loadPessoas()
    } catch {
      setError('Erro ao deletar pessoa.')
    } finally {
      setDeleting(false)
    }
  }

  return (
    <div className="animate-fade-up">
      <PageHeader
        title="Pessoas"
        subtitle="Gerencie as pessoas cadastradas no sistema"
        action={<Button onClick={handleNew}>+ Nova pessoa</Button>}
      />

      {error && <ErrorBanner message={error} onClose={() => setError('')} />}
      {loading ? <Spinner /> : (
        pessoas.length === 0
          ? <EmptyState icon="👤" title="Nenhuma pessoa cadastrada" subtitle="Clique em 'Nova pessoa' para começar." />
          : (
            <div className="stagger" style={{ display: 'flex', flexDirection: 'column', gap: 10 }}>
              {pessoas.map(p => (
                <Card key={p.id} className="animate-fade-up">
                  <div style={{ display: 'flex', alignItems: 'center', padding: '16px 20px', gap: 16 }}>
                    {/* Avatar inicial */}
                    <div style={{
                      width: 42, height: 42, borderRadius: '50%',
                      background: 'var(--ink)', color: '#fff',
                      display: 'flex', alignItems: 'center', justifyContent: 'center',
                      fontFamily: 'var(--font-display)', fontSize: 18, flexShrink: 0,
                    }}>
                      {p.nome.charAt(0).toUpperCase()}
                    </div>

                    {/* Info */}
                    <div style={{ flex: 1 }}>
                      <p style={{ fontWeight: 600, fontSize: 15 }}>{p.nome}</p>
                      <p style={{ fontSize: 13, color: 'var(--ink-2)' }}>{p.idade} anos</p>
                    </div>

                    {/* Badge de menor de idade */}
                    {p.idade < 18 && (
                      <Badge label="Menor de idade" color="amber" />
                    )}

                    {/* Ações */}
                    <div style={{ display: 'flex', gap: 8 }}>
                      <Button variant="ghost" size="sm" onClick={() => handleEdit(p)}>Editar</Button>
                      <Button variant="danger" size="sm" onClick={() => setDeleteTarget(p)}>Excluir</Button>
                    </div>
                  </div>
                </Card>
              ))}
            </div>
          )
      )}

      {/* Modal de criar / editar */}
      <PessoaModal
        open={modalOpen}
        editing={editing}
        onClose={() => setModalOpen(false)}
        onSaved={() => { setModalOpen(false); loadPessoas() }}
      />

      {/* Modal de confirmação de exclusão */}
      <ConfirmModal
        open={!!deleteTarget}
        title="Excluir pessoa"
        message={`Tem certeza que deseja excluir "${deleteTarget?.nome}"? Todas as transações desta pessoa também serão removidas.`}
        onConfirm={handleDelete}
        onCancel={() => setDeleteTarget(null)}
        loading={deleting}
      />
    </div>
  )
}

// ── Modal de criação / edição ─────────────────────────────────────────────────

interface PessoaModalProps {
  open: boolean
  editing: Pessoa | null
  onClose: () => void
  onSaved: () => void
}

function PessoaModal({ open, editing, onClose, onSaved }: PessoaModalProps) {
  const [apiError, setApiError] = useState('')
  const { register, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm<CreatePessoaDto>()

  // Preenche o formulário ao abrir para edição
  useEffect(() => {
    if (open) {
      reset(editing ? { nome: editing.nome, idade: editing.idade } : { nome: '', idade: undefined })
      setApiError('')
    }
  }, [open, editing, reset])

  const onSubmit = async (data: CreatePessoaDto) => {
    setApiError('')
    try {
      if (editing) {
        await pessoaService.update(editing.id, data)
      } else {
        await pessoaService.create(data)
      }
      onSaved()
    } catch (err: any) {
      setApiError(err.response?.data?.message ?? 'Erro ao salvar.')
    }
  }

  return (
    <Modal open={open} title={editing ? 'Editar pessoa' : 'Nova pessoa'} onClose={onClose}>
      <form onSubmit={handleSubmit(onSubmit)}>
        <FormGrid>
          {apiError && <ErrorBanner message={apiError} onClose={() => setApiError('')} />}

          <Input
            label="Nome"
            placeholder="Nome completo"
            error={errors.nome?.message}
            {...register('nome', {
              required: 'Nome é obrigatório',
              maxLength: { value: 200, message: 'Máximo 200 caracteres' },
            })}
          />
          <Input
            label="Idade"
            type="number"
            min={0}
            placeholder="Ex.: 35"
            error={errors.idade?.message}
            {...register('idade', {
              required: 'Idade é obrigatória',
              valueAsNumber: true,
              min: { value: 0, message: 'Idade não pode ser negativa' },
            })}
          />

          <FormActions>
            <Button variant="ghost" type="button" onClick={onClose}>Cancelar</Button>
            <Button type="submit" loading={isSubmitting}>
              {editing ? 'Salvar alterações' : 'Criar pessoa'}
            </Button>
          </FormActions>
        </FormGrid>
      </form>
    </Modal>
  )
}
