using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

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
