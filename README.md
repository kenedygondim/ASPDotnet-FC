## API básica de dados futebolísticos

Olá! Desenvolvi uma API básica relacionada a clubes de futebol para aprimorar minhas habilidades
utilizando a linguagem C# e ASP.NET Core. Nesse projeto, o usuário poderá ver, criar, editar e deletar 
clubes de futebol, competiçôes e jogadores históricos. 
Vale lembrar que esse é meu primeiro projeto back-end, e pode haver diversos pontos a se aprimorar,
por exemplo, em relação a redundância dos dados.

## Ferramentas instaladas separadamente

- Entity Framework Core: ORM que permitiu a manipulação de dados usando objetos de domínio ao invés de consultas SQL brutas.

- Entity Framework Tools: Adição de Migrations e atualização do banco de dados.

- AutoMapper: Foi utilizado em conjunto com objetos de transferência de dados (DTOs) para evitar a exposição de dados desnecessários e também facilitar as operações de CRUD.

## Conceitos e padrões de código

- Single Responsibility Principle - SRP: Busquei dividir ao máximo as responsabilidades.

- Code First: As classes de entidade são usadas para representar objetos que serão armazenados no banco
de dados.

- Clean Code: Tentei ao máximo deixar o código o mais legível possível, facilitando futuras alterações.

## Controladores de Clubs

1. GET 

- Busca todos os clubes:
`localhost:5165/api/clubs`

- Busca um clube por seu id:
`localhost:5165/api/clubs/{clubId}`

- Busca um clube por seu nome:
`localhost:5165/api/clubs/{clubName}`

- Busca todos as ligas associadas ao clube com base no id:
`localhost:5165/api/clubs/{clubId}/leagues`

- Busca o país de um clube específico com base no id:
`localhost:5165/api/clubs/{clubId}/clubcountry`

- Busca o ano de fundação de um clube específico com base no id:
`localhost:5165/api/clubs/{clubId}/clubfoundationyear`

- Busca o estádio de um clube específico com base no id:
`localhost:5165/api/clubs/{clubId}/clubstadium`


2. POST

- Cria um clube e o associa a uma competição existente:
`localhost:5165/api/clubs/{competitionId}/addclub`

3.PUT 

- Atualiza um clube
`localhost:5165/api/clubs/{clubId}`

3.DELETE 

- Exclui um clube
`localhost:5165/api/clubs/{clubId}`