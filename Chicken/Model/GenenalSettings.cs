using System;
using Newtonsoft.Json;

namespace Chicken.Model
{
    public class GeneralSettings
    {
        public string CurrentLanguage { get; set; }

        public APIProxy APISettings { get; set; }
    }

    public class APIProxy
    {
        public virtual string Type { get; set; }

        public bool IsCurrentInUsed { get; set; }

        public string Url { get; set; }
    }
}
