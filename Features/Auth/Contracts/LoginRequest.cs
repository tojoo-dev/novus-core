using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Novus.Features.Auth.Contracts;

/// <summary>
/// Data required to authenticate a user.
/// </summary>
public record LoginRequest(
    [property: Description("The user's email address")]
    [Required, EmailAddress] string Email,

    [property: Description("The user's password")]
    [Required] string Password
);
