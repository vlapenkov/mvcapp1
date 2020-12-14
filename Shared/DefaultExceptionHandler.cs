using Microsoft.Extensions.Logging;
using mvcapp;
using Shared.Problems;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Shared
{
   public class DefaultExceptionHandler :IExceptionHandler
    {
        private Exception _exception;

        private readonly ILogger _logger;


        public DefaultExceptionHandler(ILogger<DefaultExceptionHandler> logger)
        {
            _logger = logger;
        }



        public virtual ProblemDetailsEx Handle(Exception ex)
        {
            _exception = ex;

            ProblemDetailsEx problemDetails;

            try

            {



                switch (ex)

                {

                    //case ProblemDetailsException pEx:

                    //    problemDetails = (ProblemDetailsEx)pEx.ProblemDetails;

                    //    break;

                    //case TneException tnepEx:

                    //    problemDetails = HandleTneException(tnepEx);

                    //    break;

                    case HttpRequestException httpEx:

                        problemDetails = HandleHttpRequestException(httpEx);

                        break;

                    default:

                        problemDetails = HandleSystemException(ex);

                        break;

                }

            }

            catch (Exception innerEx)

                {

                // todo: sntr: как бы желательно сохранить и обрабатываемый Exception и этот и все залогировать. т.е. нужен свой спец. класс exception.

                _exception = ex;



                //todo: вынести в DeveloperProblem

                problemDetails = new ProblemDetailsEx

                {

                    Type = "about:blank",

                    Detail = "Exception in ExceptionHandler: " + _exception.Message,

                    ErrorLevel = ErrorLevel.Error,

                    Status = 500,

                    Title = "ExceptionHandler Error"

                };

            }





            problemDetails.Instance = GetExceptionTargetSite(_exception);

           



            // todo: добавить логирование

            if (problemDetails != null)

            {

                var logLevel = problemDetails.ErrorLevel == ErrorLevel.Warning ? LogLevel.Warning : LogLevel.Error;
                _logger.Log(logLevel, _exception, "@problemDetails", problemDetails);

            }



            return problemDetails;



        }



        private string GetExceptionTargetSite(Exception ex)

        {

            string targetSite = ex?.TargetSite?.DeclaringType?.DeclaringType?.FullName

               ?? ex?.TargetSite?.DeclaringType?.FullName;



            return targetSite;

        }



        //todo: надо реализовать некий мапинг.

        private ProblemDetailsEx HandleSystemException(Exception ex)

        {



            ProblemDetailsEx problemDetail;



       // todo: нужен какой то универсальный подход для обработки подобных случаев.

       IExceptionHandler h = new RefitExceptionHandler(); // todo: sntr: как бы надо брать из DI.

            problemDetail = h.Handle(ex);



            if (problemDetail != null)

            {

                return problemDetail;

            }



            switch (ex)

            {

                //  case AuthenticationException e1:

                case UnauthorizedAccessException e2:

                    problemDetail = new AuthorizationProblem(ex.Message);

                    break;

                //case ArgumentException e:

                //case NotImplementedException e2:

                //    problemDetail = new ApplicationProblem(ex.Message);

                //    break;

                default:

                    problemDetail = new ExceptionProblem(ex);

                    break;

            }





              return problemDetail;

            }





            #region TneExсeptions



            //private ProblemDetailsEx HandleTneException(TneException ex)

            //{

            //    ProblemDetailsEx problemDetails;

            //    switch (ex)

            //    {

            //        case TneNotFoundException tneEx:

            //            problemDetails = new NotFoundProblem(tneEx.Message); //HandleTneNotFoundException(tneEx);

            //            break;

            //        case TneWarningException tneEx:

            //            problemDetails = new WarningProblem(tneEx.Message);

            //            break;

            //        case TneErrorException tneEx:

            //            problemDetails = new ErrorProblem(tneEx.Message);

            //            break;

            //        default:

            //            problemDetails = new ErrorProblem(ex.Message);

            //            break;

            //    }



            //    return problemDetails;

            //}





            #endregion // end TneExсeptions





            private static ProblemDetailsEx HandleHttpRequestException(HttpRequestException ex)

            {

                //todo: sntr: надо как то получить Url чего вызывали.

                return new HttpProblem(ex.Message, HttpStatusCode.BadGateway, ex.Source);

            }

        }
}

