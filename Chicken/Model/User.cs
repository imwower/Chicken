using Newtonsoft.Json;
using System.ComponentModel;

namespace Chicken.Model
{
    public class User : ModelBase
    {
        [JsonProperty("id_str")]
        public string Id { get; set; }

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
        [DefaultValue(false)]
        public bool IsFollowing { get; set; }

        [JsonProperty("verified")]
        public bool IsVerified { get; set; }

        [JsonProperty("protected")]
        public bool IsPrivate { get; set; }

        [JsonProperty("is_translator")]
        public bool IsTranslator { get; set; }
    }
}
