using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Exceptions
{
    [Serializable]
    public abstract class TneException : Exception
    {
        public TneException() { }
        public TneException(string message) : base(message)
        {
        }

        public TneException(string message, Exception innerException)
        {
        }
    }
}
