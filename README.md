# 💰 Controle de Gastos Residenciais

Sistema para **gestão de despesas domésticas**, permitindo registrar, organizar e acompanhar os gastos da residência de forma simples e estruturada.

O projeto utiliza uma arquitetura moderna com **API em .NET** e **frontend em React**, separados em camadas para facilitar manutenção e evolução.

---

# 🚀 Tecnologias Utilizadas

### Backend

* C#
* .NET
* ASP.NET Core
* Entity Framework
* Clean Architecture

### Frontend

* React
* Vite
* TypeScript
* HTML / CSS

---

# 🏗 Arquitetura do Projeto

O backend segue o padrão **Clean Architecture**, separando responsabilidades em diferentes camadas:

```
Back/
 ├── GastosResidenciais.API
 ├── GastosResidenciais.Application
 ├── GastosResidenciais.Domain
 └── GastosResidenciais.Infrastructure
```

### Camadas

**Domain**

* Entidades
* Regras de negócio

**Application**

* Casos de uso
* Serviços da aplicação

**Infrastructure**

* Persistência de dados
* Integrações externas

**API**

* Endpoints REST
* Controllers

---

# 🎨 Frontend

Estrutura do frontend desenvolvido com React + Vite:

```
Front/
 ├── src
 ├── index.html
 ├── vite.config.ts
 ├── package.json
```

O frontend consome a API através de chamadas HTTP para os endpoints da aplicação.

Durante o desenvolvimento o **Vite proxy** redireciona chamadas `/api` para o backend.

---

# ⚙️ Como Executar o Projeto

## 1️⃣ Clonar o repositório

```bash
git clone https://github.com/Roxouw/ControleGastosResidenciais.git
cd ControleGastosResidenciais
```

---

# 🔧 Rodar o Backend

Entre na pasta do backend:

```bash
cd Back
```

Execute a API:

```bash
dotnet run --project GastosResidenciais.API
```

API disponível em:

```
http://localhost:5000
```

---

# 💻 Rodar o Frontend

Entre na pasta do frontend:

```bash
cd Front
```

Instale as dependências:

```bash
npm install
```

Execute o projeto:

```bash
npm run dev
```

Aplicação disponível em:

```
http://localhost:5173
```

---

# 📌 Funcionalidades

* Cadastro de gastos
* Listagem de despesas
* Organização de despesas da residência
* Integração frontend + API

---

# 📚 Objetivo do Projeto

Este projeto foi desenvolvido para:

* praticar **arquitetura limpa**
* aplicar **boas práticas de backend em .NET**
* integrar **API REST com frontend moderno**
* servir como **projeto de estudo e portfólio**

---

# 👨‍💻 Autor

**Filipe Rosso**
Desenvolvedor Full Stack

---

# ⭐ Melhorias Futuras

* Autenticação de usuários
* Dashboard de gastos
* Gráficos financeiros
* Persistência com banco de dados
* Deploy em cloud

