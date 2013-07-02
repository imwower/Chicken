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

        public override string Text
        {
            get
            {
                return "@" + DisplayName;
            }
        }

        [JsonProperty("id_str")]
        public override string Id { get; set; }

        [JsonProperty("screen_name")]
        public override string DisplayName { get; set; }
    }
}
