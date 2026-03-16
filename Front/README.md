# Gastos Residenciais — Front-end

Interface React + TypeScript para o sistema de controle de gastos residenciais.

## Pré-requisitos

- Node.js 18+
- API .NET 8 rodando em `http://localhost:5000`

## Como rodar

```bash
npm install
npm run dev
```

Acesse: **http://localhost:5173**

## Estrutura

```
src/
├── components/
│   ├── Layout/     # AppLayout + Sidebar (navegação)
│   └── UI/         # Button, Input, Select, Modal, Card, Badge, etc.
├── pages/
│   ├── Pessoas/         # CRUD de pessoas
│   ├── Categorias/      # Criar + listar categorias
│   ├── Transacoes/      # Criar + listar transações
│   └── Relatorios/      # Totais por pessoa e por categoria
├── services/       # Chamadas à API via Axios
├── types/          # Interfaces TypeScript (espelham DTOs do backend)
└── router/         # React Router v6
```

## Regras de negócio no front-end

- **Menor de idade:** ao selecionar uma pessoa < 18 anos no formulário de transação,
  o tipo é automaticamente forçado para "Despesa" e o campo fica desabilitado.
- **Categorias filtradas:** o select de categorias recarrega automaticamente
  ao mudar o tipo de transação, mostrando apenas categorias compatíveis.
- **Erros da API:** mensagens de erro retornadas pelo backend (HTTP 422, 404)
  são exibidas diretamente no formulário via `ErrorBanner`.
