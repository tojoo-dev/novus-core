using Microsoft.EntityFrameworkCore;
using Novus.Domain;
using Novus.Infrastructure.Database;
using System.Security.Claims;

namespace Novus.Features.Members;

public class MembersEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/companies/{companyId}/members").WithTags("Members").RequireAuthorization();

        group.MapPost("/", async (Guid companyId, AddMemberRequest request, AppDbContext db) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                // In a real app, we might create a placeholder or invite. 
                // For this foundation, we expect the user to exist globally.
                return Results.BadRequest("User not found globally. Please register the user first.");
            }

            var exists = await db.TenantMemberships.AnyAsync(m => m.CompanyId == companyId && m.UserId == user.Id);
            if (exists) return Results.BadRequest("User is already a member of this company.");

            var membership = new TenantMembership
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                CompanyId = companyId,
                RoleId = request.RoleId
            };

            db.TenantMemberships.Add(membership);
            await db.SaveChangesAsync();

            return Results.Ok(new { membership.Id });
        });

        group.MapGet("/", async (Guid companyId, AppDbContext db) =>
        {
            var members = await db.TenantMemberships
                .Where(m => m.CompanyId == companyId)
                .Include(m => m.User)
                .Include(m => m.Role)
                .Select(m => new
                {
                    m.User.Id,
                    m.User.FullName,
                    m.User.Email,
                    Role = m.Role.Name
                })
                .ToListAsync();

            return Results.Ok(members);
        });

        app.MapGet("/users/me/companies", async (AppDbContext db, ClaimsPrincipal userPrincipal) =>
        {
            var userId = Guid.Parse(userPrincipal.FindFirstValue("user_id")!);

            var companies = await db.TenantMemberships
                .Where(m => m.UserId == userId)
                .Include(m => m.Company)
                .Include(m => m.Role)
                .Select(m => new
                {
                    m.Company.Id,
                    m.Company.Name,
                    Role = m.Role.Name
                })
                .ToListAsync();

            return Results.Ok(companies);
        }).RequireAuthorization().WithTags("Members");
    }

    public record AddMemberRequest(string Email, int RoleId);
}
