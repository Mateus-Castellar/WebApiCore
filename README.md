# **Web Api .Net Core 6 - Restful**

## *Ferramentas*
- Visual Studio 2022
- SQL Server Localdb
- Azuere Data Studio

## *Setup/Rodar Aplicação*

- Clone o repositório para seu ambiente

  ```bash
  # Clone este repositório
  $ git clone https://github.com/Mateus-Castellar/api-aspnet-core.git
  ```
- Entre dentro da pasta de acesso a dados e gere a base de dados

  ```bash
  # Entre na pasta do projeto
  $ cd api-aspnet-core
  
  # Navegue até a pasta de infra
  $ cd src/AppCore.Data
  
  # Gere a base de dados
  $ dotnet ef database update --startup-project AppCore.API
  ```
