using JitEvolution.BusinessObjects.Identity;
using System.Security.Claims;

namespace JitEvolution.Api.Middlewares
{
    public class CurrentUserMiddleWare
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, CurrentUser currentUser)
        {
            var currentUserId = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrWhiteSpace(currentUserId) && Guid.TryParse(currentUserId, out var userId))
            {
                currentUser.Id = userId;
            }

            await _next(httpContext);
        }
    }
}
