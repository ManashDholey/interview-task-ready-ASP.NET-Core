using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Marks this class as an API controller and enables API-specific behavior,
// such as automatic validation responses for invalid request models.
[ApiController]
// Builds the base route from the controller name: /api/products.
[Route("api/[controller]")]
// ControllerBase is the base class for API controllers.
// It provides HTTP response helpers like Ok(), NotFound(), BadRequest(),
// and is used when the controller returns JSON data instead of MVC views.
public class ProductsController : ControllerBase
{
    // Demo-only in-memory storage. The static list is shared across requests,
    // but its contents are lost when the application restarts.
    private static readonly List<Product> products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 45000 },
        new Product { Id = 2, Name = "Mobile", Price = 25000 }
    };

    // GET /api/products: return all products with HTTP 200 (OK).
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        return Ok(products);
    }

    // GET /api/products/{id}: find and return one product by its ID.
    [HttpGet("{id}")]
    public ActionResult<Product> GetById(int id)
    {
        // Search the in-memory list for the product whose ID matches the route value.
        var product = products.FirstOrDefault(p => p.Id == id);

        // Return HTTP 404 (Not Found) if no product has that ID.
        if (product == null)
        {
            return NotFound(new { Message = $"Product {id} not found." });
        }

        // Return HTTP 200 (OK) with the matching product in the response body.
        return Ok(product);
    }

    // POST /api/products: create a product from the JSON request body.
    [HttpPost]
    public ActionResult<Product> Create([FromBody] Product product)
    {
        // Reject a missing product payload with HTTP 400 (Bad Request).
        if (product == null)
        {
            return BadRequest(new { Message = "Product data is required." });
        }

        // Prevent duplicate product IDs and report the conflict with HTTP 409.
        if (products.Any(p => p.Id == product.Id))
        {
            return Conflict(new { Message = $"Product {product.Id} already exists." });
        }

        // Add the new product to the in-memory collection.
        products.Add(product);

        // Return HTTP 201 (Created), including the new product and its GET URL.
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // PUT /api/products/{id}: replace the editable fields of an existing product.
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Product updatedProduct)
    {
        // Reject a missing product payload with HTTP 400 (Bad Request).
        if (updatedProduct == null)
        {
            return BadRequest(new { Message = "Updated product data is required." });
        }

        // Find the existing product using the ID supplied in the route.
        var product = products.FirstOrDefault(p => p.Id == id);

        // Return HTTP 404 (Not Found) if the requested product does not exist.
        if (product == null)
        {
            return NotFound(new { Message = $"Product {id} not found." });
        }

        // Copy the updated values onto the existing product.
        // The route ID remains authoritative; the request body does not change it.
        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;

        // Return HTTP 200 (OK) with the updated product.
        return Ok(product);
    }

    // DELETE /api/products/{id}: remove a product by ID.
    [HttpDelete("{id}")]
    // Require callers to satisfy the application's AdminOnly authorization policy.
    [Authorize(Policy = "AdminOnly")]
    public IActionResult Delete(int id)
    {
        // Find the product to remove.
        var product = products.FirstOrDefault(p => p.Id == id);

        // Return HTTP 404 (Not Found) if there is nothing to delete.
        if (product == null)
        {
            return NotFound(new { Message = $"Product {id} not found." });
        }

        // Remove the product from the in-memory collection.
        products.Remove(product);

        // Return HTTP 200 (OK) with a confirmation message.
        return Ok(new { Message = $"Product {id} deleted." });
    }

    // GET /api/products/public: demonstrate an endpoint accessible without authentication.
    [HttpGet("public")]
    // Overrides any fallback authorization requirement for this action.
    [AllowAnonymous]
    public IActionResult GetPublic()
    {
        return Ok(new { Message = "This is publicly accessible." });
    }
}

// Model representing a product returned by or accepted by the API.
public class Product
{
    // Product identifier used by the routes and lookup operations.
    public int Id { get; set; }

    // Product name; initialized to an empty string to avoid a null default.
    public string Name { get; set; } = string.Empty;

    // Product price represented as decimal for currency-like values.
    public decimal Price { get; set; }
}
