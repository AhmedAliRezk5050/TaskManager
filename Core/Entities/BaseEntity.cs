using Infrastructure.Data;

namespace Core.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    public string CreatedById { get; set; } = null!;
    public virtual ApplicationUser CreatedBy { get; set; } = null!;

    public string? UpdatedById { get; set; }
    public virtual ApplicationUser? UpdatedBy { get; set; } = null!;
}
