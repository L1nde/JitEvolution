using JitEvolution.Api.Dtos.Identity;
using JitEvolution.Core.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JitEvolution.Api.Controllers.Identity
{
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserTokenDto>> LoginWithPassword([FromBody] PasswordLoginDto dto)
        {
            var (user, token) = await _authenticationService.LoginWithPasswordAsync(dto.Username, dto.Password);

            return new UserTokenDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                },
                Token = token
            };
        }
    }
}
