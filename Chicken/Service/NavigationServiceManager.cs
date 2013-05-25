﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Chicken.Service.Interface;
using Microsoft.Phone.Controls;
using Chicken.Common;
using System;

namespace Chicken.Service
{
    public class NavigationServiceManager
    {
        #region properties
        static PhoneApplicationFrame Frame = Application.Current.RootVisual as PhoneApplicationFrame;
        #endregion

        public static void NavigateTo(string pageName, IDictionary<string, object> parameters = null)
        {
            if (string.IsNullOrEmpty(pageName))
            {
                return;
            }
            string url = TwitterHelper.GenerateRelativeUri(pageName, parameters);
            Frame.Navigate(new Uri(url, UriKind.Relative));
        }

        public static void ChangeSelectedIndex(int selectedIndex, IDictionary<string, object> parameters = null)
        {
            var navigator = Frame.Content as INavigationService;
            navigator.ChangeSelectedIndex(selectedIndex, parameters);
        }
    }
}
