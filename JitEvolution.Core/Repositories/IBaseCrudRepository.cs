using JitEvolution.Core.Models;

namespace JitEvolution.Core.Repositories
{
    public interface IBaseCrudRepository<TEntity> where TEntity : class, IBaseEntity
    {
        IQueryable<TEntity> Queryable { get; }

        Task<List<TEntity>> ListAsync(IQueryable<TEntity>? queryable = null);

        Task<TEntity> AddAsync(TEntity user);

        TEntity Remove(TEntity entity);

        Task SaveChangesAsync();

        Task SaveChangesAsync(CancellationToken ct);
    }
}
