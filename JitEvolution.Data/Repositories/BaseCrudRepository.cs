using JitEvolution.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace JitEvolution.Data.Repositories
{
    internal class BaseCrudRepository<TEntity> where TEntity : class, IBaseEntity
    {
        protected readonly JitEvolutionDbContext DbContext;

        public BaseCrudRepository(JitEvolutionDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public IQueryable<TEntity> Queryable =>
            DbContext.Set<TEntity>().AsQueryable();

        public virtual Task<List<TEntity>> ListAsync(IQueryable<TEntity>? queryable = null) =>
            (queryable ?? Queryable).ToListAsync();

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await DbContext.AddAsync(entity);

            return entity;
        }

        public virtual TEntity Remove(TEntity entity)
        {
            DbContext.Remove(entity);

            return entity;
        }

        public virtual Task SaveChangesAsync(CancellationToken ct)
        {
            return DbContext.SaveChangesAsync(ct);
        }

        public virtual Task SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }
    }
}
