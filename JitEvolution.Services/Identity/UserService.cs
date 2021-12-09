using JitEvolution.Core.Models.Identity;
using JitEvolution.Core.Repositories.Identity;
using JitEvolution.Core.Services.Identity;

namespace JitEvolution.Services.Identity
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateAsync(User user)
        {
            await _userRepository.AddAsync(user);

            await _userRepository.SaveChangesAsync();

            return user;
        }
    }
}
