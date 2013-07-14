using Newtonsoft.Json;

namespace Chicken.Model.Entity
{
    public class HashTag : EntityBase
    {
        public override EntityType EntityType
        {
            get
            {
                return EntityType.HashTag;
            }
        }

        [JsonProperty("text")]
        public string DisplayText { get; set; }

        public override string Text
        {
            get
            {
                return "#" + DisplayText;
            }
        }
    }
}
