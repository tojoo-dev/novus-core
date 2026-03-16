using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Novus.Features.Auth.Contracts;

/// <summary>
/// Data required to register a new user and their company.
/// </summary>
public record RegisterRequest(
    [property: Description("The user's unique email address")]
    [Required, EmailAddress] string Email,

    [property: Description("The user's password (min 8 characters)")]
    [Required, MinLength(8)] string Password,

    [property: Description("The user's full name")]
    [Required] string FullName,

    [property: Description("The name of the company to create for the user")]
    [Required] string CompanyName
);
