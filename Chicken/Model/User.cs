using Newtonsoft.Json;

namespace Chicken.Model
{
    public class User : ModelBase
    {
        [JsonProperty("id_str")]
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("profile_image_url")]
        public string ProfileImage { get; set; }
    }
}
