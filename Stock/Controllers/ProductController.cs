using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.Data;
using Stock.Models;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Product
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }

    // POST: api/Product
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDTO productDTO)
    {
        var product = new Product
        {
            Name = productDTO.Name,
            Description = productDTO.Description,
            Quantity = productDTO.Quantity,
            Price = productDTO.Price,
            DateAdded = DateTime.Now, // Define a data de criação
            DateUpdated = DateTime.MinValue // Inicializa a data de atualização
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO updateProductDTO)
    {
        if (updateProductDTO == null)
        {
            return BadRequest("O campo updateProductDTO é obrigatório.");
        }

        // Verifica se o ID no DTO corresponde ao ID na URL
        if (id != updateProductDTO.Id)
        {
            return BadRequest("O ID do produto não corresponde.");
        }

        // Verifique se o produto existe
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        // Atualize as propriedades do produto
        product.Name = updateProductDTO.Name;
        product.Description = updateProductDTO.Description;
        product.Quantity = updateProductDTO.Quantity;
        product.Price = updateProductDTO.Price;

        await _context.SaveChangesAsync();

        return NoContent(); 
    }

    // GET: api/Product/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    // DELETE: api/Product/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}
