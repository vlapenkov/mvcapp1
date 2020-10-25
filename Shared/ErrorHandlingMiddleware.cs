using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared;
using System;
using System.Net;
using System.Threading.Tasks;

namespace mvcapp
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ErrorHandlingMiddleware>();
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {


            //if (ex is MyNotFoundException) code = HttpStatusCode.NotFound;
            //else if (ex is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
            //else if (ex is MyException) code = HttpStatusCode.BadRequest;
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            if (!context.Response.HasStarted)
            {
                _logger.LogError(ex.Message +
               ex.StackTrace);

            }
            var result = JsonConvert.SerializeObject(new
                ProblemDetailsEx
            {
                Type ="https://developer.mozilla.org/ru/docs/web/HTTP/Status",
                ErrorLevel = ErrorLevel.Error,
                Status = (int)code,
                Title = "Http error",
                Detail = ex.Message,
                Instance = context.Request.Path

            }

                );
           
            context.Response.ContentType = "application/problem+json";
            
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
