using System;
using System.Windows;
using Chicken.Common;
using Chicken.Service.Interface;
using Microsoft.Phone.Controls;

namespace Chicken.Service
{
    public class NavigationServiceManager
    {
        #region properties
        private static PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
        #endregion

        public static void NavigateTo(string pageName, object parameter = null)
        {
            if (parameter != null)
                IsolatedStorageService.CreateObject(pageName, parameter);
            string random = "?random=" + DateTime.Now.Ticks.ToString("x");
            frame.Navigate(new Uri(pageName + random, UriKind.Relative));
        }

        public static void ChangeSelectedIndex(int selectedIndex)
        {
            var navigator = frame.Content as INavigationService;
            navigator.ChangeSelectedIndex(selectedIndex);
        }

        public static void NavigateBack()
        {
            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
        }
    }
}
