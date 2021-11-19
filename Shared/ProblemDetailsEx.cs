using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared
{

    public enum ErrorLevel
    {
        Unknown = 0,
        Warning,
        Error
    }


    public class ProblemDetailsEx
    {


        public string Type { get; set; }

        // [JsonConverter(typeof(StringEnumConverter))]
        public ErrorLevel ErrorLevel { get; set; }

        public int Status { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }

        [Newtonsoft.Json.JsonExtensionData]
        public virtual IDictionary<string, object> Extensions { get; set; } = new Dictionary<string, object>();
    }
}
