namespace Api.Persistence.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
    Task AddAsync(T entity, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
