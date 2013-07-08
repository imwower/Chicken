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

        public static void NavigateTo(PageNameEnum pageName, object parameter = null)
        {
            if (parameter != null)
                IsolatedStorageService.CreateObject(pageName, parameter);
            string random = "?random=" + DateTime.Now.Ticks.ToString("x");
            switch (pageName)
            {
                case PageNameEnum.SplashScreenPage:
                    frame.Navigate(new Uri(Const.SplashScreenPage + random, UriKind.Relative));
                    break;
                case PageNameEnum.HomePage:
                    frame.Navigate(new Uri(Const.HomePage + random, UriKind.Relative));
                    break;
                case PageNameEnum.ProfilePage:
                    frame.Navigate(new Uri(Const.ProfilePage + random, UriKind.Relative));
                    break;
                case PageNameEnum.StatusPage:
                    frame.Navigate(new Uri(Const.StatusPage + random, UriKind.Relative));
                    break;
                case PageNameEnum.NewTweetPage:
                    frame.Navigate(new Uri(Const.NewTweetPage + random, UriKind.Relative));
                    break;
                case PageNameEnum.NewMessagePage:
                    frame.Navigate(new Uri(Const.NewMessagePage + random, UriKind.Relative));
                    break;
                case PageNameEnum.EditMyProfilePage:
                    frame.Navigate(new Uri(Const.EditMyProfilePage + random, UriKind.Relative));
                    break;
                case PageNameEnum.SettingsPage:
                    frame.Navigate(new Uri(Const.SettingsPage + random, UriKind.Relative));
                    break;
                case PageNameEnum.APISettingsPage:
                    frame.Navigate(new Uri(Const.APISettingsPage + random, UriKind.Relative));
                    break;
                default:
                    break;
            }
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
