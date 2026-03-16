# Gastos Residenciais — Back-end

API REST em **C# .NET 8** com **SQLite** para controle de gastos residenciais.

---

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Terminal (bash, PowerShell ou CMD)

---

## Como executar

### 1. Restaurar dependências e criar o banco

```bash
# Na raiz da solução
cd GastosResidenciais

dotnet restore

# Instalar a ferramenta de migrations do EF Core (uma vez por máquina)
dotnet tool install --global dotnet-ef

# Criar a migration inicial (caso não exista)
dotnet ef migrations add InitialCreate --project GastosResidenciais.Infrastructure --startup-project GastosResidenciais.API

# Aplicar a migration (cria o arquivo .db do SQLite)
dotnet ef database update --project GastosResidenciais.Infrastructure --startup-project GastosResidenciais.API
```

> **Nota:** As migrations também são aplicadas automaticamente ao iniciar a API (`db.Database.Migrate()` no `Program.cs`).

### 2. Rodar a API

```bash
cd GastosResidenciais.API
dotnet run
```

A API estará disponível em:
- `http://localhost:5000`
- Swagger UI: `http://localhost:5000/swagger`

---

## Estrutura do projeto

```
GastosResidenciais/
├── GastosResidenciais.Domain/          # Entidades e Enums (sem dependências)
├── GastosResidenciais.Application/     # Interfaces, Services, DTOs
├── GastosResidenciais.Infrastructure/  # EF Core, Repositórios, Migrations
└── GastosResidenciais.API/             # Controllers, Validators, Middleware, Program.cs
```

## Endpoints principais

| Método | Rota                              | Descrição                              |
|--------|-----------------------------------|----------------------------------------|
| GET    | /api/pessoas                      | Listar pessoas                         |
| POST   | /api/pessoas                      | Criar pessoa                           |
| PUT    | /api/pessoas/{id}                 | Editar pessoa                          |
| DELETE | /api/pessoas/{id}                 | Deletar pessoa + transações (cascade)  |
| GET    | /api/categorias                   | Listar categorias                      |
| GET    | /api/categorias/compativeis?tipo= | Categorias filtradas por tipo          |
| POST   | /api/categorias                   | Criar categoria                        |
| GET    | /api/transacoes                   | Listar transações                      |
| POST   | /api/transacoes                   | Criar transação (valida regras)        |
| GET    | /api/relatorios/pessoas           | Totais por pessoa + geral              |
| GET    | /api/relatorios/categorias        | Totais por categoria + geral           |

## Regras de negócio

1. **Menor de 18 anos** → apenas transações do tipo `Despesa` são aceitas
2. **Categoria × Tipo** → categoria com `Finalidade=Receita` não pode ser usada em `Despesa` e vice-versa
3. **Deleção em cascata** → deletar uma pessoa remove todas suas transações
4. **Valor positivo** → transações devem ter valor > 0

## Códigos HTTP retornados

| Código | Situação                                        |
|--------|-------------------------------------------------|
| 200    | Sucesso com conteúdo                            |
| 201    | Recurso criado                                  |
| 204    | Sucesso sem conteúdo (DELETE)                   |
| 400    | Dados inválidos (FluentValidation)              |
| 404    | Recurso não encontrado                          |
| 422    | Violação de regra de negócio                    |
| 500    | Erro interno                                    |
