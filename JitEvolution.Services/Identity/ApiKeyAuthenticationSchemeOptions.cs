using Microsoft.AspNetCore.Authentication;

namespace JitEvolution.Services.Identity
{
    public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "API_KEY";
        public string Scheme => DefaultScheme;
        public string AuthenticationType => DefaultScheme;
    }
}
