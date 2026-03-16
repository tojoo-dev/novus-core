using System.ComponentModel;

namespace Novus.Features.Companies.Contracts;

/// <summary>
/// Data representing a company.
/// </summary>
public record CompanyResponse(
    [property: Description("The unique identifier of the company")]
    Guid Id,

    [property: Description("The name of the company")]
    string Name
);
