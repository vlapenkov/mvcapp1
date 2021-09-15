using Microsoft.Extensions.Logging;
using mvcapp;
using Shared.Exceptions;
using Shared.Problems;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;

namespace Shared
{
    public class DefaultExceptionHandler : IExceptionHandler
    {

        private readonly IProblemDetailsLogger _logger;


        public DefaultExceptionHandler(IProblemDetailsLogger logger)
        {
            _logger = logger;
        }



        public virtual ProblemDetailsEx Handle(Exception ex)
        {
            ProblemDetailsEx problemDetails = null;

            try
            {
                switch (ex)
                {
                    case ProblemDetailsException prEx:
                        {
                            problemDetails = prEx.ProblemDetails;
                            break;
                        }
                    case TneException theEx:
                        {
                            problemDetails = HandleTneException(theEx);
                            break;
                        }

                    default:
                        problemDetails = HandleSystemException(ex);
                        break;
                }
            }
            catch (Exception innerEx) // если что-то случится в этом блоке
            {

            }



            if (problemDetails != null)
            {
                problemDetails.Instance = GetExceptionTargetSite(ex);
                _logger.Log(problemDetails, ex);
            }



            return problemDetails;
        }

        private string GetExceptionTargetSite(Exception ex)
        {
            string targetSite = ex.TargetSite?.DeclaringType?.DeclaringType?.FullName ?? ex.TargetSite?.DeclaringType?.FullName;
            return targetSite;
        }

        private ProblemDetailsEx HandleSystemException(Exception ex)
        {
            ProblemDetailsEx problemDetail;
            // Обработка для refit   

            // Далее switch case  в зависимости от  типа исключений
            // здесь загрушка

            switch (ex)
            {
                case AuthenticationException e1:
                case UnauthorizedAccessException e2:
                    {
                        problemDetail = new AuthorizationProblem(ex.Message);
                        break;
                    }
                //case DbUpdateExcetion e3:
                //    {
                //        problemDetail = new ExceptionProblem(ex.InnerException ?? ex);
                //        break;
                //    }
                default:
                    {
                        var code = HttpStatusCode.InternalServerError; // 500 if unexpected
                        problemDetail = new ProblemDetailsEx
                        {
                            Type = "https://developer.mozilla.org/ru/docs/web/HTTP/Status",
                            ErrorLevel = ErrorLevel.Error,
                            Status = (int)code,
                            Title = "Http error",
                            Detail = ex.Message

                        };

                    }
                    break;
            }


            return problemDetail;





        }

        private ProblemDetailsEx HandleTneException(TneException ex)
        {

            switch (ex)

            {
                case TneValidationException tve:
                    {
                        return new ValidationsProblem(tve.Data);
                        break;
                    }
                case TneErrorException tve:
                    {
                        return new ExceptionProblem(tve);
                        break;
                    }
                default:
                    return new ExceptionProblem(ex);
                    break;
            }
        }
    }
}

