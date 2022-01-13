using JitEvolution.Core.Models.Analyzer;

namespace JitEvolution.Core.Repositories.Analyzer
{
    public interface IVariableRepository
    {
        Task<IEnumerable<Result<Variable>>> GetAll(long appId, long classId, string? filter = null);
    }
}
