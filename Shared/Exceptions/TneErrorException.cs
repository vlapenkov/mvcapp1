using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Exceptions
{
    public class TneErrorException : TneException
    {
        public TneErrorException(string message) : base(message)
        {
        }
    }
}
