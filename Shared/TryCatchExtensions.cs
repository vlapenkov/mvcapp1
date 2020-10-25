using mvcapp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
   public static class TryCatchExtensions
    {

        public static IExceptionHandler Handler { get; set; } = null;

        public static async Task<(T data, ProblemDetailsEx error)> TryCatch<T>(this Task<T> task)
        {
            var handler = Handler;

            return await TryCatch(task, handler.Handle);
        }

        public static async Task<(T data, ProblemDetailsEx error)> TryCatch<T>(this Task<T> task,Func<Exception,ProblemDetailsEx> func) {

            try
            {
                return (await task, (ProblemDetailsEx)null);
            }
            catch (Exception ex)
            {
                ProblemDetailsEx problemDetails = func(ex);
                return (default(T), problemDetails);
            }
        }
    }
}
