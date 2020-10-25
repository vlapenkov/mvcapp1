using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Shared.Problems
{
    public class AuthorizationProblem : ProblemDetailsEx

    {

        public AuthorizationProblem(string detail)

        {



            this.Title = "Permission Error";

            this.Type = "ProblemTypes.AuthorizationProblem";

            this.ErrorLevel = ErrorLevel.Warning;

           // this.Category = ProblemCategory.Authentication;



            this.Status = (int)HttpStatusCode.Unauthorized;

            this.Detail = detail;

        }

    }
}
