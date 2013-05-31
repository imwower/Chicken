using Newtonsoft.Json;

namespace Chicken.Model
{
    public class ModelBase
    {
        [JsonProperty("errors")]
        public ErrorMessage[] Errors { get; set; }
    }

    public class ErrorMessage
    {
        public string Message { get; set; }

        public string Code { get; set; }
    }
}
