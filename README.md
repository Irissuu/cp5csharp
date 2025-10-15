# ElysiaAPI - CP5

API RESTful desenvolvida em .NET 8 com Entity Framework Core e Oracle, parte do projeto **Elysia: Inteligência para Gestão Inteligente de Pátios** da empresa Mottu. Esta API permite o gerenciamento de **motos** e **vagas de estacionamento**, com foco em uma solução inteligente para controle de pátios.

## 👥 Integrantes
Iris Tavares Alves - 557728 - 2TDSPM

Taís Tavares Alves - 557553 - 2TDSPM

## ⚙️ Tecnologias Utilizadas

```text
- ASP.NET Core 8
- Entity Framework Core
- Oracle Database
- Swagger (OpenAPI)
- Clean Architecture (camadas Domain, Infrastructure, Application)
- MongoDB
- Health Check
```

### 1. Clone o repositório
```text
git clone https://github.com/Irissuu/cp5csharp.git
```

### 2. Configure a string de conexão
```text
"ConnectionStrings": {
  "OracleDB": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/orcl;"
}
```

### 3. Instale os pacotes
```text
dotnet restore
```

### 4. Gere o banco de dados com EF Core
```text
dotnet ef migrations add Inicial
dotnet ef database update
```

### 5. Execute o projeto
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

## 🧾 Consulta no banco Oracle

Para visualizar os dados diretamente no Oracle SQL Developer, use **aspas nos nomes das tabelas**:

```sql
SELECT * FROM "MotoCsharp";
SELECT * FROM "VagaCsharp";

