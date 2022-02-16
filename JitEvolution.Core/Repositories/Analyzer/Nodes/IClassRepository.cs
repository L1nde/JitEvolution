using JitEvolution.Core.Models.Analyzer;

namespace JitEvolution.Core.Repositories.Analyzer.Nodes
{
    public interface IClassRepository : INodeRepository<Class>
    {
        Task<IEnumerable<Result<Class>>> GetAllAsync(long appId, string? filter = null);

        Task<Result<Class>?> GetByUsrAsync(string projectId, string usr);

        Task<Result<Class>?> GetAsync(long id);
    }
}
