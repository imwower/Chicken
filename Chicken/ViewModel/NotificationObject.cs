using System.ComponentModel;
using System.Globalization;
using System.Threading;
using Chicken.Model;
using Chicken.Resources;
using Chicken.Service;

namespace Chicken.ViewModel
{
    public class NotificationObject : INotifyPropertyChanged
    {
        #region event handler
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region property
        private static bool isInit;
        #endregion

        #region public method
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string this[string key]
        {
            get
            {
                if (!isInit)
                    InitCurrentCulture();
                return AppResources.ResourceManager.GetString(key);
            }
        }

        public void SetLanguage(string language)
        {
            var settings = App.Settings;
            if (settings == null)
                settings = new GeneralSettings();
            settings.CurrentLanguage = language;
            IsolatedStorageService.CreateAppSettings(settings);
            App.InitAppSettings();
            isInit = false;
            RaisePropertyChanged("Item[]");
        }
        #endregion

        #region private method
        private static void InitCurrentCulture()
        {
            var ci = CultureInfo.CurrentUICulture;
            if (App.Settings != null
                && !string.IsNullOrEmpty(App.Settings.CurrentLanguage))
                ci = new CultureInfo(App.Settings.CurrentLanguage);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            isInit = true;
        }
        #endregion
    }
}
