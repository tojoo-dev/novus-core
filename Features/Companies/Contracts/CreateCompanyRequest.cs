using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Novus.Features.Companies.Contracts;

/// <summary>
/// Data required to create a new company.
/// </summary>
public record CreateCompanyRequest(
    [property: Description("The name of the company")]
    [Required] string Name
);
