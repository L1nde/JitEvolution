using JitEvolution.BusinessObjects;
using JitEvolution.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace JitEvolution.Services.Errors
{
    public class ErrorResponseWriter
    {
        public static Task GenerateExceptionResponse(HttpContext context, BaseException exception)
        {
            return GenerateExceptionResponse(context, exception.Message, exception.GetType().Name, exception.HttpStatusCode);
        }

        public static Task GenerateExceptionResponse(HttpContext context, string message, string exceptionType, int statusCode)
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
