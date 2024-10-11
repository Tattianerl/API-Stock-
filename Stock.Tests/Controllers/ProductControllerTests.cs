using Xunit;
using System.Collections.Generic;
using System.Linq;
using Stock.Models;
using Stock.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class ProductControllerTests
{
    [Fact]
    public async Task GetProducts_RetornaTodosOsProdutos()
    {
        // Configura o contexto em memória
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "StockTestDatabase")
            .Options;

        using (var context = new AppDbContext(options))
        {
            // Adiciona produtos ao contexto
            context.Products.AddRange(
                new Product { Id = 1, Name = "Produto 1", Description = "Desc 1", Quantity = 10, Price = 20.0M },
                new Product { Id = 2, Name = "Produto 2", Description = "Desc 2", Quantity = 5, Price = 15.0M }
            );
            await context.SaveChangesAsync();

            var controller = new ProductController(context);

            // Act
            var resultado = await controller.GetProducts();
            var produtosResultado = resultado.Value;

            // Assert
            Assert.NotNull(produtosResultado);
            Assert.Equal(2, produtosResultado.Count());
        }
    }

    [Fact]
    public async Task CreateProduct_AdicionaProdutoERetornaResultadoCriado()
    {

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "StockTestDatabase_CriarProduto")
            .Options;

        using (var context = new AppDbContext(options))
        {
            var controlador = new ProductController(context);

            // Cria um novo objeto DTO para o produto
            var novoProdutoDTO = new CreateProductDTO
            {
                Name = "Produto 3",
                Description = "Desc 3",
                Quantity = 20,
                Price = 30.0M
            };

            // Act
            var resultado = await controlador.CreateProduct(novoProdutoDTO);

            // Assert
            var resultadoCriado = Assert.IsType<CreatedAtActionResult>(resultado.Result); // Mudança aqui
            var produtoCriado = Assert.IsType<Product>(resultadoCriado.Value);
            Assert.Equal(novoProdutoDTO.Name, produtoCriado.Name);
            Assert.Equal(novoProdutoDTO.Description, produtoCriado.Description);
            Assert.Equal(novoProdutoDTO.Quantity, produtoCriado.Quantity);
            Assert.Equal(novoProdutoDTO.Price, produtoCriado.Price);

            // Verifica se o produto foi adicionado ao contexto
            var produtoNoContexto = await context.Products.FindAsync(produtoCriado.Id);
            Assert.NotNull(produtoNoContexto); // Certifica-se de que o produto foi adicionado
        }
    }

    [Fact]
    public async Task GetProductById_RetornaProdutoEspecifico_QuandoProdutoExistir()
    {

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "StockTestDatabase_GetProductById")
            .Options;

        using (var context = new AppDbContext(options))
        {
            var produtoExistente = new Product
            {
                Id = 1,
                Name = "Produto 1",
                Description = "Desc 1",
                Quantity = 10,
                Price = 20.0M
            };
            context.Products.Add(produtoExistente);
            await context.SaveChangesAsync();

            var controller = new ProductController(context);

            // Act
            var resultado = await controller.GetProductById(1);

            // Assert
            var produtoResultado = Assert.IsType<ActionResult<Product>>(resultado);
            var produto = Assert.IsType<Product>(produtoResultado.Value);
            Assert.Equal(produtoExistente.Name, produto.Name);
            Assert.Equal(produtoExistente.Description, produto.Description);
            Assert.Equal(produtoExistente.Quantity, produto.Quantity);
            Assert.Equal(produtoExistente.Price, produto.Price);
        }
    }

    [Fact]
    public async Task GetProductById_RetornaNotFound_QuandoProdutoNaoExistir()
    {

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "StockTestDatabase_GetProductById_NotFound")
            .Options;

        using (var context = new AppDbContext(options))
        {
            var controller = new ProductController(context);

            // Act
            var resultado = await controller.GetProductById(999);

            // Assert
            Assert.IsType<NotFoundResult>(resultado.Result);
        }
    }

    [Fact]
    public async Task UpdateProduct_AtualizaProdutoExistente_ERetornaNoContent()
    {

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "StockTestDatabase_UpdateProduct")
            .Options;

        using (var context = new AppDbContext(options))
        {

            var produtoExistente = new Product
            {
                Id = 1,
                Name = "Produto 1",
                Description = "Desc 1",
                Quantity = 10,
                Price = 20.0M
            };
            context.Products.Add(produtoExistente);
            await context.SaveChangesAsync();

            var controller = new ProductController(context);

            var updateProductDTO = new UpdateProductDTO
            {
                Id = produtoExistente.Id,
                Name = "Produto Atualizado",
                Description = "Descrição Atualizada",
                Quantity = 15,
                Price = 25.0M
            };

            // Act
            var resultado = await controller.UpdateProduct(produtoExistente.Id, updateProductDTO);

            // Assert
            Assert.IsType<NoContentResult>(resultado);

            // Verifica se o produto foi atualizado no contexto
            var produtoAtualizado = await context.Products.FindAsync(produtoExistente.Id);
            Assert.NotNull(produtoAtualizado);
            Assert.Equal(updateProductDTO.Name, produtoAtualizado.Name);
            Assert.Equal(updateProductDTO.Description, produtoAtualizado.Description);
            Assert.Equal(updateProductDTO.Quantity, produtoAtualizado.Quantity);
            Assert.Equal(updateProductDTO.Price, produtoAtualizado.Price);
        }
    }

    [Fact]
    public async Task UpdateProduct_RetornaBadRequest_QuandoIDNaoCorrespondente()
    {

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "StockTestDatabase_UpdateProduct_BadRequest")
            .Options;

        using (var context = new AppDbContext(options))
        {

            var produtoExistente = new Product
            {
                Id = 1,
                Name = "Produto 1",
                Description = "Desc 1",
                Quantity = 10,
                Price = 20.0M
            };
            context.Products.Add(produtoExistente);
            await context.SaveChangesAsync();

            var controller = new ProductController(context);

            var updateProductDTO = new UpdateProductDTO
            {
                Id = 2,
                Name = "Produto Atualizado",
                Description = "Descrição Atualizada",
                Quantity = 15,
                Price = 25.0M
            };

            // Act
            var resultado = await controller.UpdateProduct(produtoExistente.Id, updateProductDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado);
            Assert.Equal("O ID do produto não corresponde.", badRequestResult.Value);
        }
    }

    [Fact]
    public async Task UpdateProduct_RetornaNotFound_QuandoProdutoNaoExistir()
    {

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "StockTestDatabase_UpdateProduct_NotFound")
            .Options;

        using (var context = new AppDbContext(options))
        {
            var controller = new ProductController(context);


            var updateProductDTO = new UpdateProductDTO
            {
                Id = 999,
                Name = "Produto Inexistente",
                Description = "Descrição Inexistente",
                Quantity = 0,
                Price = 0.0M
            };

            // Act
            var resultado = await controller.UpdateProduct(999, updateProductDTO);

            // Assert
            Assert.IsType<NotFoundResult>(resultado);
        }
    }

    [Fact]
    public async Task DeleteProduct_RetornaNoContent_QuandoProdutoExistir()
    {

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "StockTestDatabase_DeleteProduct_Exists")
            .Options;

        using (var context = new AppDbContext(options))
        {

            var product = new Product
            {
                Id = 1,
                Name = "Produto Teste",
                Description = "Descrição do Produto Teste",
                Quantity = 10,
                Price = 20.0M
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var controller = new ProductController(context);

            // Act
            var resultado = await controller.DeleteProduct(1);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }
    }

    [Fact]
    public async Task DeleteProduct_RetornaNotFound_QuandoProdutoNaoExistir()
    {

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "StockTestDatabase_DeleteProduct_NotFound")
            .Options;

        using (var context = new AppDbContext(options))
        {
            var controller = new ProductController(context);

            // Act
            var resultado = await controller.DeleteProduct(999);
            // Assert
            Assert.IsType<NotFoundResult>(resultado);
        }
    }
}
