using Newtonsoft.Json;

namespace Chicken.Model.Entity
{
    public class Coordinates
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public double[] Points { get; set; }

        public double X
        {
            get
            {
                return Points[0];
            }
        }

        public double Y
        {
            get
            {
                return Points[1];
            }
        }

        public override string ToString()
        {
            return (int)X + ", " + (int)Y;
        }
    }
}
