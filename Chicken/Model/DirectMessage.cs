using Newtonsoft.Json;

namespace Chicken.Model
{
    public class DirectMessage
    {  
        public string Id { get; set; }

        public string Text { get; set; }

        public User Sender { get; set; }

        [JsonProperty("created_at")]
        public string CreatedDate { get; set; }
    }
}
