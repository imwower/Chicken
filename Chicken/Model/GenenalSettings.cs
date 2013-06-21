namespace Chicken.Model
{
    public class GeneralSettings
    {
        public APIProxy APISettings { get; set; }

        public GeneralSettings()
        {
            APISettings = new APIProxy();
        }
    }

    public class APIProxy
    {
        public virtual string Type { get; set; }

        public bool IsCurrentInUsed { get; set; }

        public string Url { get; set; }
    }
}
