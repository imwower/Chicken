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

        public override string Text
        {
            get
            {
                return "#" + DisplayText;
            }
        }

        [JsonProperty("text")]
        public override string DisplayText { get; set; }
    }
}
