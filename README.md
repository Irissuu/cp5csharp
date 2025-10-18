# ElysiaAPI - CP5

API RESTful desenvolvida em .NET 8 com MongoDB, parte do projeto Elysia: Inteligência para Gestão Inteligente de Pátios da empresa Mottu.
Esta API permite o gerenciamento de motos e vagas de estacionamento, agora com integração ao MongoDB, Health Check e versionamento via Swagger, seguindo os princípios de Clean Architecture e Clean Code.

## 👥 Integrantes
Iris Tavares Alves - 557728 - 2TDSPM

Taís Tavares Alves - 557553 - 2TDSPM

## ⚙️ Tecnologias Utilizadas

```text
- ASP.NET Core 8
- MongoDB 
- Swagger com Versionamento
- Health Check 
- Clean Architecture 
- Princípios de Clean Code

```

### 1. Clone o repositório
```text
git clone https://github.com/Irissuu/cp5csharp.git
```

### 2. Instale os pacotes
```text
dotnet restore
```

### 3. Execute o projeto
```text
dotnet run
```

## 🔁 Rotas Disponíveis (via Swagger)

### 🔹 MotoController

| Método | Rota                            | Descrição                          |
|--------|----------------------------------|-------------------------------------|
| GET    | `/api/moto`                     | Lista todas as motos                |
| GET    | `/api/moto/{id}`                | Busca uma moto por ID               |
| GET    | `/api/moto/search?placa=XXX`    | Busca motos por placa (parcial)     |
| POST   | `/api/moto`                     | Cadastra uma nova moto              |
| PUT    | `/api/moto/{id}`                | Atualiza uma moto existente         |
| DELETE | `/api/moto/{id}`                | Remove uma moto                     |

### 🔹 VagaController

| Método | Rota                                | Descrição                           |
|--------|-------------------------------------|--------------------------------------|
| GET    | `/api/vaga`                         | Lista todas as vagas                 |
| GET    | `/api/vaga/{id}`                    | Busca uma vaga por ID                |
| GET    | `/api/vaga/patio?patio=XYZ`         | Lista vagas por pátio                |
| POST   | `/api/vaga`                         | Cadastra uma nova vaga               |
| PUT    | `/api/vaga/{id}`                    | Atualiza uma vaga existente          |
| DELETE | `/api/vaga/{id}`                    | Remove uma vaga                      |

---

## 🧾 Consulta no MongoDB

Para visualizar os dados diretamente no MongoDB Compass, conecte-se em:
```
mongodb://localhost:27017
```
