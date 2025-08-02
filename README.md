# UxComexAPI - Guia de Inicialização

## ✅ Requisitos Iniciais

Antes de iniciar o projeto, certifique-se de ter os seguintes recursos instalados:

1. [Docker Desktop](https://www.docker.com/products/docker-desktop/)
2. SQL Server (local ou via Docker)
3. [Redis Insight (Opcional)](https://redis.com/redis-enterprise/redis-insight/)

---

## 🚀 Passos Iniciais para Rodar o Projeto

### 1. Abrir o Projeto da API

Abra a solução `UxComexAPI` no Visual Studio.

### 2. Subir os Containers com Docker Compose

No terminal, navegue até a pasta onde está o `docker-compose.yml`:

```bash
cd projectapi
docker-compose up
```

### 3. Acessar o SQL Server

Conecte-se ao SQL Server com as seguintes credenciais:

Servidor: localhost,14330

Login: sa

Senha: #Maluko1998

### Criar os database:

GLOBAL e GLOBAL_LOG

### 4. Configurar NuGet Local no Visual Studio

Abra o Visual Studio

Vá em Ferramentas > Gerenciador de Pacotes NuGet > Configurações

Clique em Adicionar Nova Origem de Pacote

Aponte para a pasta LocalNuget dentro do projeto UxComexAPI, onde estão os .nupkg

### 5. Executar Scripts do Banco de Dados

Na pasta scripts do projeto UxComexAPI, execute os scripts na ordem indicada para criar e popular o banco de dados.

### 6. Executar a API

Execute o projeto API no Visual Studio. A API estará disponível na porta:

``` bash
https://localhost:44366
```

### 7. Executar o Projeto Principal

Após a API estar em execução, inicie o projeto principal.

📝 Observações

Certifique-se de que o Docker está rodando antes de subir os containers.

Os pacotes NuGet locais são necessários para o correto funcionamento de algumas bibliotecas internas do projeto.
