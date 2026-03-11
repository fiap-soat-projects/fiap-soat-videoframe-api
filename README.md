# fiap-soat-videoframe-api

Objetivo
- API para upload, gerenciamento e edição de vídeos com extração/serviço de download de arquivos. Destinada ao projeto FIAP SOAT; usa autenticação JWT (Cognito) e a lib Scalar para exposição da referência de API (em vez do Swagger).

Como executar
- Requisitos: .NET 10 SDK
- Exemplo:
  dotnet restore
  dotnet build
  dotnet run --project src/Drivers/Api

Autenticação
- JWT Bearer (Cognito). Enviar header:
  Authorization: Bearer <token>

Middlewares relevantes
- `ErrorHandlingMiddleware` — tratamento centralizado de erros.
- `FileTypeValidationMiddleware` — validação do tipo de arquivo em uploads.
- `RequestSizeLimit(10485760)` aplicado no endpoint de upload (10 MB).

Endpoints (extraídos do código)
- Base comum: v1/user/videos

1) Upload de vídeo
- Método: POST
- Rota: `POST /v1/user/videos`
- Autorização: Obrigatória
- Headers:
  - `fileName` (nome do arquivo)
  - `Content-Type` (tipo do arquivo, enviado pelo cliente)
- Body: Stream bruto do arquivo (conteúdo do request)
- Observações:
  - Limite configurado: 10 MB.
  - Retorna `200 OK` com o `id` do vídeo (string) no corpo.

Exemplo (curl):
curl -X POST "https://{host}/v1/user/videos" \
  -H "Authorization: Bearer <token>" \
  -H "fileName: exemplo.mp4" \
  -H "Content-Type: video/mp4" \
  --data-binary @exemplo.mp4

2) Obter link do vídeo
- Método: GET
- Rota: `GET /v1/user/videos/{id}/link`
- Autorização: Obrigatória
- Retorno: JSON com link ou dados apresentados pelo presenter.

3) Download de vídeo (arquivo)
- Método: GET
- Rota: `GET /v1/user/videos/{id}/download`
- Autorização: Obrigatória
- Comportamento: Stream do arquivo com `Content-Type` e `Content-Disposition` definidos; arquivo enviado como attachment.

Exemplo (curl):
curl -X GET "https://{host}/v1/user/videos/{id}/download" \
  -H "Authorization: Bearer <token>" -o video.mp4

4) Listagem paginada de vídeos
- Método: GET
- Rota: `GET /v1/user/videos`
- Autorização: Obrigatória
- Query: parâmetros de paginação via `PaginationRequest` (query string)
- Retorno: ViewModel com paginação.

5) Recuperar vídeo por id (metadados)
- Método: GET
- Rota: `GET /v1/user/videos/{id}`
- Autorização: Obrigatória
- Retorno: ViewModel com dados do vídeo.

Endpoints de edição (editions)
- Base: `v1/user/videos/edits`

6) Listagem paginada de edições
- Método: GET
- Rota: `GET /v1/user/videos/edits`
- Autorização: Obrigatória
- Query: `PaginationRequest`
- Retorno: ViewModel paginado.

7) Criar edição
- Método: POST
- Rota: `POST /v1/user/videos/edits`
- Autorização: Obrigatória
- Body: `CreateVideoEditRequest` (JSON)
- Retorno: `200 OK` com o `id` da edição.

8) Iniciar edição
- Método: POST
- Rota: `POST /v1/user/videos/edits/{id}/start`
- Autorização: Obrigatória
- Ação: inicia processamento/edição; retorna `204 No Content`.

9) Atualizar status da edição
- Método: PATCH
- Rota: `PATCH /v1/user/videos/edits/{id}/status`
- Autorização: Uso Interno sem autenticação
- Body: `UpdateEditionStatusRequest` (contém `userId` e `Status`)
- Ação: atualiza status da edição; retorna `204 No Content`.

10) Download de edição (resultado)
- Método: GET
- Rota: `GET /v1/user/videos/edits/{id}/download`
- Autorização: Obrigatória
- Comportamento: stream de arquivo com `Content-Type` e `Content-Disposition`.

UI de referência (Scalar)
- Observação: O Scalar foi usado em lugar do Swagger/OpenAPI
  - Rode a API localmente e acesse a raiz/paths comuns (ex.: `/scalar`, `/scalar-api`, `/openapi`) ou verifique os logs de inicialização.

Observações finais
- Todos os endpoints principais exigem JWT salvo exceção apontada no PATCH de status que será de uso interno.
- Upload usa stream direto no corpo do request — enviar `fileName` por header.
- Valide tipos de arquivo e tamanhos conforme middleware existente.