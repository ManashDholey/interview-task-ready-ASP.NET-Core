

// ============================================================
//  CONTROLLER 1 — Type Constraints (1–7)
// ============================================================

[ApiController]
[Route("api/types")]
public class TypeConstraintsController : ControllerBase
{
    // ── 1. int ────────────────────────────────────────────────
    // Matches:     /api/types/int/42
    // No match:    /api/types/int/hello   /api/types/int/3.14
    [HttpGet("int/{id:int}")]
    public IActionResult GetById(int id)
        => Ok(new
        {
            Constraint = "int",
            Value      = id,
            Example    = "Matched /api/types/int/42"
        });

    // ── 2. long ───────────────────────────────────────────────
    // Matches:     /api/types/long/9876543210
    // No match:    /api/types/long/hello
    [HttpGet("long/{id:long}")]
    public IActionResult GetByLongId(long id)
        => Ok(new
        {
            Constraint = "long",
            Value      = id,
            Example    = "Matched /api/types/long/9876543210"
        });

    // ── 3. bool ───────────────────────────────────────────────
    // Matches:     /api/types/bool/true   /api/types/bool/false
    // No match:    /api/types/bool/yes    /api/types/bool/1
    [HttpGet("bool/{flag:bool}")]
    public IActionResult GetByFlag(bool flag)
        => Ok(new
        {
            Constraint = "bool",
            Value      = flag,
            Example    = "Matched /api/types/bool/true"
        });

    // ── 4. double ─────────────────────────────────────────────
    // Matches:     /api/types/double/3.14   /api/types/double/100
    // No match:    /api/types/double/hello
    [HttpGet("double/{value:double}")]
    public IActionResult GetByDouble(double value)
        => Ok(new
        {
            Constraint = "double",
            Value      = value,
            Example    = "Matched /api/types/double/3.14"
        });

    // ── 5. float ──────────────────────────────────────────────
    // Matches:     /api/types/float/1.5
    // No match:    /api/types/float/abc
    [HttpGet("float/{value:float}")]
    public IActionResult GetByFloat(float value)
        => Ok(new
        {
            Constraint = "float",
            Value      = value,
            Example    = "Matched /api/types/float/1.5"
        });

    // ── 6. guid ───────────────────────────────────────────────
    // Matches:     /api/types/guid/3fa85f64-5717-4562-b3fc-2c963f66afa6
    // No match:    /api/types/guid/12345   /api/types/guid/hello
    [HttpGet("guid/{id:guid}")]
    public IActionResult GetByGuid(Guid id)
        => Ok(new
        {
            Constraint = "guid",
            Value      = id,
            Example    = "Matched /api/types/guid/3fa85f64-5717-4562-b3fc-2c963f66afa6"
        });

    // ── 7. datetime ───────────────────────────────────────────
    // Matches:     /api/types/datetime/2024-01-15
    // No match:    /api/types/datetime/hello   /api/types/datetime/99-99-99
    [HttpGet("datetime/{date:datetime}")]
    public IActionResult GetByDate(DateTime date)
        => Ok(new
        {
            Constraint = "datetime",
            Value      = date.ToString("yyyy-MM-dd"),
            Example    = "Matched /api/types/datetime/2024-01-15"
        });
}


// ============================================================
//  CONTROLLER 2 — String / Pattern Constraints (8–10)
// ============================================================

[ApiController]
[Route("api/strings")]
public class StringConstraintsController : ControllerBase
{
    // ── 8. alpha ──────────────────────────────────────────────
    // Matches:     /api/strings/alpha/JohnDoe
    // No match:    /api/strings/alpha/John123   /api/strings/alpha/John_Doe
    [HttpGet("alpha/{name:alpha}")]
    public IActionResult GetByAlpha(string name)
        => Ok(new
        {
            Constraint = "alpha",
            Value      = name,
            Example    = "Matched /api/strings/alpha/JohnDoe"
        });

