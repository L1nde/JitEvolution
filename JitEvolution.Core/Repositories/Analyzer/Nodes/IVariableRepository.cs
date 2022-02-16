using JitEvolution.Core.Models.Analyzer;

namespace JitEvolution.Core.Repositories.Analyzer.Nodes
{
    public interface IVariableRepository : INodeRepository<Variable>
    {
        Task<IEnumerable<Result<Variable>>> GetAllAsync(long appId, long classId, string? filter = null);

        Task<Result<Variable>?> GetByUsrAsync(string projectId, string usr);

        Task<Result<Variable>?> GetAsync(long id);
    }
}
