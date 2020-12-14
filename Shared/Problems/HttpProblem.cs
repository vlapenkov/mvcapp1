using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Shared.Problems
{
   public class HttpProblem : ProblemDetailsEx
    {
        public HttpProblem(string detail, HttpStatusCode status, string requestUri)
        {
            this.Title = "Network Error";
            this.Type = "ProblemTypes.NetworkProblem";
            this.ErrorLevel = ErrorLevel.Error;
            this.Status = (int)status;
            this.Detail = detail;
            this.Instance = requestUri;
        }
    }
}
