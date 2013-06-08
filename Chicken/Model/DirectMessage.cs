using Newtonsoft.Json;

namespace Chicken.Model
{
    public class DirectMessage : Tweet
    {
        [JsonProperty("sender")]
        public override User User { get; set; }
    }

    public class DirectMessageList<T> : ModelBaseList<T> where T : DirectMessage
    { }
}
