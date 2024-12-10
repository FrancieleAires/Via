# VIA - Transporte Sobre Trilhos em Tempo Real 🚆  

Bem-vindo ao repositório do **VIA**, um aplicativo inovador que oferece uma experiência em tempo real para usuários de transporte público sobre trilhos. Este projeto foi desenvolvido como parte do meu TCC, onde fui responsável por toda a implementação do backend.  

---

## 🌟 Funcionalidades  
- **Rastreamento em tempo real:** Localização dos trens atualizada dinamicamente.  
- **Consulta de pontos turísticos:** Acesse informações detalhadas de cada local, incluindo uma rota sugerida que pode ser rastreada em tempo real.  
- **Autenticação segura:** Gerenciamento de contas de usuários com autenticação JWT.  
- **Envio de feedbacks:** Usuários podem enviar feedbacks diretamente pelo aplicativo.  

---

## 🛠️ Tecnologias Utilizadas  
O backend foi desenvolvido com tecnologias modernas e escaláveis:  
- **ASP.NET Core**  
- **.NET 8**  
- **C#**  
- **SignalR** (atualizações em tempo real com suporte a WebSockets)  
- **JWT** para autenticação e autorização  
- **Entity Framework Core** para mapeamento de dados  
- **MySQL** como banco de dados  
- **Elastic Email** para envio de e-mails (confirmação e recuperação de senha)  
- **IHostedService** para tarefas em segundo plano, como movimentação dos trens  
- **Swagger** para documentação e testes da API  

---

## 🚀 Como Executar  

### Pré-requisitos  
- .NET 8 SDK  
- Banco de dados MySQL configurado  
- Ferramenta para requisições HTTP (ex.: Postman ou cURL)  

### Passos  
1. Clone o repositório:  
   ```bash
   git clone https://github.com/FrancieleAires/VIA.git
