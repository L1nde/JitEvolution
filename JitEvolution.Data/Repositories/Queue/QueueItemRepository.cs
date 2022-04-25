using JitEvolution.Core.Models.Queue;
using JitEvolution.Core.Repositories.Queue;
using Microsoft.EntityFrameworkCore;

namespace JitEvolution.Data.Repositories.Queue
{
    internal class QueueItemRepository : BaseCrudRepository<QueueItem>, IQueueItemRepository
    {
        public QueueItemRepository(JitEvolutionDbContext dbContext) : base(dbContext)
        {
        }

        public Task<QueueItem?> GetNextItemAsync()
        {
            var validItems = Queryable
                .GroupBy(x => x.ProjectId)
                .Where(x => !x.Any(y => y.IsActive))
                .Select(x => x.Key);

            return 
                Queryable
                .Where(x => validItems.Contains(x.ProjectId))
                .OrderBy(x => x.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public Task<QueueItem?> GetNextItemForAsync(Guid projectId)
        {
            var validItems = Queryable
                .Where(x => x.ProjectId == projectId)
                .GroupBy(x => x.ProjectId)
                .Where(x => !x.Any(y => y.IsActive))
                .Select(x => x.Key);

            return
                Queryable
                .Where(x => validItems.Contains(x.ProjectId))
                .OrderBy(x => x.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
