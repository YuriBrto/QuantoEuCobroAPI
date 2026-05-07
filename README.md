Quanto Eu Cobro API

API backend do sistema Quanto Eu Cobro, uma plataforma SaaS para criação e gerenciamento de propostas comerciais profissionais com suporte a templates dinâmicos, campos personalizados e visualização pública de propostas.

🚀 Visão do Produto

O sistema permite que freelancers e agências:

Criem propostas comerciais profissionais em minutos
Personalizem campos dinamicamente
Escolham templates visuais modernos
Compartilhem propostas via link público
Acompanhem visualizações e métricas
🧠 Diferencial da Plataforma

🔥 Templates dinâmicos de propostas

Cada proposta pode ser renderizada com diferentes estilos visuais, transformando documentos comuns em experiências semelhantes a:

Landing pages modernas
Portfólios interativos
Propostas estilo Apple / Stripe / Notion
⚙️ Stack Utilizada
ASP.NET Core (.NET 9)
Entity Framework Core
SQLite (desenvolvimento)
JWT Authentication
Swagger (OpenAPI)
Arquitetura em camadas (Clean Architecture simplificada)
🧱 Arquitetura do Projeto
QuantoEuCubro.API
│
├── Authentication
├── Controllers
├── Domain
│   ├── DTOs
│   ├── Entities
│   └── Enums
│
├── Infrastructure
├── Services
│   ├── Interfaces
│   └── Implementações
│
├── Migrations
├── Middlewares
├── Extensions
└── Program.cs
🔐 Autenticação

A API utiliza JWT (JSON Web Token) para autenticação.

Fluxo:
Usuário realiza login
API retorna token JWT
Token é enviado via header:
Authorization: Bearer {token}
📡 Endpoints Principais
🔐 Auth
Registrar usuário
POST /api/auth/register
Login
POST /api/auth/login
📄 Propostas
Listar propostas
GET /api/propostas
Obter proposta por ID
GET /api/propostas/{id}
Criar proposta
POST /api/propostas
Atualizar proposta
PUT /api/propostas/{id}
Excluir proposta (soft delete)
DELETE /api/propostas/{id}
Resumo do dashboard
GET /api/propostas/resumo
Visualização pública de proposta
GET /api/propostas/preview/{token}
🎨 Templates
Listar templates
GET /api/templates
Obter template por ID
GET /api/templates/{id}
🌐 Funcionalidade de Visualização Pública

Cada proposta possui um PublicToken, permitindo acesso público via URL:

/api/propostas/preview/{token}

✔ Incrementa visualizações automaticamente
✔ Registra última visualização
✔ Renderização baseada em template

📊 Funcionalidades do Sistema
✔ Autenticação JWT
✔ CRUD completo de propostas
✔ Campos dinâmicos personalizados
✔ Sistema de templates visuais
✔ Soft delete de propostas
✔ Tracking de visualizações
✔ Dashboard com métricas
✔ Preview público de propostas
🧩 Modelo de Proposta

Uma proposta contém:

Dados do cliente
Valor total
Campos personalizados
Template selecionado
Token público
Métricas de visualização
📈 Dashboard

A API fornece métricas como:

Total de propostas
Propostas visualizadas
Propostas criadas no mês
Faturamento estimado
🔒 Segurança
JWT obrigatório para endpoints protegidos
Validação de ownership por usuário
Soft delete para preservação de histórico
Middleware global de tratamento de exceções
🧪 Swagger

A documentação da API está disponível via Swagger:

/swagger
⚙️ Como executar o projeto
1. Clonar repositório
git clone https://github.com/YuriBrto/QuantoEuCobroAPI.git
2. Restaurar dependências
dotnet restore
3. Rodar migrations
dotnet ef database update
4. Executar API
dotnet run
📦 Configuração

Exemplo de appsettings.json:

{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=quantoeucubro.db"
  },
  "Jwt": {
    "Chave": "SUA_CHAVE_SECRETA",
    "Issuer": "QuantoEuCobroAPI",
    "Audience": "QuantoEuCobroFrontend"
  }
}
🧠 Status do Projeto

✔ Backend funcional
✔ Autenticação JWT implementada
✔ Sistema de propostas completo
✔ Templates base implementados
✔ API pronta para integração com frontend Angular

🚀 Próximos passos
Frontend Angular (SaaS UI)
Editor visual de propostas
Sistema de templates avançado
Exportação PDF profissional
Deploy em cloud (Azure / AWS / Render)
👨‍💻 Autor

Projeto desenvolvido como SaaS de portfólio:

Quanto Eu Cobro API
