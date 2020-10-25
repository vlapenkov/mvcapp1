using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Shared.Problems
{
    public class ExceptionProblem : ProblemDetailsEx

    {

        [JsonConstructor]

        private ExceptionProblem()

        {

        }



        public ExceptionProblem(Exception ex)

        {



            this.Title = "System Error";

            this.Type = "ProblemTypes.ExceptionProblem";

            this.ErrorLevel = ErrorLevel.Error;

           
            this.Status = GetStatusCodeFromException(ex);

            this.Detail = ex.Message;

            ExceptionType = ex.GetType().FullName;

        }




        public string ExceptionType { get; set; }




        private int GetStatusCodeFromException(Exception exception)

        {

            var code = HttpStatusCode.InternalServerError; // 500 if unexpected



            if (exception is NotImplementedException) code = HttpStatusCode.NotImplemented;



            //else if (exception is UnauthorizedAccessException) code = HttpStatusCode.Unauthorized;

            //    || exception is ArgumentException

            //    || exception is ArgumentOutOfRangeException) code = HttpStatusCode.BadRequest;



            return (int)code;



        }

    }
}
