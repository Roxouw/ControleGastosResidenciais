// ── Enums ─────────────────────────────────────────────────────────────────────
// Espelham os enums do backend. A API serializa como string (JsonStringEnumConverter),
// então usamos string literal types aqui para segurança de tipos.

export type TipoTransacao = 'Despesa' | 'Receita'
export type FinalidadeCategoria = 'Despesa' | 'Receita' | 'Ambas'

// ── Pessoa ────────────────────────────────────────────────────────────────────

export interface Pessoa {
  id: string
  nome: string
  idade: number
}

export interface CreatePessoaDto {
  nome: string
  idade: number
}

export interface UpdatePessoaDto {
  nome: string
  idade: number
}

// ── Categoria ─────────────────────────────────────────────────────────────────

export interface Categoria {
  id: string
  descricao: string
  finalidade: FinalidadeCategoria
}

export interface CreateCategoriaDto {
  descricao: string
  finalidade: FinalidadeCategoria
}

// ── Transacao ─────────────────────────────────────────────────────────────────

export interface Transacao {
  id: string
  descricao: string
  valor: number
  tipo: TipoTransacao
  categoriaId: string
  categoriaNome: string
  pessoaId: string
  pessoaNome: string
}

export interface CreateTransacaoDto {
  descricao: string
  valor: number
  tipo: TipoTransacao
  categoriaId: string
  pessoaId: string
}

// ── Relatório ─────────────────────────────────────────────────────────────────

export interface Totais {
  totalReceitas: number
  totalDespesas: number
  saldo: number
}

export interface RelatorioPessoa {
  pessoaId: string
  pessoaNome: string
  pessoaIdade: number
  totalReceitas: number
  totalDespesas: number
  saldo: number
}

export interface RelatorioCategoria {
  categoriaId: string
  categoriaDescricao: string
  totalReceitas: number
  totalDespesas: number
  saldo: number
}

export interface RelatorioResumoPessoas {
  pessoas: RelatorioPessoa[]
  totaisGerais: Totais
}

export interface RelatorioResumoCategoria {
  categorias: RelatorioCategoria[]
  totaisGerais: Totais
}

// ── Erro da API ───────────────────────────────────────────────────────────────

export interface ApiError {
  status: number
  error: string
  message: string
}
