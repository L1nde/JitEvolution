using JitEvolution.Core.Models.Analyzer;

namespace JitEvolution.Core.Repositories.Analyzer.Nodes
{
    public interface IMethodRepository : INodeRepository<Method>
    {
        Task<IEnumerable<Result<Method>>> GetAllAsync(long appId, long classId, string? filter = null);

        Task<IEnumerable<Result<Relationship>>> GetAllRelationshipsAsync(long appId, long classId, string? filter = null);

        Task<Result<Method>?> GetByUsrAsync(string projectId, string usr);

        Task<Result<Method>?> GetAsync(long id);

        Task<Result<Method>?> GetRelatedMethodAsync(long id, string version);

        Task<IEnumerable<(string ClassUsr, Result<Method> Method)>> GetAllWithClassUsrForAsync(string appKey, string version);
        Task<IEnumerable<(string ClassUsr, Result<Method> Method)>> GetAllLatestWithClassUsrAsync(string appKey, string exludeVersion);
    }
}
