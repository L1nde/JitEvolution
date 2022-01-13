using JitEvolution.Core.Models.Analyzer;

namespace JitEvolution.Core.Repositories.Analyzer
{
    public interface IAppRepository
    {
        Task<IEnumerable<Result<App>>> GetAll();

        Task<Result<App>> Get(long appId);
    }
}
