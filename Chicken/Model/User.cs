using Newtonsoft.Json;

namespace Chicken.Model
{
    public class User : ModelBase
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        public string CreatedDate { get; set; }

        /// <summary>
        /// with @
        /// </summary>
        [JsonIgnore]
        public string DisplayName
        {
            get
            {
                return "@" + ScreenName;
            }
        }

        [JsonProperty("profile_image_url_https")]
        public string ProfileImage { get; set; }

        [JsonProperty("following")]
        [JsonConverter(typeof(StringToBooleanConverter))]
        public bool IsFollowing { get; set; }

        [JsonProperty("verified")]
        public bool IsVerified { get; set; }

        [JsonProperty("protected")]
        public bool IsPrivate { get; set; }

        [JsonProperty("is_translator")]
        public bool IsTranslator { get; set; }

        /// <summary>
        /// for profile page,
        /// only
        /// </summary>
        public bool IsMyself { get; set; }
    }
}
