using System.ComponentModel;

namespace Novus.Features.Auth.Contracts;

/// <summary>
/// Data returned after a successful authentication.
/// </summary>
public record AuthResponse(
    [property: Description("The unique identifier of the user")]
    Guid UserId,

    [property: Description("The unique identifier of the user's current company")]
    Guid CompanyId,

    [property: Description("The JWT access token")]
    string Token
);
