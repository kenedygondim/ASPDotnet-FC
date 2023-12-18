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

