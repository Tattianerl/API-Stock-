# API de Gerenciamento de Estoque de Produtos

## Descrição

A API de Gerenciamento de Estoque de Produtos permite que os usuários realizem operações CRUD (Criar, Ler, Atualizar, Deletar) em um sistema de estoque. Esta API foi desenvolvida para gerenciar produtos em uma loja, facilitando o controle do inventário e a atualização das informações dos produtos.

## Funcionalidades

A API oferece as seguintes funcionalidades:

- **Criar Produto**: Adiciona um novo produto ao estoque.
- **Listar Produtos**: Retorna uma lista de todos os produtos cadastrados no sistema.
- **Obter Produto por ID**: Recupera as informações de um produto específico pelo seu ID.
- **Atualizar Produto**: Permite a atualização das informações de um produto existente.
- **Deletar Produto**: Remove um produto do estoque pelo seu ID.

## Tecnologias Utilizadas

- **.NET 8.0**: Plataforma de desenvolvimento.
- **Entity Framework Core**: ORM para interagir com o banco de dados.
- **In-Memory Database**: Utilizado para testes e desenvolvimento.
- **xUnit**: Framework de testes unitários.

## Endpoints

### 1. Criar Produto

- **Método**: `POST`
- **URL**: `/api/products`
- **Corpo da Requisição**:
  ```json
  
  {
      "name": "Nome do Produto",
      "description": "Descrição do Produto",
      "quantity": 10,
      "price": 100.00
  }

- **Resposta:** 201 Created com o produto criado.

### 2. Listar Produtos
- **Método:** `GET`
`URL: /api/products`.
- **Resposta:** 200 OK com uma lista de produtos.

### 3. Obter Produto por ID
- **Método:** `GET`
`URL: /api/products/{id}`
- **Resposta:**  
200 OK com os detalhes do produto.  
404 Not Found se o produto não existir.

### 4. Atualizar Produto
- **Método:** `PUT`
`URL: /api/products/{id}`
- **Corpo da Requisição**:
 ```json

{
    "id": 1,
    "name": "Produto Atualizado",
    "description": "Nova Descrição",
    "quantity": 15,
    "price": 150.00
}
```
- **Resposta:**  
204 No Content se a atualização for bem-sucedida.  
400 Bad Request se o ID não corresponder.  
404 Not Found se o produto não existir.

### 5. Deletar Produto
- **Método:** `DELETE`
``URL: /api/products/{id}``
- **Resposta:**  
204 No Content se a exclusão for bem-sucedida.  
404 Not Found se o produto não existir.

## Execução da API

Clone este repositório.

Navegue até o diretório da API.

Execute o comando:

```bash          
dotnet run
dotnet watch run
```

### Testes  
Os testes unitários foram implementados utilizando o xUnit.  
 Para executar os testes, navegue até o diretório de testes e execute:

```bash  
dotnet test
```

### Contribuição
Sinta-se à vontade para contribuir com este projeto! Abra uma issue ou envie um pull request.
