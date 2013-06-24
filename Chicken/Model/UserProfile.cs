
using Newtonsoft.Json;
namespace Chicken.Model
{
    public class UserProfile : User
    {
        [JsonProperty("description")]
        public override string Text { get; set; }

        [JsonIgnore]
        public string TruncatedDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(Text) && Text.Length > 60)
                {
                    return Text.Substring(0, 60) + "...";
                }
                return Text;
            }
        }
    }

    public class UserProfileList<T> : ModelBaseList<T> where T : UserProfile
    { }
}
