# 🎬 VideoFrame API

Este repositório contém a API de upload e edição de vídeos desenvolvida para o hackathon FIAP SOAT. O serviço expõe endpoints para upload, gerenciamento e processamento de vídeos (ex.: extração de frames), além de download dos resultados editados. Utiliza autenticação JWT via AWS Cognito e arquitetura orientada a eventos com Kafka.

## 🏃 Integrantes do grupo

- **Jeferson dos Santos Gomes** - RM 362669
- **Jamison dos Santos Gomes** - RM 362671
- **Alison da Silva Cruz** - RM 362628

## 👨‍💻 Tecnologias Utilizadas

- **.NET 10 (C# 14)** – API construída com ASP.NET Core
- **MongoDB** – banco de dados NoSQL para persistência de vídeos e edições
- **AWS S3 / MinIO** – armazenamento de objetos (vídeos e arquivos editados)
- **Apache Kafka** – mensageria para processamento assíncrono de edições
- **AWS Cognito** – autenticação e autorização via JWT
- **Docker & Docker Compose** – containerização e orquestração local
- **Prometheus** – métricas
- **Serilog** – logging estruturado
- **Scalar (documentação)** – para explorar e testar endpoints (substituto do Swagger)

## 🔌 Endpoints Disponíveis

### 🎥 Vídeos (`/v1/user/videos`)

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/v1/user/videos` | Upload de um novo vídeo (stream no body, `fileName` via header) |
| `GET` | `/v1/user/videos` | Listagem paginada de vídeos do usuário |
| `GET` | `/v1/user/videos/{id}` | Obtém metadados de um vídeo por ID |
| `GET` | `/v1/user/videos/{id}/link` | Obtém link de acesso ao vídeo |
| `GET` | `/v1/user/videos/{id}/download` | Download do arquivo de vídeo |

### ✂️ Edições de Vídeo (`/v1/user/videos/edits`)

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/v1/user/videos/edits` | Listagem paginada de edições do usuário |
| `POST` | `/v1/user/videos/edits` | Cria uma nova edição de vídeo |
| `POST` | `/v1/user/videos/edits/{id}/start` | Inicia o processamento da edição |
| `PATCH` | `/v1/user/videos/edits/{id}/status` | Atualiza o status da edição (uso interno) |
| `GET` | `/v1/user/videos/edits/{id}/download` | Download do resultado da edição |

> **Nota:** Todos os endpoints exigem autenticação JWT (`Authorization: Bearer <token>`), exceto o `PATCH` de status que é de uso interno.

## 🌐 Variáveis de Ambiente

A API utiliza as seguintes variáveis para configuração:

| Variável | Descrição |
|----------|-----------|
| `ASPNETCORE_ENVIRONMENT` | Define o ambiente em que a aplicação está rodando (`Development`, `Staging`, `Production`) |
| `VideoframeMongoDbConnectionString` | String de conexão para o banco MongoDB |
| `S3BucketBaseUrl` | URL base do bucket S3 / MinIO para armazenamento de vídeos |
| `S3BucketName` | Nome do bucket S3 utilizado |
| `S3BucketUser` | Usuário de acesso ao bucket S3 / MinIO |
| `S3BucketPassword` | Senha de acesso ao bucket S3 / MinIO |
| `KafkaConnectionString` | String de conexão para o broker Kafka |
| `KafkaProduceTopicName` | Nome do tópico Kafka usado para publicar eventos de edição |
| `AppName` | Nome da aplicação (usado em logs e métricas) |
| `AWS_CLIENT_ID` | Identificador do cliente usado na integração com AWS Cognito |
| `AWS_CLIENT_SECRET` | Segredo associado ao `AWS_CLIENT_ID` para autenticação junto ao Cognito |
| `AWS_USER_POOL_ID` | ID do pool de usuários Cognito que armazena os usuários do sistema |
| `AWS_REGION` | Região dos recursos AWS |
| `AWS_ACCESS_KEY_ID` | Chave de acesso da conta ou role AWS utilizada para operações programáticas |
| `AWS_SECRET_ACCESS_KEY` | Segredo correspondente à `AWS_ACCESS_KEY_ID` utilizado para autenticação AWS |

## 👤 Convenções

- Todas as requisições e respostas utilizam **JSON** (exceto upload/download de arquivos que usam stream).
- Utilize um cliente HTTP (Postman, curl, Scalar etc.) para testar os endpoints.
- Configurações de ambiente estão em `appsettings.json` / `appsettings.Development.json`.
- Upload de vídeo usa stream direto no corpo do request — enviar `fileName` por header.
- Middlewares de validação (`FileTypeValidationMiddleware`) garantem que apenas tipos de arquivo permitidos sejam aceitos.
- Erros são tratados de forma centralizada pelo `ErrorHandlingMiddleware`.

## 🏦 Banco de Dados

O serviço utiliza **MongoDB** como banco de dados. As entidades principais seguem as estruturas abaixo:

### Video

| Campo | Tipo |
|-------|------|
| Id | string |
| CreatedAt | datetime |
| UserId | string |
| Path | string |
| Name | string |
| ContentType | string |
| ContentLength | long |

### VideoEdit

| Campo | Tipo |
|-------|------|
| Id | string |
| CreatedAt | datetime |
| UserId | string |
| Recipient | string |
| Type | EditType (`None`, `Frame`) |
| Status | EditStatus (`None`, `Created`, `Processing`, `Processed`, `Error`) |
| VideoId | string |
| EditPath | string |
| NotificationTargets | lista de NotificationTarget |

## 🐳 Como Executar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/)
- [Docker & Docker Compose](https://www.docker.com/)

### Rodando localmente (sem Docker)

```bash
dotnet restore
dotnet build
dotnet run --project src/Drivers/Api
```

### Rodando com Docker Compose

```bash
docker-compose up --build
```

Isso irá subir a API junto com as dependências: **Kafka**, **MongoDB**, **MinIO** e **Kafka UI**.

| Serviço | Porta |
|---------|-------|
| VideoFrame API | `8080` / `8081` |
| Kafka | `9092` |
| Kafka UI | `8083` |
| MongoDB | `27017` |
| MinIO (API) | `9000` |
| MinIO (Console) | `9001` |

## 📖 Documentação Interativa (Scalar)

Rode a API localmente e acesse `/scalar/v1` para explorar e testar os endpoints via interface gráfica.