using JitEvolution.Core.Models.Analyzer;

namespace JitEvolution.Core.Repositories.Analyzer
{
    public interface IClassRepository
    {
        Task<IEnumerable<Result<Class>>> GetAllAsync(long appId, string? filter = null);
    }
}
