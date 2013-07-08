using System.Globalization;
using System.Linq;
using System.Threading;
using Chicken.Model;
using Chicken.Resources;

namespace Chicken.ViewModel
{
    public class LanguageHelper : NotificationObject
    {
        private static Helper helper;

        public void InitLanguage()
        {
            helper = null;
            RaisePropertyChanged("Item[]");
        }

        public string this[string key]
        {
            get
            {
                if (helper == null)
                    helper = new Helper();
                return helper[key];
            }
        }

        public static string GetString(string key, params string[] parameters)
        {
            if (helper == null)
                helper = new Helper();
            return string.Format(helper[key], parameters);
        }

        private class Helper
        {
            public Helper()
            {
                var ci = CultureInfo.CurrentUICulture;
                if (App.Settings != null && App.Settings.CurrentLanguage != null)
                    ci = new CultureInfo(App.Settings.CurrentLanguage.Name);
                else
                {
                    //ci.Name default is "zh-CN":
                    var lang = Language.DefaultLanguages.FirstOrDefault(d => d.Name == ci.Name);
                    //not zh-CN:
                    if (lang == null)
                        lang = Language.DefaultLanguages.First(d => d.Name == "en-US");
                    App.InitLanguage(lang);
                }
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }

            public string this[string key]
            {
                get
                {
                    return AppResources.ResourceManager.GetString(key);
                }
            }
        }
    }
}
