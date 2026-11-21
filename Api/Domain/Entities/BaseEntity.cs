namespace Api.Domain.Entities;

/// <summary>Bütün entity-lərin ortaq bazası: Guid Id və yaradılma vaxtı.</summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
}

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }
}