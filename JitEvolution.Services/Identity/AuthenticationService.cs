using JitEvolution.Config;
using JitEvolution.Core.Models.Identity;
using JitEvolution.Core.Services.Identity;
using JitEvolution.Exceptions.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JitEvolution.Services.Identity
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Jwt _jwtConfig;

        public AuthenticationService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<Configuration> options)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfig = options.Value.Jwt;
        }

        public async Task<(User User, string Token)> LoginWithPasswordAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                throw new AuthenticationException("Authentication failed: No such user found!");
            }

            var result = await _signInManager.PasswordSignInAsync(username, password, true, false);

            if (!result.Succeeded)
            {
                throw new AuthenticationException("Wrong username or password!");
            }

            return (user, GenerateJwt(user));
        }
        private string GenerateJwt(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            return GenerateJwt(claims);
        }

        private string GenerateJwt(IEnumerable<Claim> claims)
        {
            // todo: Can add roles into token

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));

            var token = new JwtSecurityToken(
                _jwtConfig.Issuer,
                _jwtConfig.Audience,
                claims,
                expires: DateTime.UtcNow.AddHours(_jwtConfig.ExpireHours),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateAccessKey(User user)
        {
            var accessKey = new byte[16];
            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(accessKey);

            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user.UserName}:").Concat(accessKey.AsEnumerable()).ToArray());
        }
    }
}
