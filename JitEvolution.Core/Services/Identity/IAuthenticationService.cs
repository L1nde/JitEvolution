using JitEvolution.Core.Models.Identity;

namespace JitEvolution.Core.Services.Identity
{
    public interface IAuthenticationService
    {
        Task<(User User, string Token)> LoginWithPasswordAsync(string username, string password);

        string GenerateAccessKey(string username);
    }
}
