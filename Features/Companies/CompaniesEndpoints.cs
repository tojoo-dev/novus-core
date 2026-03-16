using Novus.Domain;
using Novus.Infrastructure.Database;
using System.Security.Claims;
using Novus.Features.Companies.Contracts;
using Novus.Infrastructure.Common;

namespace Novus.Features.Companies;

public class CompaniesEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/companies").WithTags("Companies").RequireAuthorization();

        group.MapPost("/", CreateCompany)
            .Produces<BaseResponse<CompanyResponse>>()
            .Produces<BaseResponse>(StatusCodes.Status401Unauthorized);

        group.MapGet("/{id}", GetCompany)
            .Produces<BaseResponse<CompanyResponse>>()
            .Produces<BaseResponse>(StatusCodes.Status404NotFound)
            .Produces<BaseResponse>(StatusCodes.Status401Unauthorized);
    }

    [EndpointSummary("Create a new company")]
    [EndpointDescription("Creates a new company and assigns the current user as the owner.")]
    private static async Task<IResult> CreateCompany(CreateCompanyRequest request, AppDbContext db, ClaimsPrincipal userPrincipal)
    {
        var userId = Guid.Parse(userPrincipal.FindFirstValue("user_id")!);

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        var membership = new TenantMembership
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Company = company,
            RoleId = 1 // Owner
        };

        db.Companies.Add(company);
        db.TenantMemberships.Add(membership);
        await db.SaveChangesAsync();

        var response = new CompanyResponse(company.Id, company.Name);
        return Results.Ok(BaseResponse<CompanyResponse>.Successful(response, "Company created successfully."));
    }

    [EndpointSummary("Get company by ID")]
    [EndpointDescription("Retrieves the details of a specific company by its unique identifier.")]
    private static async Task<IResult> GetCompany(Guid id, AppDbContext db)
    {
        var company = await db.Companies.FindAsync(id);
        if (company is null)
        {
            return Results.NotFound(BaseResponse.Failure("Company not found."));
        }

        var response = new CompanyResponse(company.Id, company.Name);
        return Results.Ok(BaseResponse<CompanyResponse>.Successful(response));
    }
}
