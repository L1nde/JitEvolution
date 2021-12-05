using JitEvolution.BusinessObjects.Identity;
using JitEvolution.Core.Models;
using JitEvolution.Core.Models.Identity;
using JitEvolution.Data.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JitEvolution.Core.Repositories
{
    internal class JitEvolutionDbContext : IdentityDbContext<User, Role, Guid>
    {
        private readonly CurrentUser _currentUser;

        public JitEvolutionDbContext(DbContextOptions<JitEvolutionDbContext> options, CurrentUser currentUser) : base(options)
        {
            _currentUser = currentUser;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(JitEvolutionDbContext).Assembly);

            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(IBaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.SetSoftDeleteFilter();
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is IBaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                var entity = (IBaseEntity)entry.Entity;

                var now = DateTime.UtcNow;

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entity.ChangedAt = now;
                    entity.ChangedById = _currentUser.Id;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = now;
                        entity.CreatedById = _currentUser.Id;
                    }
                }
                else if (entry.State == EntityState.Deleted && entity.DeletedAt == null)
                {
                    entry.State = EntityState.Deleted;

                    entity.DeletedAt = now;
                    entity.DeletedById = _currentUser.Id;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            throw new NotSupportedException("Use method SaveChangesAsync(...)");
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new NotSupportedException("Use method SaveChangesAsync(...)");
        }
    }
}
