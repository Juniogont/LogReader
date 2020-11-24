# Desafio Técnico .NET Log Reader

Escrever uma aplicação .NET que tenha como objetivo, fazer upload de arquivo de log e
popular o banco de dados.

#### BackEnd

- ASP.NET Core API 3.1 MVC 
- ORM (Entity Framework Core (Code First Aproach))
- Banco de Dados PostGreSQL
- xUnit (Testes Unitários)
- LINQ
- Lambda Expressions
- Interface
- Herança
- Serviço para Processamento em Batch
- Serviço para Inserção manual
- Raalizado a agregação de USER pelo IP do Log
- Exemplo do arquivo  de Log : Log.txt

#### FrontEnd

- ASP.NET Core 3.1 MVC 
- AJAX
- Operações CRUD 
- Importação de 1 ou mais arquivos ao fazer o UPLOAD
- Interface
- Herança
- Serviço para Processamento em Batch
- Serviço para Inserção manual
- Na tela de listagem foi criar a possibilidade de busca por IP ou por USER

#### Instruções 
    
- Alterar a ConnectionString com um servidor e user PostGreSQL Válido
	Arquivos: 
		appsettings.json:
		Linha 3 - "LogReaderConnectionString": "User ID =postgres;Password=123qwe;Server=localhost;Port=5432;Database=DBLogReader;Integrated Security=true;Pooling=true;"
		LogReaderDbContext:
		Linha 19 - options.UseNpgsql(@"User ID =postgres;Password=123qwe;Server=localhost;Port=5432;Database=DBLogReader;Integrated Security=true;Pooling=true; ",
- Rodar o migration para criar as tabelas no DB (update-database)
- O BackEnd deve estar rodando para o FrontEnd rodar sem erros


