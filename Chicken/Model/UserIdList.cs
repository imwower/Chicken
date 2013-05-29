using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chicken.Model
{
    public class UserIdList : BulkList
    {
        [JsonProperty("ids")]
        public string[] UserIds { get; set; }
    }
}
