using JitEvolution.Core.Models.Analyzer;

namespace JitEvolution.Core.Repositories.Analyzer
{
    public interface IMethodRepository
    {
        Task<IEnumerable<Result<Method>>> GetAll(long appId, long classId, string? filter = null);

        Task<IEnumerable<Result<Relationship>>> GetAllRelationships(long appId, long classId, string? filter = null);
    }
}
