using JitEvolution.Exceptions;
using JitEvolution.Services.Errors;

namespace JitEvolution.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BaseException ex)
            {
                await ErrorResponseWriter.GenerateExceptionResponse(context, ex);
            }
            catch (Exception ex)
            {
                await ErrorResponseWriter.GenerateExceptionResponse(context, ex.Message, ex.GetType().Name, 500);
            }
        }
    }
}
