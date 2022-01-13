using JitEvolution.Core.Models.Identity;
using JitEvolution.Core.Services.Identity;
using Microsoft.AspNetCore.Identity;

namespace JitEvolution.Services.Identity
{
    internal class UserService : IUserService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _userManager = userManager;
            _authenticationService = authenticationService;
        }

        public async Task<User> CreateAsync(string username, string password, string email)
        {
            var user = new User
            {
                UserName = username,
                Email = email,
                AccessKey = _authenticationService.GenerateAccessKey(username)
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join("; ", result.Errors.Select(x => x.Description)));
            }

            return user;
        }
    }
}
