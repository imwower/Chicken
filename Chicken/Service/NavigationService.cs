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
using Microsoft.Phone.Controls;
using System.Collections.Generic;

namespace Chicken.Service
{
    public static class NavigationService
    {
        static PhoneApplicationFrame frame;
        static bool isInited;
        static Dictionary<string, string> dic = new Dictionary<string, string>();

        public const string ProfilePage = "ProfilePage";


        public static void NavigateTo(Uri uri)
        {
            if (!isInited)
            {
                Init();
            }
            frame.Navigate(uri);
        }

        public static void NavigateTo(string pageName, string parameter = "")
        {
            if (!isInited)
            {
                Init();
            }
            if (string.IsNullOrEmpty(parameter))
            {
                frame.Navigate(new Uri(dic[pageName], UriKind.Relative));
            }
            else
            {
                frame.Navigate(new Uri(dic[pageName] + parameter, UriKind.Relative));
            }
        }

        private static void Init()
        {
            frame = null;
            frame = Application.Current.RootVisual as PhoneApplicationFrame;
            dic.Add(ProfilePage, "/View/ProfilePage.xaml");
            //
            isInited = true;
        }
    }
}
