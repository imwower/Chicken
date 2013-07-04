using Newtonsoft.Json;

namespace Chicken.Model
{
    public class DirectMessage : Tweet
    {
        [JsonProperty("sender")]
        public override User User { get; set; }

        [JsonProperty("recipient")]
        public User Receiver { get; set; }
    }

    public class DirectMessageList : ModelBaseList<DirectMessage>
    {
    }
}
