namespace Api.Persistence.Repositories;

/// <summary>Bütün entity-lər üçün ümumi CRUD əməliyyatları (EF Core üzərində).</summary>
public sealed class Repository<T>(AppDbContext db, TimeProvider clock) : IRepository<T> where T : BaseEntity
{
    private DbSet<T> Table => db.Set<T>();

    /// <summary>Id ilə entity-ni oxuyur (tracking olmadan).</summary>
    public Task<T?> GetByIdAsync(Guid id, CancellationToken ct) =>
        Table.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct);

    /// <summary>Şərtə uyğun ilk entity-ni qaytarır.</summary>
    public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct) =>
        Table.AsNoTracking().FirstOrDefaultAsync(predicate, ct);

    /// <summary>Şərtə uyğun entity-nin mövcudluğunu yoxlayır.</summary>
    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct) =>
        Table.AnyAsync(predicate, ct);

    /// <summary>Yeni entity əlavə edir; AuditableEntity-dirsə CreatedAt-i UTC vaxtla doldurur.</summary>
    public async Task AddAsync(T entity, CancellationToken ct)
    {
        if (entity is AuditableEntity auditable)
            auditable.CreatedAt = clock.GetUtcNow().UtcDateTime;

        await Table.AddAsync(entity, ct);
    }

    public Task SaveChangesAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
