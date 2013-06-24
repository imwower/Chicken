using Newtonsoft.Json;

namespace Chicken.Model
{
    public class User : TweetBase
    {
        //[JsonProperty("id_str")]
        //public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("screen_name")]
        public string DisplayName { get; set; }

        [JsonIgnore]
        public string ScreenName
        {
            get
            {
                return "@" + DisplayName;
            }
        }

        [JsonProperty("profile_image_url_https")]
        public string ProfileImage { get; set; }

        /// <summary>
        /// status detail page needs bigger avatar,
        /// so add this property to User,
        /// not to UserProfile.
        /// </summary>
        [JsonIgnore]
        public string ProfileImageBigger
        {
            get
            {
                return ProfileImage.Replace("_normal", "_bigger");
            }
        }

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
        [JsonIgnore]
        public bool IsMyself { get; set; }
    }
}
