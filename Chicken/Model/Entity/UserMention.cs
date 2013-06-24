using Newtonsoft.Json;

namespace Chicken.Model.Entity
{
    public class UserMention : EntityBase
    {
        public override EntityType EntityType
        {
            get
            {
                return EntityType.UserMention;
            }
        }

        [JsonProperty("id_str")]
        public string Id { get; set; }

        [JsonProperty("screen_name")]
        public string DisplayName { get; set; }

        public override string Text
        {
            get
            {
                return "@" + DisplayName;
            }
        }
    }
}
