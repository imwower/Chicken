using System.Windows;
using System.Windows.Navigation;
using Chicken.Model;
using Chicken.Service;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Chicken
{
    public partial class App : Application
    {
        #region properites
        public PhoneApplicationFrame RootFrame { get; private set; }
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
            private set
            {
                authenticatedUser = value;
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
            private set
            {
                settings = value;
            }
        }
        //public static Size GetScreenSize()
        //{
        //    return Application.Current.RootVisual.RenderSize;
        //} 
        #endregion

        public App()
        {
            UnhandledException += Application_UnhandledException;
            InitializeComponent();
            InitializePhoneApplication();
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Application.Current.Host.Settings.EnableFrameRateCounter = true;
#if RELEASE
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled; 
#endif
            }
        }

        public static void InitAuthenticatedUser()
        {
            authenticatedUser = IsolatedStorageService.GetAuthenticatedUser();
        }

        public static void InitAppSettings()
        {
            settings = IsolatedStorageService.GetAppSettings();
        }

        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
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