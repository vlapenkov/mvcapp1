using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Exceptions
{
    public class TneValidationException : TneException
    {
        public TneValidationException(IDictionary<string, List<string>> data)
        {
            Data = data;
        }
        public TneValidationException(string message) : base(message) { }

        public TneValidationException(string message, string fieldname) : this(message)
        {
            Data.Add(fieldname, new List<string> { message });
        }


        public IDictionary<string, List<string>> Data { get; set; } = new Dictionary<string, List<string>>();


    }
}
