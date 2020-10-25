using mvcapp;
using System;

namespace Shared
{
    public interface IExceptionHandler
    {
        ProblemDetailsEx Handle(Exception ex);
    }
}