using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class RefitExceptionHandler : IExceptionHandler
    {
        public ProblemDetailsEx Handle(Exception ex)
        {
            ProblemDetailsEx problemDetails = null;

           

            if (ex.GetType().FullName == "Refit.ApiException")

            { 

                if (!string.IsNullOrWhiteSpace(( ex as ApiException).Content))

                {

                    problemDetails = DeserializeContent<ProblemDetailsEx>((ex as ApiException).Content);



                    if (problemDetails == null)

                    {

                        //todo: значит не ProblemDetailsEx

                    }



                }



            }



            if (ex.GetType().FullName == "Refit.ValidationApiException") //  если ContentType == Application/Problem* то refit возвращает ValidationApiException, и Contnet is Refit.Problemdetail

            {
                Refit.ProblemDetails problemDet = (ex as ValidationApiException).Content;

                problemDetails = new ProblemDetailsEx
                {
                    Title = problemDet.Title,
                    ErrorLevel = ErrorLevel.Error,
                    Status = problemDet.Status,
                    Type = problemDet.Type,
                    Detail = problemDet.Detail,
                    Instance = problemDet.Instance

                };
                //todo:  по хоруошему вообще отключить Refit.Problemdetail - они нам только мешают.

                
            }

            return problemDetails;

        }



       



        private static T DeserializeContent<T>(string content)

        {

            if (typeof(T) == typeof(string))

            {

                return (T)(object)content;

            }



            try

            {

                return JsonConvert.DeserializeObject<T>(content);

            }

            catch (Exception)

            {

                return default(T);

            }

        }

    }

}

