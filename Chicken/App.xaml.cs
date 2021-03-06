﻿using System;
using System.Windows;
using System.Windows.Navigation;
using Chicken.Model;
using Chicken.Service;
using Chicken.View;
using Chicken.ViewModel;
#if !RELEASE
using MemoryDiagnostics;
#endif
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace Chicken
{
    public partial class App : Application
    {
        #region properites
        public PhoneApplicationFrame RootFrame { get; private set; }
        private static Brush accentBrush;
        public static Brush PhoneAccentBrush
        {
            get
            {
                if (accentBrush == null)
                    accentBrush = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
                return accentBrush;
            }
        }
        private static Brush foregroundBrush;
        public static Brush ForegroundBrush
        {
            get
            {
                if (foregroundBrush == null)
                    foregroundBrush = Application.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush;
                return foregroundBrush;
            }
        }

        private static FontFamily fontFamily;
        public static FontFamily FontFamily
        {
            get
            {
                if (fontFamily == null)
                    fontFamily = Application.Current.Resources["FontFamily"] as FontFamily;
                return fontFamily;
            }
        }
        private static UserProfileDetail authenticatedUser;
        public static UserProfileDetail AuthenticatedUser
        {
            get
            {
                if (authenticatedUser == null)
                {
                    InitAuthenticatedUser();
                }
                return authenticatedUser;
            }
        }
        private static GeneralSettings settings;
        public static GeneralSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    InitAppSettings();
                }
                return settings;
            }
        }
        #endregion

        public App()
        {
            UnhandledException += Application_UnhandledException;
            InitializeComponent();
            InitializePhoneApplication();
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Application.Current.Host.Settings.EnableFrameRateCounter = true;
#if !RELEASE
                MemoryDiagnosticsHelper.Start(TimeSpan.FromMilliseconds(500), true);
#endif
#if RELEASE
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
#endif
            }
        }

        #region public method
        public static void InitAuthenticatedUser()
        {
            authenticatedUser = IsolatedStorageService.GetAuthenticatedUser();
        }

        public static void InitAppSettings()
        {
            settings = IsolatedStorageService.GetAppSettings();
        }

        public static void InitAPISettings(APIProxy api)
        {
            if (Settings == null)
                settings = new GeneralSettings();
            settings.APISettings = api;
            IsolatedStorageService.CreateAppSettings(settings);
        }

        public static void InitLanguage(Language language)
        {
            if (Settings == null)
                settings = new GeneralSettings();
            settings.CurrentLanguage = language;
            IsolatedStorageService.CreateAppSettings(settings);
            var helper = App.Current.Resources["LanguageHelper"] as LanguageHelper;
            helper.InitLanguage();
        }

        public static void HandleMessage(ToastMessage message)
        {
            PageBase.HandleMessage(message);
        }
        #endregion

        #region Application
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            var brush = App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
        }

        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            var brush = App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
        }

        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }
        #endregion

        #region
        private bool phoneApplicationInitialized = false;

        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;
            phoneApplicationInitialized = true;
        }

        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }
        #endregion
    }
}