

// ============================================================
//  EXAMPLE CONTROLLER  (Endpoint — Middleware 10)
// ============================================================

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // ✅ Inherits fallback policy — requires authenticated user
    [HttpGet]
    public IActionResult GetAll()
        => Ok(new[] { "Product A", "Product B" });

    // ✅ Admin-only endpoint
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult Delete(int id)
        => Ok(new { Message = $"Product {id} deleted." });

    // ✅ Public endpoint — overrides the fallback policy
    [HttpGet("public")]
    [AllowAnonymous]
    public IActionResult GetPublic()
        => Ok(new { Message = "This is publicly accessible." });
}
