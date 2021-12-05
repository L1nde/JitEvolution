using JitEvolution.Api.Dtos;
using JitEvolution.Exceptions;
using Newtonsoft.Json;

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
                await GenerateExceptionResponse(context, ex.Message, ex.GetType().Name, ex.HttpStatusCode);
            }
            catch (Exception ex)
            {
                await GenerateExceptionResponse(context, ex.Message, ex.GetType().Name, 500);
            }
        }

        private static Task GenerateExceptionResponse(HttpContext context, string message, string exceptionType, int statusCode)
        {
            var errorDto = new ErrorResponseDto
            {
                Errors = new Dictionary<string, string>()
                {
                    {"ExceptionType", exceptionType}
                },
                Title = message,
                Status = statusCode
            };

            var result = JsonConvert.SerializeObject(errorDto);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(result);
        }
    }
}
