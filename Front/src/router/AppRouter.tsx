import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { AppLayout } from '../components/Layout/AppLayout'
import PessoasPage from '../pages/Pessoas/PessoasPage'
import CategoriasPage from '../pages/Categorias/CategoriasPage'
import TransacoesPage from '../pages/Transacoes/TransacoesPage'
import RelatorioPessoasPage from '../pages/Relatorios/RelatorioPessoasPage'
import RelatorioCategoriasPage from '../pages/Relatorios/RelatorioCategoriasPage'

/**
 * Roteador principal da aplicação.
 * Usa React Router v6 com layout aninhado:
 * - AppLayout envolve todas as rotas e renderiza a sidebar
 * - Cada rota filho renderiza no <Outlet /> do AppLayout
 * - A rota raiz "/" redireciona para "/pessoas"
 */
export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<AppLayout />}>
          <Route index element={<Navigate to="/pessoas" replace />} />
          <Route path="pessoas"    element={<PessoasPage />} />
          <Route path="categorias" element={<CategoriasPage />} />
          <Route path="transacoes" element={<TransacoesPage />} />
          <Route path="relatorios/pessoas"    element={<RelatorioPessoasPage />} />
          <Route path="relatorios/categorias" element={<RelatorioCategoriasPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}
