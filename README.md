# ASP.NET Minimal API com MariaDB

Este projeto é uma implementação de uma API Minimal utilizando ASP.NET Core e MariaDB como banco de dados.


## Configuração do Projeto

1. Clone este repositório
2. Atualize a string de conexão no arquivo `appsettings.json` ou use variáveis de ambiente:

```json
{
  "ConnectionStrings": {
    "MariaDbConnection": "Server=localhost;Database=trabalho_api;User=usuario;Password=sua_senha;"
  }
}
```

## Executando Migrações

Para criar e aplicar migrações do banco de dados:

```bash
dotnet ef migrations add Inicio
dotnet ef database update
```

## Executando o Projeto

Para executar o projeto localmente:

```bash
cd /caminho/para/trabalhoApi
dotnet restore
dotnet run
```

Após iniciar, a API estará disponível em: (Verifique a porta)
- http://localhost:XXXX

## Testando a API

Você pode testar a API usando ferramentas como:
- [Swagger UI](https://localhost:5001/swagger) (disponível automaticamente no ambiente de desenvolvimento)
- [Postman](https://www.postman.com/)
- cURL

Exemplo de requisição com cURL:

```bash
curl -X GET https://localhost:5001/estados
```

## Estrutura do Projeto

- `Program.cs`: Ponto de entrada e configuração da aplicação
- `appsettings.json`: Configurações da aplicação
- `Models/`: Classes de modelo de dados
- `Migrations/`: Migrações do Entity Framework Core

