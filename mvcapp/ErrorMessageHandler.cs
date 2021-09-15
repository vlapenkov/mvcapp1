using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shared;
using Shared.Exceptions;
using Shared.Problems;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mvc.services
{
    public class ErrorMessageHandler : DelegatingHandler
    {
        IHttpContextAccessor _contextAccessor;

        public ErrorMessageHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ProblemDetailsEx problem = null;
            var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {


                if (response.Content.Headers.ContentType != null)

                    switch (response.Content.Headers.ContentType.MediaType.ToUpperInvariant())
                    {
                        case "APPLICATION/PROBLEM+JSON":
                            {
                                string content = await response.Content.ReadAsStringAsync();
                                problem = JsonConvert.DeserializeObject<ProblemDetailsEx>(content);
                                break;
                            }
                        // если не достучаться до сервиса
                        default:
                            {
                                problem = new HttpProblem(response.ReasonPhrase, response.StatusCode, response.RequestMessage?.RequestUri?.AbsoluteUri);
                                break;
                            }

                    }
                else // если Access Denied
                    problem = new HttpProblem(response.ReasonPhrase, response.StatusCode, response.RequestMessage?.RequestUri?.AbsoluteUri);
                throw new ProblemDetailsException(problem);
            }

            return response;
        }
    }
}
