namespace JitEvolution.Api.Middlewares
{
    public class CorsMiddleware
    {
        private readonly RequestDelegate _next;

        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue("Origin", out var origin);
            httpContext.Request.Headers.TryGetValue("Access-Control-Request-Headers", out var allowHeaders);

            httpContext.Response.Headers.Add("Access-Control-Allow-Origin", origin);
            httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            httpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count");
            httpContext.Response.Headers.Add("Access-Control-Allow-Headers", allowHeaders);
            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PUT,PATCH,DELETE,OPTIONS");
            httpContext.Response.Headers.Add("Access-Control-Max-Age", "600");

            if (httpContext.Request.Method == "OPTIONS")
            {
                await httpContext.Response.WriteAsync("");
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}
