using System.Collections.Generic;
using Newtonsoft.Json;

namespace Chicken.Model.Entity
{
    public class Coordinates
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public double[] Points { get; set; }

        public override string ToString()
        {
            return (int)Points[0] + ", " + (int)Points[1];
        }
    }
}
