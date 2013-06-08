using Newtonsoft.Json;

namespace Chicken.Model.Entity
{
    public class UserMention : EntityBase
    {
        [JsonProperty("id_str")]
        public string Id { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }
    }
}
