using JitEvolution.Core.Models.Queue;

namespace JitEvolution.Core.Repositories.Queue
{
    public interface IQueueItemRepository : IBaseCrudRepository<QueueItem>
    {
        Task<QueueItem?> GetNextItemAsync();

        Task<QueueItem?> GetNextItemForAsync(Guid projectId);
    }
}
