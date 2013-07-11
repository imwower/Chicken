using Newtonsoft.Json;

namespace Chicken.Model
{
    public class UserProfile : User
    {
        /// <summary>
        /// description
        /// </summary>
        [JsonProperty("description")]
        public string Text { get; set; }
    }

    public class UserProfileList : ModelBaseList<UserProfile>
    {
    }
}