    // ── 9. regex ──────────────────────────────────────────────
    // Pattern: exactly 2 uppercase letters (country code like "US", "IN", "UK")
    // Matches:     /api/strings/regex/US   /api/strings/regex/IN
    // No match:    /api/strings/regex/usa  /api/strings/regex/12
    [HttpGet(@"regex/{code:regex(^[A-Z]{{2}}$)}")]
    public IActionResult GetByRegex(string code)
        => Ok(new
        {
            Constraint = "regex(^[A-Z]{2}$)",
            Value      = code,
            Example    = "Matched /api/strings/regex/US"
        });

    // ── 10. length ────────────────────────────────────────────
    // Matches exactly 5-character strings
    // Matches:     /api/strings/length/Hello   /api/strings/length/AB123
    // No match:    /api/strings/length/Hi      /api/strings/length/TooLong
    [HttpGet("length/{code:length(5)}")]
    public IActionResult GetByExactLength(string code)
        => Ok(new
        {
            Constraint = "length(5)",
            Value      = code,
            Example    = "Matched /api/strings/length/AB123"
        });

    // length with min and max range: between 3 and 10 characters
    // Matches:     /api/strings/length-range/Hello   /api/strings/length-range/Hi
    // No match:    /api/strings/length-range/A       /api/strings/length-range/TooLongWord
    [HttpGet("length-range/{username:length(3,10)}")]
    public IActionResult GetByLengthRange(string username)
        => Ok(new
        {
            Constraint = "length(3,10)",
            Value      = username,
            Example    = "Matched /api/strings/length-range/Hello"
        });
}


// ============================================================
//  CONTROLLER 3 — Numeric Range Constraints (11–12)
// ============================================================

[ApiController]
[Route("api/numbers")]
public class NumericConstraintsController : ControllerBase
{
    // ── 11a. min ──────────────────────────────────────────────
    // Only matches if age >= 18
    // Matches:     /api/numbers/min/18   /api/numbers/min/25
    // No match:    /api/numbers/min/17   /api/numbers/min/0
    [HttpGet("min/{age:int:min(18)}")]
    public IActionResult GetByMinAge(int age)
        => Ok(new
        {
            Constraint = "min(18)",
            Value      = age,
            Message    = $"User is {age} years old — eligible (18+)",
            Example    = "Matched /api/numbers/min/25"
        });

    // ── 11b. max ──────────────────────────────────────────────
    // Only matches if score <= 100
    // Matches:     /api/numbers/max/100   /api/numbers/max/55
    // No match:    /api/numbers/max/101
    [HttpGet("max/{score:int:max(100)}")]
    public IActionResult GetByMaxScore(int score)
        => Ok(new
        {
            Constraint = "max(100)",
            Value      = score,
            Message    = $"Score is {score}/100",
            Example    = "Matched /api/numbers/max/55"
        });

    // ── 12. range ─────────────────────────────────────────────
    // Only matches if year is between 1900 and 2026
    // Matches:     /api/numbers/range/2024   /api/numbers/range/1950
    // No match:    /api/numbers/range/1899   /api/numbers/range/2027
    [HttpGet("range/{year:int:range(1900,2026)}")]
    public IActionResult GetByYearRange(int year)
        => Ok(new
        {
            Constraint = "range(1900,2026)",
            Value      = year,
            Message    = $"Year {year} is valid",
            Example    = "Matched /api/numbers/range/2024"
        });

    // Combining multiple numeric constraints
    // Only matches integers between 1 and 999 (product quantity)
    // Matches:     /api/numbers/quantity/50
    // No match:    /api/numbers/quantity/0   /api/numbers/quantity/1000
    [HttpGet("quantity/{qty:int:min(1):max(999)}")]
    public IActionResult GetByQuantity(int qty)
        => Ok(new
        {
            Constraint = "int:min(1):max(999)",
            Value      = qty,
            Message    = $"Quantity {qty} is valid",
            Example    = "Matched /api/numbers/quantity/50"
        });
}


