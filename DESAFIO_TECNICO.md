# Desafio técnico — Estágio em Desenvolvimento .NET

> Documento de requisitos do projeto **TechHelpSystem**. O desafio é fictício, mas foi estruturado no formato utilizado em processos seletivos reais para estágio em desenvolvimento .NET.

## 1. Objetivo

Desenvolver uma API para controlar chamados internos de suporte.

Atualmente, os funcionários enviam problemas por mensagens, dificultando saber quais chamados estão abertos, em atendimento ou concluídos. A API deverá permitir o cadastro dos solicitantes e o gerenciamento dos chamados.

## 2. Prazo sugerido

- 5 dias corridos.
- Aproximadamente 6 a 10 horas de desenvolvimento.
- É permitido consultar documentações.
- Os requisitos opcionais não são necessários para concluir o desafio.

## 3. Tecnologias obrigatórias

- C#.
- .NET 8 ou superior.
- ASP.NET Core Web API.
- Entity Framework Core.
- Banco de dados relacional: SQLite, SQL Server ou PostgreSQL.
- Git para versionamento.

Não é necessário desenvolver um front-end.

## 4. Entidades

### 4.1. Solicitante

Representa o funcionário que abriu um chamado.

| Campo | Tipo | Regras |
| --- | --- | --- |
| `Id` | inteiro | Gerado pelo sistema |
| `Nome` | texto | Obrigatório; entre 3 e 100 caracteres |
| `Email` | texto | Obrigatório; formato válido e valor único |
| `CriadoEm` | data e hora | Preenchido pelo sistema |

### 4.2. Chamado

| Campo | Tipo | Regras |
| --- | --- | --- |
| `Id` | inteiro | Gerado pelo sistema |
| `Titulo` | texto | Obrigatório; entre 5 e 100 caracteres |
| `Descricao` | texto | Obrigatório; entre 10 e 1.000 caracteres |
| `Status` | enumeração | `Aberto`, `EmAtendimento` ou `Concluido` |
| `Prioridade` | enumeração | `Baixa`, `Media` ou `Alta` |
| `SolicitanteId` | inteiro | Deve identificar um solicitante existente |
| `CriadoEm` | data e hora | Preenchido pelo sistema |
| `AtualizadoEm` | data e hora | Atualizado quando o chamado for alterado |

## 5. Requisitos funcionais

### RF01 — Cadastrar solicitante

```http
POST /api/solicitantes
```

Exemplo de entrada:

```json
{
  "nome": "Maria Oliveira",
  "email": "maria@empresa.com"
}
```

Regras:

- Não aceitar nome ou e-mail inválidos.
- Não permitir e-mails duplicados, inclusive quando diferirem apenas por letras maiúsculas ou minúsculas.
- Em caso de sucesso, retornar `201 Created`.
- Para e-mail já cadastrado, retornar `409 Conflict`.
- Para dados inválidos, retornar `400 Bad Request`.
- A resposta de sucesso deve conter o solicitante criado.
- O cabeçalho `Location` deve apontar para a consulta do novo solicitante.

### RF02 — Listar solicitantes

```http
GET /api/solicitantes
```

- Retornar `200 OK` com todos os solicitantes.
- Quando não houver registros, retornar uma coleção vazia.

### RF03 — Buscar solicitante por ID

```http
GET /api/solicitantes/{id}
```

- Retornar `200 OK` quando o solicitante existir.
- Retornar `404 Not Found` quando não existir.

### RF04 — Abrir chamado

```http
POST /api/chamados
```

Exemplo de entrada:

```json
{
  "titulo": "Computador não inicia",
  "descricao": "Ao pressionar o botão, o computador não apresenta nenhum sinal.",
  "prioridade": "Alta",
  "solicitanteId": 1
}
```

Regras:

- O solicitante informado deve existir.
- Todo chamado novo deve começar com status `Aberto`.
- `CriadoEm` e `AtualizadoEm` devem ser preenchidos pela aplicação.
- Em caso de sucesso, retornar `201 Created`.
- Para dados inválidos, retornar `400 Bad Request`.
- Se o solicitante não existir, retornar `404 Not Found`.

### RF05 — Listar e filtrar chamados

```http
GET /api/chamados
GET /api/chamados?status=Aberto
GET /api/chamados?prioridade=Alta
GET /api/chamados?status=Aberto&prioridade=Alta
```

Regras:

- Os filtros `status` e `prioridade` são opcionais.
- Os filtros podem ser combinados.
- Retornar `200 OK` com os registros encontrados.
- Quando não houver registros, retornar uma coleção vazia.
- Cada item deve apresentar também o identificador, nome e e-mail do solicitante.

Exemplo de resposta:

```json
[
  {
    "id": 1,
    "titulo": "Computador não inicia",
    "status": "Aberto",
    "prioridade": "Alta",
    "solicitante": {
      "id": 1,
      "nome": "Maria Oliveira",
      "email": "maria@empresa.com"
    },
    "criadoEm": "2026-09-23T14:30:00Z",
    "atualizadoEm": "2026-09-23T14:30:00Z"
  }
]
```

### RF06 — Buscar chamado por ID

```http
GET /api/chamados/{id}
```

- Retornar `200 OK` quando o chamado existir.
- Retornar `404 Not Found` quando não existir.
- A resposta deve apresentar os dados do solicitante.

### RF07 — Alterar o status do chamado

```http
PATCH /api/chamados/{id}/status
```

Exemplo de entrada:

```json
{
  "status": "EmAtendimento"
}
```

Transições permitidas:

```text
Aberto -> EmAtendimento -> Concluido
```

Não é permitido:

