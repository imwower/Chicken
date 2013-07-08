using System.Globalization;
using System.Linq;
using System.Threading;
using Chicken.Model;
using Chicken.Resources;

namespace Chicken.ViewModel
{
    public class LanguageHelper : NotificationObject
    {
        public void InitLanguage()
        {
            Helper.InitLanguage();
            RaisePropertyChanged("Item[]");
        }

        public string this[string key]
        {
            get
            {
                return Helper.GetString(key);
            }
        }

        public static string GetString(string key, params string[] parameters)
        {
            return string.Format(Helper.GetString(key), parameters);
        }

        private class Helper
        {
            #region property
            private static bool isInit;
            #endregion

            public static void InitLanguage()
            {
                isInit = false;
            }

            public static string GetString(string key)
            {
                if (!isInit)
                    InitCurrentCulture();
                return AppResources.ResourceManager.GetString(key);
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
}
