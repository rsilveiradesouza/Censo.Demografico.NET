# Censo Demográfico 2020

Projeto de cadastro de pessoas e manipulação de filtros "dinamicos", estatísticos e em árvore.

Este repositório está utilizando o **CircleCI** para integração contínua. Ele está configurado para executar o build da solução e executar os testes unitários e de integração.

[![CircleCI](https://circleci.com/gh/rsilveiradesouza/Censo.NET.svg?style=svg)](https://circleci.com/gh/rsilveiradesouza/Censo.NET)

### Sobre

Neste projeto estamos simulando um cadastro de pessoas de uma determinada região, estamos utilizando uma página web com uma dashboard feita em **Angular 9+** para visualização em tempo real dos cadastros realizadas, essa integração é realizada via **SignalR** com o servidor. No cadastro estamos utilizando **webapi asp.net core(3.1)** para receber as solicitações e processar, estamos armazenando em um banco de dados relacional(**SQL SERVER**) e utilizando o **Entity Framework Core** como framework de ORM para essa comunicação. 
Estamos utilizando uma arquitetura simples no backend que é representando por a camada de Api, Aplicação, Dominio e Infraestrutura.

### Execução do projeto(Docker)

O projeto está utilizando docker, de forma simples e fácil execute o seguinte comando na raíz do repositório para levantar os ambientes do sql server, api e web:<br/>
```docker-compose up```

Caso a API não levante, basta executar esse comando isolado em outro terminal:<br/>
```docker-compose up server```

Obs: O serviço do docker tem que estar rodando para o comando acima funcionar.

### Acessos

- A dashboard é acessada via: http://localhost:8081/
- A documentação da API é acessada via: http://localhost:8080/swagger

## License

This project is licensed under the MIT License.
