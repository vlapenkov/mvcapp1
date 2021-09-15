using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared;
using Shared.Exceptions;
using Shared.Problems;
using System;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace mvcapp
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate next;
        private readonly IExceptionHandler _defaultHandler;
        public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IExceptionHandler defaultHandler)
        {
            _defaultHandler = defaultHandler;
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

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (context.Response.HasStarted) return;

            var problemDetails = _defaultHandler.Handle(ex);

            // var result = JsonConvert.SerializeObject(problemDetails);

            if (problemDetails == null) return;
            //problemDetails.Extensions = null;
            var result = new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status, //?? context.Response.StatusCode,
                DeclaredType = problemDetails.GetType()

            };

            // var resAsString = JsonConvert.SerializeObject(result);
            // context.Items["Error"] = result;
            context.Response.ContentType = "application/problem+json";
            // context.Response.ContentType = "application/json";

            //  context.Response.StatusCode = (int)problemDetails.Status;
            // context.Response.WriteAsync(resAsString);

            var routeData = context.GetRouteData() ?? new RouteData();
            var emptyActionDescriptor = new ActionDescriptor();
            var actionContext = new ActionContext(context, routeData, emptyActionDescriptor);

            await result.ExecuteResultAsync(actionContext);
        }


    }
}
