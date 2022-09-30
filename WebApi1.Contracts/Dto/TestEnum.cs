using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WebApi1.Contracts.Dto
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TestEnum
    {
        [EnumMember(Value = "success value")]
        Success,
        [EnumMember(Value = "fail value")]
        Fail,
        [EnumMember(Value = "error value")]
        Error
    }
}
