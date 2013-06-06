using System;
using System.Collections.Generic;
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
        static PhoneApplicationFrame Frame = Application.Current.RootVisual as PhoneApplicationFrame;
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
                    Frame.Navigate(new Uri(Const.MainPage, UriKind.Relative));
                    break;
                case Const.PageNameEnum.ProfilePage:
                    Frame.Navigate(new Uri(Const.ProfilePage, UriKind.Relative));
                    break;
                case Const.PageNameEnum.StatusPage:
                    Frame.Navigate(new Uri(Const.StatusPage, UriKind.Relative));
                    break;
                case Const.PageNameEnum.NewTweetPage:
                    Frame.Navigate(new Uri(Const.NewTweetPage, UriKind.Relative));
                    break;
                default:
                    break;
            }
        }

        public static void ChangeSelectedIndex(int selectedIndex)
        {
            var navigator = Frame.Content as INavigationService;
            navigator.ChangeSelectedIndex(selectedIndex);
        }
    }
}
