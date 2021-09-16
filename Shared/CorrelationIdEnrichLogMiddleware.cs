using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class CorrelationIdEnrichLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CorrelationIdOptions _options;
        private readonly ILogger<CorrelationIdEnrichLogMiddleware> _logger;

        public CorrelationIdEnrichLogMiddleware(
            RequestDelegate next,
            IOptions<CorrelationIdOptions> options,
            ILogger<CorrelationIdEnrichLogMiddleware> logger
            )
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next ?? throw new ArgumentNullException(nameof(next));

            _options = options.Value;
            _logger = logger;
        }

        public Task Invoke(HttpContext context)
        {

            if (context.Items.TryGetValue(_options.Header, out object correlationId))
            {
                using (LogContext.PushProperty("CorrelationId", correlationId))
                {
                    return _next.Invoke(context);
                }
            }
            else
                return _next.Invoke(context);

        }
    }
}
