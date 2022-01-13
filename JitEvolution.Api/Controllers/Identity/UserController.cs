using JitEvolution.Api.Dtos.Identity;
using JitEvolution.BusinessObjects.Identity;
using JitEvolution.Core.Services.Identity;
using JitEvolution.Data.Constants.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JitEvolution.Api.Controllers.Identity
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly CurrentUser _currentUser;

        public UserController(IUserService userService, CurrentUser currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<UserDto> Register(RegisterDto dto)
        {
            using (_currentUser.Overrider(UserConstants.AnonymousUserId))
            {
                var user = await _userService.CreateAsync(dto.Username, dto.Password, dto.Email);

                return new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName
                };
            }
        }
    }
}
