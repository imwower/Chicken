using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chicken.Model
{
    public class UserIdList : Cursor
    {
        [JsonProperty("ids")]
        public IList<string> UserIds { get; set; }
    }
}
