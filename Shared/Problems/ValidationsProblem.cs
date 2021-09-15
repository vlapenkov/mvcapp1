using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Shared.Problems
{
    public class ValidationsProblem : ProblemDetailsEx
    {
        public ValidationsProblem(IDictionary<string, List<string>> errors)
        {
            this.InitData(errors);
            //  Errors = errors;
        }

        public IDictionary<string, List<string>> Errors { get; set; }

        private void InitData(IDictionary<string, List<string>> errors)
        {
            this.Title = "Некорректные данные";
            this.Type = "ProblemType.ValidationsProblem";
            this.ErrorLevel = ErrorLevel.Error;
            this.Detail = "Некорректные данные";
            this.Status = (int)HttpStatusCode.BadRequest;
            this.Errors = errors;
            //this.C
        }
    }
}