- Voltar um chamado para um status anterior.
- Passar diretamente de `Aberto` para `Concluido`.
- Alterar um chamado que já esteja concluído.

Respostas esperadas:

- `200 OK` ou `204 No Content` para uma alteração válida.
- `400 Bad Request` para uma transição inválida.
- `404 Not Found` quando o chamado não existir.

### RF08 — Excluir chamado

```http
DELETE /api/chamados/{id}
```

Regras:

- Somente chamados com status `Aberto` podem ser excluídos.
- Em caso de sucesso, retornar `204 No Content`.
- Retornar `400 Bad Request` quando o status não permitir a exclusão.
- Retornar `404 Not Found` quando o chamado não existir.

## 6. Requisitos técnicos

- Persistir os dados em banco de dados relacional.
- Criar e utilizar migrations do Entity Framework Core.
- Validar os dados recebidos pela API.
- Utilizar corretamente os códigos HTTP.
- Utilizar operações assíncronas nas consultas e gravações no banco.
- Disponibilizar a documentação dos endpoints com OpenAPI/Swagger.
- Manter separadas as representações de entrada, saída e persistência quando necessário.
- Não expor detalhes internos ou rastreamentos de exceção nas respostas da API.
- A aplicação deve compilar e executar sem alterações manuais no código.
- Não armazenar senhas, chaves ou credenciais reais no repositório.
- Manter nomenclatura e formatação consistentes.

Não é obrigatório utilizar Repository Pattern, Clean Architecture, DDD ou microsserviços.

## 7. Tratamento de erros

As respostas de erro devem ser compreensíveis e consistentes.

| Situação | Código esperado |
| --- | ---: |
| Dados de entrada inválidos | `400 Bad Request` |
| Transição ou operação não permitida | `400 Bad Request` |
| Recurso não encontrado | `404 Not Found` |
| E-mail já cadastrado | `409 Conflict` |
| Recurso criado | `201 Created` |
| Exclusão concluída sem corpo | `204 No Content` |

O formato exato do corpo de erro pode ser definido pelo candidato, desde que seja utilizado de maneira consistente.

## 8. Documentação da entrega

O repositório deve possuir um `README.md` contendo:

- Objetivo do projeto.
- Tecnologias utilizadas.
- Pré-requisitos.
- Instruções para configurar e executar a aplicação.
- Instruções para criar ou atualizar o banco de dados.
- Exemplos de requisições.
- Decisões técnicas relevantes.
- Melhorias que seriam realizadas com mais tempo.

## 9. Diferenciais opcionais

- Testes unitários.
- Testes de integração.
- Tratamento global de exceções.
- Paginação da listagem.
- Ordenação dos chamados.
- Docker e Docker Compose.
- Logs estruturados.
- Padronização das respostas de erro.
- Endpoint com a quantidade de chamados por status.
- Histórico das alterações de status.

Os diferenciais não compensam requisitos obrigatórios ausentes ou quebrados.

## 10. Critérios de avaliação

| Critério | Pontuação |
| --- | ---: |
| Funcionamento e atendimento dos requisitos | 35 |
| Fundamentos de C# e lógica | 20 |
| Modelagem e persistência | 15 |
| Uso correto de HTTP e REST | 15 |
| Organização e legibilidade | 10 |
| Git e documentação | 5 |

## 11. Checklist de aceite

### Solicitantes

- [ ] Cadastrar solicitante válido.
- [ ] Rejeitar nome inválido.
- [ ] Rejeitar e-mail inválido.
- [ ] Rejeitar e-mail duplicado com `409 Conflict`.
- [ ] Listar solicitantes.
- [ ] Buscar solicitante existente por ID.
- [ ] Retornar `404` para solicitante inexistente.

### Chamados

- [ ] Abrir chamado para solicitante existente.
- [ ] Rejeitar chamado para solicitante inexistente.
- [ ] Criar chamado inicialmente como `Aberto`.
- [ ] Listar todos os chamados.
- [ ] Filtrar por status.
- [ ] Filtrar por prioridade.
- [ ] Combinar os dois filtros.
- [ ] Buscar chamado existente por ID.
- [ ] Retornar `404` para chamado inexistente.
- [ ] Permitir `Aberto -> EmAtendimento`.
- [ ] Permitir `EmAtendimento -> Concluido`.
- [ ] Rejeitar transições inválidas.
- [ ] Excluir chamado aberto.
- [ ] Rejeitar exclusão de chamado que não esteja aberto.

### Qualidade técnica

- [ ] Projeto compila sem erros.
- [ ] Projeto compila sem avisos relevantes.
- [ ] Banco pode ser criado por migrations.
- [ ] Dados continuam disponíveis após reiniciar a aplicação.
- [ ] Contratos OpenAPI correspondem às respostas reais.
- [ ] Erros não retornam rastreamento interno da aplicação.
- [ ] README contém instruções suficientes para executar o projeto.
- [ ] Nenhuma credencial real foi versionada.

## 12. Perguntas para apresentação técnica

Ao concluir, esteja preparado para explicar:

1. Como a aplicação foi organizada?
2. Por que o banco de dados escolhido foi utilizado?
3. Como a aplicação garante que um e-mail não seja duplicado?
4. Onde estão implementadas as regras de mudança de status?
5. O que acontece se duas requisições tentarem cadastrar o mesmo e-mail ao mesmo tempo?
6. Qual é a diferença entre validação de entrada e regra de negócio?
7. Por que foram utilizadas operações assíncronas?
8. Quais comportamentos deveriam receber testes primeiro?
9. Qual parte seria melhorada se houvesse mais tempo?
10. Como um novo requisito seria incorporado à solução?
