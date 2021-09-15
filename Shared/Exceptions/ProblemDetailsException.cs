
using System;
using System.Collections.Generic;
using System.Text;


namespace Shared.Exceptions
{
    public class ProblemDetailsException : Exception
    {
        public ProblemDetailsException(ProblemDetailsEx problemDetails)
        {
            ProblemDetails = problemDetails;
        }

        public ProblemDetailsEx ProblemDetails { get; set; }
    }
}
