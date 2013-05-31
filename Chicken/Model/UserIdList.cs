using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Chicken.Model
{
    [DefaultValue(null)]
    public class UserIdList : Cursor
    {
        [JsonProperty("ids")]
        public string[] UserIds { get; set; }
    }
}
