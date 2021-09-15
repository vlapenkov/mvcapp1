using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Shared
{
    public class ProblemDetailsLogger : IProblemDetailsLogger
    {
        ILoggerFactory _loggerFactory;


        public ProblemDetailsLogger(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public void Log(ProblemDetailsEx problemDetail, Exception exception)
        {
            string categoryName = exception?.TargetSite?.DeclaringType?.DeclaringType?.FullName
                ?? exception?.TargetSite?.DeclaringType?.FullName ?? typeof(ProblemDetailsLogger).FullName;
            var logLevel = problemDetail.ErrorLevel == ErrorLevel.Warning ? LogLevel.Warning : LogLevel.Error;

            ILogger logger = _loggerFactory.CreateLogger(categoryName);

            logger.Log(logLevel, exception, "{@problemDetails}", problemDetail);

        }
    }
}
