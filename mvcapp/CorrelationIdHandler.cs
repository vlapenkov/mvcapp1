using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace mvcapp
{
    public class CorrelationIdHandler : DelegatingHandler
    {
        IHttpContextAccessor _contextAccessor;
        private readonly CorrelationIdOptions _options;

        public CorrelationIdHandler(IHttpContextAccessor contextAccessor, IOptions<CorrelationIdOptions> options)
        {
            _contextAccessor = contextAccessor;
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_contextAccessor.HttpContext.Items.TryGetValue(_options.Header, out object correlationId))
                request.Headers.Add(_options.Header, correlationId.ToString());
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
