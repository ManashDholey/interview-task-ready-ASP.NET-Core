using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
// Custom Validation Attribute — Validate Email Address

// Here we'll create our own attribute instead of directly using [EmailAddress].
public class ValidEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is not string email)
        {
            return new ValidationResult("Email must be a string.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return new ValidationResult("Email is required.");
        }

        const string pattern =
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (!Regex.IsMatch(email, pattern))
        {
            return new ValidationResult(
                "Please enter a valid email address.");
        }

        return ValidationResult.Success;
    }
}
// Use the Attribute in a Model
public class UserModel
{
    public string Name { get; set; } = string.Empty;

    [ValidEmail]
    public string Email { get; set; } = string.Empty;
}

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateUser(UserModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(new
        {
            Message = "User created successfully",
            User = model
        });
    }
}

