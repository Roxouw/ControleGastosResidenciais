import axios from 'axios'
import type {
  Pessoa, CreatePessoaDto, UpdatePessoaDto,
  Categoria, CreateCategoriaDto, TipoTransacao,
  Transacao, CreateTransacaoDto,
  RelatorioResumoPessoas, RelatorioResumoCategoria,
} from '../types'

/**
 * Instância central do Axios.
 * Todas as chamadas HTTP passam por aqui.
 * O proxy do Vite (vite.config.ts) redireciona /api → localhost:5000,
 * então não precisamos repetir a URL base em cada chamada.
 */
const api = axios.create({
  baseURL: '/api',
  headers: { 'Content-Type': 'application/json' },
})

// ── Pessoas ───────────────────────────────────────────────────────────────────

export const pessoaService = {
  /** Busca todas as pessoas cadastradas. */
  getAll: () => api.get<Pessoa[]>('/pessoas').then(r => r.data),

  /** Cria uma nova pessoa. */
  create: (dto: CreatePessoaDto) =>
    api.post<Pessoa>('/pessoas', dto).then(r => r.data),

  /** Atualiza os dados de uma pessoa existente. */
  update: (id: string, dto: UpdatePessoaDto) =>
    api.put<Pessoa>(`/pessoas/${id}`, dto).then(r => r.data),

  /** Deleta uma pessoa e suas transações (cascade no backend). */
  delete: (id: string) => api.delete(`/pessoas/${id}`),
}

// ── Categorias ────────────────────────────────────────────────────────────────

export const categoriaService = {
  /** Busca todas as categorias. */
  getAll: () => api.get<Categoria[]>('/categorias').then(r => r.data),

  /**
   * Busca categorias filtradas por compatibilidade com o tipo de transação.
   * Usado no formulário de transação para restringir as opções do select.
   */
  getCompatíveis: (tipo: TipoTransacao) =>
    api.get<Categoria[]>(`/categorias/compativeis?tipo=${tipo}`).then(r => r.data),

  /** Cria uma nova categoria. */
  create: (dto: CreateCategoriaDto) =>
    api.post<Categoria>('/categorias', dto).then(r => r.data),
}

// ── Transações ────────────────────────────────────────────────────────────────

export const transacaoService = {
  /** Busca todas as transações com dados de pessoa e categoria. */
  getAll: () => api.get<Transacao[]>('/transacoes').then(r => r.data),

  /**
   * Cria uma transação. O backend valida:
   * - Menor de 18 anos → apenas Despesa
   * - Categoria compatível com o tipo
   */
  create: (dto: CreateTransacaoDto) =>
    api.post<Transacao>('/transacoes', dto).then(r => r.data),
}

// ── Relatórios ────────────────────────────────────────────────────────────────

export const relatorioService = {
  /** Relatório de totais agrupados por pessoa com total geral. */
  getPorPessoa: () =>
    api.get<RelatorioResumoPessoas>('/relatorios/pessoas').then(r => r.data),

  /** Relatório de totais agrupados por categoria com total geral. */
  getPorCategoria: () =>
    api.get<RelatorioResumoCategoria>('/relatorios/categorias').then(r => r.data),
}

export default api
