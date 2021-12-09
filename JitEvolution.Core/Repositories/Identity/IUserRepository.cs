using JitEvolution.Core.Models.Identity;

namespace JitEvolution.Core.Repositories.Identity
{
    public interface IUserRepository
    {
        Task<User?> GetByApiKeyAsync(string accessKey);

        Task<User> AddAsync(User user);

        Task SaveChangesAsync();
    }
}
