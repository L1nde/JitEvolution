using JitEvolution.Core.Models.Identity;

namespace JitEvolution.Core.Repositories.Identity
{
    public interface IUserRepository : IBaseCrudRepository<User>
    {
        Task<User?> GetByApiKeyAsync(string accessKey);
    }
}
