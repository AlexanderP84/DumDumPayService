using Newtonsoft.Json;
using System.Collections.Generic;

namespace DumDumPayService.Models
{
    public class Error
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class ErrorResult
    {
        [JsonProperty("errors")]
        public List<Error> Errors { get; set; }
    }
}
