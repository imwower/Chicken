using System.Windows;
using System.Windows.Navigation;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Chicken
{
    public partial class App : Application
    {
        public PhoneApplicationFrame RootFrame { get; private set; }
        public static User AuthenticatedUser { get; private set; }
        //public static Size GetScreenSize()
        //{
        //    return Application.Current.RootVisual.RenderSize;
        //}

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
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

        public static void UpdateAuthenticatedUser(User user)
        {
            IsolatedStorageService.CreateAuthenticatedUser(user);
            AuthenticatedUser = IsolatedStorageService.GetAuthenticatedUser();
        }

        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            if (AuthenticatedUser == null)
                AuthenticatedUser = IsolatedStorageService.GetAuthenticatedUser();
            if (AuthenticatedUser == null)
                TweetService.GetMyProfileDetail<User>(
                    profile =>
                    {
                        AuthenticatedUser = profile;
                        IsolatedStorageService.CreateAuthenticatedUser(AuthenticatedUser);
                    });
        }

        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (AuthenticatedUser == null)
                AuthenticatedUser = IsolatedStorageService.GetAuthenticatedUser();
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