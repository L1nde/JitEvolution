using JitEvolution.Core.Models.Analyzer;

namespace JitEvolution.Core.Repositories.Analyzer
{
    public interface IMethodRepository
    {
        Task<IEnumerable<Result<Method>>> GetAllAsync(long appId, long classId, string? filter = null);

        Task<IEnumerable<Result<Relationship>>> GetAllRelationshipsAsync(long appId, long classId, string? filter = null);
    }
}
