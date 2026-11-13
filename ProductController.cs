using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 45000 },
        new Product { Id = 2, Name = "Mobile", Price = 25000 }
    };

    // GET: api/products
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        return Ok(products);
    }

    // GET: api/products/1
    [HttpGet("{id}")]
    public ActionResult<Product> GetById(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound(new { Message = $"Product {id} not found." });
        }

        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public ActionResult<Product> Create([FromBody] Product product)
    {
        if (product == null)
        {
            return BadRequest(new { Message = "Product data is required." });
        }

        if (products.Any(p => p.Id == product.Id))
        {
            return Conflict(new { Message = $"Product {product.Id} already exists." });
        }

        products.Add(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // PUT: api/products/1
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Product updatedProduct)
    {
        if (updatedProduct == null)
        {
            return BadRequest(new { Message = "Updated product data is required." });
        }

        var product = products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound(new { Message = $"Product {id} not found." });
        }

        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;

        return Ok(product);
    }

    // DELETE: api/products/1
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult Delete(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound(new { Message = $"Product {id} not found." });
        }

        products.Remove(product);
        return Ok(new { Message = $"Product {id} deleted." });
    }

    // GET: api/products/public
    [HttpGet("public")]
    [AllowAnonymous]
    public IActionResult GetPublic()
    {
        return Ok(new { Message = "This is publicly accessible." });
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
