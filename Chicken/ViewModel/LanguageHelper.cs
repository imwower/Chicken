using System.Globalization;
using System.Linq;
using System.Threading;
using Chicken.Model;
using Chicken.Resources;

namespace Chicken.ViewModel
{
    public class LanguageHelper : NotificationObject
    {
        #region property
        private static bool isInit;
        #endregion

        public string this[string key]
        {
            get
            {
                if (!isInit)
                    InitCurrentCulture();
                return AppResources.ResourceManager.GetString(key);
            }
        }

        public string FormatString(string key, params string[] parameters)
        {
            return string.Format(this[key], parameters);
        }

        public void InitLanguage()
        {
            isInit = false;
            RaisePropertyChanged("Item[]");
        }

        #region private method
        private static void InitCurrentCulture()
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
            isInit = true;
        }
        #endregion
    }
}
