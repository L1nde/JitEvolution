using JitEvolution.Core.Models.Identity;

namespace JitEvolution.Core.Services.Identity
{
    public interface IUserService
    {
        Task<User> CreateAsync(string username, string password, string email);
    }
}
