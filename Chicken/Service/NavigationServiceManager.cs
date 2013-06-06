using System;
using System.Windows;
using System.Windows.Controls;
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

        public static void NavigateTo(Const.PageNameEnum pageName, object parameter = null)
        {
            if (parameter != null)
            {
                IsolatedStorageService.CreateObject(pageName, parameter);
            }
            switch (pageName)
            {
                case Const.PageNameEnum.MainPage:
                    frame.Navigate(new Uri(Const.MainPage, UriKind.Relative));
                    break;
                case Const.PageNameEnum.ProfilePage:
                    frame.Navigate(new Uri(Const.ProfilePage, UriKind.Relative));
                    break;
                case Const.PageNameEnum.StatusPage:
                    frame.Navigate(new Uri(Const.StatusPage, UriKind.Relative));
                    break;
                case Const.PageNameEnum.NewTweetPage:
                    frame.Navigate(new Uri(Const.NewTweetPage, UriKind.Relative));
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
