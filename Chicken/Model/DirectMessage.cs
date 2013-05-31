using Newtonsoft.Json;

namespace Chicken.Model
{
    public class DirectMessage : ModelBase
    {
        [JsonProperty("id_str")]
        public string Id { get; set; }

        public string Text { get; set; }

        public User Sender { get; set; }

        [JsonProperty("created_at")]
        public string CreatedDate { get; set; }
    }

    public class DirectMessageList<T> : ModelBaseList<T> where T : DirectMessage
    { }
}
