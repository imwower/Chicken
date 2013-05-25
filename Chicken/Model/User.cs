using Newtonsoft.Json;

namespace Chicken.Model
{
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("profile_image_url")]
        public string ProfileImage { get; set; }
    }
}
