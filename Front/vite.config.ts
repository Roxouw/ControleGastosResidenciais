import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

/**
 * Configuração do Vite.
 * O proxy redireciona todas as chamadas /api/* para a API .NET em :5000,
 * evitando problemas de CORS em desenvolvimento e permitindo usar
 * caminhos relativos no código do front-end.
 */
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://localhost:5000',
        changeOrigin: true,
      },
    },
  },
})