// ============================================================
//  CONTROLLER 4 — Presence Constraints (13–14)
// ============================================================

[ApiController]
[Route("api/presence")]
public class PresenceConstraintsController : ControllerBase
{
    // ── 13. required ──────────────────────────────────────────
    // The parameter MUST be present in the URL.
    // Without :required, optional parameters can be omitted.
    // Matches:     /api/presence/required/john
    // No match:    /api/presence/required/          (empty segment)
    [HttpGet("required/{username:required}")]
    public IActionResult GetRequired(string username)
        => Ok(new
        {
            Constraint = "required",
            Value      = username,
            Message    = $"Username '{username}' is present",
            Example    = "Matched /api/presence/required/john"
        });

    // ── 14. nonempty ──────────────────────────────────────────
    // Ensures the parameter value is not empty, null, or whitespace.
    // Matches:     /api/presence/nonempty/hello
    // No match:    /api/presence/nonempty/         (blank value)
    [HttpGet("nonempty/{tag:nonempty}")]
    public IActionResult GetNonEmpty(string tag)
        => Ok(new
        {
            Constraint = "nonempty",
            Value      = tag,
            Message    = $"Tag '{tag}' is not empty",
            Example    = "Matched /api/presence/nonempty/hello"
        });

    // Combining required + alpha for a clean username route
    // Matches:     /api/presence/profile/Alice
    // No match:    /api/presence/profile/Alice123   (digits not allowed)
    [HttpGet("profile/{username:required:alpha}")]
    public IActionResult GetProfile(string username)
        => Ok(new
        {
            Constraint = "required + alpha",
            Value      = username,
            Message    = $"Profile for '{username}'",
            Example    = "Matched /api/presence/profile/Alice"
        });
}


// ============================================================
//  CONTROLLER 5 — String Length Constraints (15)
// ============================================================

[ApiController]
[Route("api/length")]
public class LengthConstraintsController : ControllerBase
{
    // ── 15a. maxlength ────────────────────────────────────────
    // Username must be 20 characters or fewer
    // Matches:     /api/length/maxlength/John   /api/length/maxlength/SuperLongNameHere123
    // No match:    /api/length/maxlength/ThisUsernameIsWayTooLongToBeValid
    [HttpGet("maxlength/{username:maxlength(20)}")]
    public IActionResult GetByMaxLength(string username)
        => Ok(new
        {
            Constraint  = "maxlength(20)",
            Value       = username,
            ValueLength = username.Length,
            Message     = $"'{username}' is within 20 characters",
            Example     = "Matched /api/length/maxlength/John"
        });

    // ── 15b. minlength ────────────────────────────────────────
    // Password must be at least 8 characters
    // Matches:     /api/length/minlength/Password1
    // No match:    /api/length/minlength/Pass    (only 4 chars)
    [HttpGet("minlength/{password:minlength(8)}")]
    public IActionResult GetByMinLength(string password)
        => Ok(new
        {
            Constraint  = "minlength(8)",
            Value       = password,
            ValueLength = password.Length,
            Message     = $"Password meets minimum length of 8",
            Example     = "Matched /api/length/minlength/Password1"
        });

    // Combining minlength + maxlength (username: 3–15 chars)
    // Matches:     /api/length/username/john   /api/length/username/superuser
    // No match:    /api/length/username/ab     /api/length/username/waytoolongusername
    [HttpGet("username/{username:minlength(3):maxlength(15)}")]
    public IActionResult GetByUsernameLength(string username)
        => Ok(new
        {
            Constraint  = "minlength(3):maxlength(15)",
            Value       = username,
            ValueLength = username.Length,
            Message     = $"Username '{username}' is valid (3–15 chars)",
            Example     = "Matched /api/length/username/john"
        });
}


