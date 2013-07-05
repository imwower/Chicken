using System;
using System.Windows;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;
using Microsoft.Phone.Controls;

namespace Chicken
{
    public partial class SplashScreenPage : PhoneApplicationPage
    {
        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public SplashScreenPage()
        {
            InitializeComponent();
            this.Loaded += SplashScreenPage_Loaded;
        }

        private void SplashScreenPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Settings == null)
            {
                NavigationServiceManager.NavigateTo(PageNameEnum.APISettingsPage);
            }
            else
            {
                //if (App.Configuration == null ||
                //    App.Configuration.LastUpdateTime.Date < DateTime.Now.Date)
                //{
                //TweetService.GetTweetConfiguration(
                //        configuration =>
                //        {
                //            configuration.LastUpdateTime = DateTime.Now;
                //            IsolatedStorageService.CreateTweetConfiguration(configuration);
                //            App.InitConfiguration();
                //            NavigationServiceManager.NavigateTo(PageNameEnum.HomePage);
                //        });
                //}
                //else
                //{
                NavigationServiceManager.NavigateTo(PageNameEnum.HomePage);
                //}
            }
        }
    }
}