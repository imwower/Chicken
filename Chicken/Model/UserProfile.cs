using Newtonsoft.Json;

namespace Chicken.Model
{
    public class UserProfile : User
    {
        [JsonProperty("description")]
        public override string Text { get; set; }
    }

    public class UserProfileList : ModelBaseList<UserProfile>
    {
    }
}
