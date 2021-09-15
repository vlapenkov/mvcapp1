using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public interface IProblemDetailsLogger
    {
        void Log(ProblemDetailsEx problemDetail, Exception ex);
    }
}
