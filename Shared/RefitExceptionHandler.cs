using Newtonsoft.Json;
using Shared;
using System;

public class RefitExceptionHandler : IExceptionHandler

{

    public ProblemDetailsEx Handle(Exception ex)

    {

        ProblemDetailsEx problemDetails = null;

        dynamic refitApiException = ex;


        if (ex.GetType().FullName == "Refit.ApiException")
        {
            if (!string.IsNullOrWhiteSpace(refitApiException.Content ?? ""))
            {
                problemDetails = DeserializeContent<ProblemDetailsEx>(refitApiException.Content);

                if (problemDetails == null)

                {
                }
            }
        }





        if (ex.GetType().FullName == "Refit.ValidationApiException") //  если ContentType == Application/Problem* то refit возвращает ValidationApiException, и Contnet is Refit.Problemdetail

        {

            //todo:  по хоруошему вообще отключить Refit.Problemdetail - они нам только мешают.

            problemDetails = DeserializeContent<ProblemDetailsEx>(refitApiException.Content);// TneProblemDetails.ConvertTo<ProblemDetailsEx>(refitApiException.Content);

            problemDetails.ErrorLevel = ErrorLevel.Error;

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