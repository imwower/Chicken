﻿using Newtonsoft.Json;

namespace Chicken.Model
{
    public class User : ModelBase
    {
        [JsonProperty("id_str")]
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("screen_name")]
        public string DisplayName { get; set; }

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
        public string ProfileImageBigger
        {
            get
            {
                return ProfileImage.Replace("_normal", "_bigger");
            }
        }

        [JsonProperty("verified")]
        public bool IsVerified { get; set; }

        [JsonProperty("protected")]
        public bool IsPrivate { get; set; }

        [JsonProperty("is_translator")]
        public bool IsTranslator { get; set; }
    }
}