// ============================================================
//  CONTROLLER 6 — Real-World Combined Constraint Examples
// ============================================================

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    // Scenario: GET product by positive integer ID
    // Matches:     /api/products/5
    // No match:    /api/products/0   /api/products/abc
    [HttpGet("{id:int:min(1)}")]
    public IActionResult GetProduct(int id)
        => Ok(new { Message = $"Product #{id}", Constraints = "int:min(1)" });

    // Scenario: GET products by year published (1980–2026)
    [HttpGet("year/{year:int:range(1980,2026)}")]
    public IActionResult GetByYear(int year)
        => Ok(new { Message = $"Products from {year}", Constraints = "int:range(1980,2026)" });

    // Scenario: GET product by SKU — exactly 8 chars, letters only
    [HttpGet("sku/{sku:alpha:length(8)}")]
    public IActionResult GetBySku(string sku)
        => Ok(new { Message = $"Product SKU: {sku}", Constraints = "alpha:length(8)" });

    // Scenario: GET product by GUID (database primary key)
    [HttpGet("guid/{id:guid}")]
    public IActionResult GetByGuid(Guid id)
        => Ok(new { Message = $"Product GUID: {id}", Constraints = "guid" });

    // Scenario: Search by category (letters only, 3–20 chars)
    [HttpGet("category/{category:alpha:minlength(3):maxlength(20)}")]
    public IActionResult GetByCategory(string category)
        => Ok(new { Message = $"Category: {category}", Constraints = "alpha:minlength(3):maxlength(20)" });
}


// ============================================================
//  BONUS — Custom Route Constraint
//  (extends IRouteConstraint for logic beyond built-ins)
// ============================================================

// Only matches even integers
// Usage:   {id:evenNumber}
// Matches:     /api/even/4   /api/even/100
// No match:    /api/even/3   /api/even/7
public class EvenNumberConstraint : IRouteConstraint
{
    public bool Match(
        HttpContext?        httpContext,
        IRouter?            route,
        string              routeKey,
        RouteValueDictionary values,
        RouteDirection      routeDirection)
    {
        if (values.TryGetValue(routeKey, out var value))
        {
            if (int.TryParse(value?.ToString(), out int intValue))
                return intValue % 2 == 0;
        }
        return false;
    }
}

[ApiController]
[Route("api/even")]
public class EvenController : ControllerBase
{
    [HttpGet("{id:int:evenNumber}")]
    public IActionResult GetEven(int id)
        => Ok(new
        {
            Constraint = "Custom: evenNumber",
            Value      = id,
            Message    = $"{id} is an even number",
            Example    = "Matched /api/even/42"
        });
}


// ============================================================
//  QUICK REFERENCE TABLE
// ============================================================
//
//  Constraint           Template Syntax                Matches Example
//  ─────────────────────────────────────────────────────────────────────
//  int                  {id:int}                       42
//  long                 {id:long}                      9876543210
//  bool                 {flag:bool}                    true / false
//  double               {val:double}                   3.14
//  float                {val:float}                    1.5
//  guid                 {id:guid}                      3fa85f64-5717-...
//  datetime             {date:datetime}                2024-01-15
//  alpha                {name:alpha}                   JohnDoe
//  regex(pattern)       {code:regex(^[A-Z]{2}$)}       US
//  length(n)            {code:length(5)}               AB123
//  length(min,max)      {s:length(3,10)}               Hello
//  min(n)               {age:min(18)}                  25
//  max(n)               {score:max(100)}               99
//  range(min,max)       {year:range(1900,2026)}         2024
//  required             {name:required}                john
//  nonempty             {tag:nonempty}                 dotnet
//  maxlength(n)         {username:maxlength(20)}        Alice
//  minlength(n)         {pw:minlength(8)}              Password1
//  ─────────────────────────────────────────────────────────────────────
//  NOTE: Chain multiple constraints with colons:
//        {username:required:alpha:minlength(3):maxlength(15)}
// ============================================================
