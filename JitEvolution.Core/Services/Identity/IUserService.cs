using JitEvolution.Core.Models.Identity;

namespace JitEvolution.Core.Services.Identity
{
    public interface IUserService
    {
        Task<User> CreateAsync(User user);
    }
}
