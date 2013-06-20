using System;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;

namespace Chicken.ViewModel.Settings
{
    public class APISettingsViewModel : PivotItemViewModelBase
    {
        #region properties
        private GeneralSettings generalSettings;
        public GeneralSettings GeneralSettings
        {
            get
            {
                return generalSettings;
            }
            set
            {
                generalSettings = value;
                RaisePropertyChanged("GeneralSettings");
            }
        }
        private bool isInitAPI;
        #endregion

        #region binding
        public ICommand SaveCommand
        {
            get
            {
                return new DelegateCommand(SaveAction);
            }
        }
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public APISettingsViewModel()
        {
            Header = "Edit API settings";
            RefreshHandler = this.RefreshAction;
        }

        #region actions
        private void RefreshAction()
        {
            GeneralSettings = App.Settings;
            if (GeneralSettings == null)
            {
                Header = "Init API settings";
                isInitAPI = true;
            }
            base.Refreshed();
        }

        private void SaveAction()
        {
            if (string.IsNullOrEmpty(GeneralSettings.APISettings.Url) ||
                GeneralSettings.APISettings.Type == App.Settings.APISettings.Type ||
                GeneralSettings.APISettings.Url == App.Settings.APISettings.Url)
            {
                return;
            }
            IsLoading = true;
            if (!GeneralSettings.APISettings.Url.EndsWith("/"))
            {
                GeneralSettings.APISettings.Url += "/";
            }
            try
            {
                TweetService.TestAPIUrl<UserProfileDetail>(GeneralSettings.APISettings.Url,
                    userProfileDetail =>
                    {
                        App.UpdateAuthenticatedUser(userProfileDetail);
                        App.UpdateAppSettings(GeneralSettings);
                        HandleMessage(new ToastMessage
                        {
                            Message = isInitAPI ? "hello, " + userProfileDetail.ScreenName : "update successfully",
                            Complete =
                            () =>
                            {
                                NavigationServiceManager.NavigateTo(PageNameEnum.MainPage);
                            }
                        });
                    });
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }
        #endregion
    }
}
