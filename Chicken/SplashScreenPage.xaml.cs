﻿using System.Windows;
using Chicken.Common;
using Chicken.Service;
using Microsoft.Phone.Controls;

namespace Chicken
{
    public partial class SplashScreenPage : PhoneApplicationPage
    {
        public SplashScreenPage()
        {
            InitializeComponent();
            this.Loaded += SplashScreenPage_Loaded;
        }

        private void SplashScreenPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Settings == null
                || App.Settings.APISettings == null)
                NavigationServiceManager.NavigateTo(PageNameEnum.APISettingsPage);
            else
                NavigationServiceManager.NavigateTo(PageNameEnum.HomePage);
        }
    }
}