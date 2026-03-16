using Microsoft.EntityFrameworkCore;
using Novus.Domain;
using Novus.Infrastructure.Authentication;
using Novus.Infrastructure.Database;
using Novus.Features.Auth.Contracts;
using Novus.Infrastructure.Common;

namespace Novus.Features.Auth;

public class AuthEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("/register", Register)
            .Produces<BaseResponse<AuthResponse>>()
            .Produces<BaseResponse>(StatusCodes.Status400BadRequest);

        group.MapPost("/login", Login)
            .Produces<BaseResponse<AuthResponse>>()
            .Produces<BaseResponse>(StatusCodes.Status401Unauthorized)
            .Produces<BaseResponse>(StatusCodes.Status400BadRequest);
    }

    [EndpointSummary("Register a new user")]
    [EndpointDescription("Creates a new user account and an associated company.")]
    private static async Task<IResult> Register(RegisterRequest request, AppDbContext db, JwtProvider jwt)
    {
        if (await db.Users.AnyAsync(u => u.Email == request.Email))
            return Results.BadRequest(BaseResponse.Failure("Email already exists."));

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = PasswordHasher.Hash(request.Password)
        };

        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = request.CompanyName
        };

        var membership = new TenantMembership
        {
            Id = Guid.NewGuid(),
            User = user,
            Company = company,
            RoleId = 1 // Owner
        };

        db.Users.Add(user);
        db.Companies.Add(company);
        db.TenantMemberships.Add(membership);

        await db.SaveChangesAsync();

        var token = jwt.Generate(user, company, "Owner");
        var response = new AuthResponse(user.Id, company.Id, token);
        return Results.Ok(BaseResponse<AuthResponse>.Successful(response, "User registered successfully."));
    }

    [EndpointSummary("User login")]
    [EndpointDescription("Authenticates a user and returns a JWT access token.")]
    private static async Task<IResult> Login(LoginRequest request, AppDbContext db, JwtProvider jwt)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
            return Results.Unauthorized();

        // For now, pick the first company if none specified, or just return first for foundation
        var membership = await db.TenantMemberships
            .Include(m => m.Company)
            .Include(m => m.Role)
            .FirstOrDefaultAsync(m => m.UserId == user.Id);

        if (membership == null)
            return Results.BadRequest(BaseResponse.Failure("User has no company memberships."));

        var token = jwt.Generate(user, membership.Company, membership.Role.Name);
        var response = new AuthResponse(user.Id, membership.CompanyId, token);
        return Results.Ok(BaseResponse<AuthResponse>.Successful(response, "Login successful."));
    }
}
