using System;

namespace Novus.Domain;

public class TenantMembership
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public int RoleId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User User { get; set; } = null!;
    public Company Company { get; set; } = null!;
    public Role Role { get; set; } = null!;
}
