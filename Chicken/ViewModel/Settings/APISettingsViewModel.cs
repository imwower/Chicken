using System.Collections.Generic;
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

        public ICommand SettingsCommand
        {
            get
            {
                return new DelegateCommand(SettingsAction);
            }
        }
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public APISettingsViewModel()
        {
            Header = "Edit API";
            RefreshHandler = this.RefreshAction;
        }

        #region actions
        private void RefreshAction()
        {
            if (App.Settings == null)
            {
                isInitAPI = true;
                GeneralSettings = new GeneralSettings
                {
                    APISettings = new APIProxy
                    {
                        Url = Const.testAPI
                    }
                };
            }
            else
            {
                GeneralSettings = App.Settings;
            }
            base.Refreshed();
        }

        private void SaveAction()
        {
            if (IsLoading
                || string.IsNullOrEmpty(GeneralSettings.APISettings.Url))
                return;
            IsLoading = true;
            generalSettings.APISettings.Url = GeneralSettings.APISettings.Url.TrimEnd('/') + "/";
            TweetService.TestAPIUrl(GeneralSettings.APISettings.Url,
                userProfileDetail =>
                {
                    IsLoading = false;
                    List<ErrorMessage> errors = userProfileDetail.Errors;
                    if (errors != null && errors.Count != 0)
                    {
                        HandleMessage(new ToastMessage
                        {
                            Message = errors[0].Message
                        });
                    }
                    else
                    {
                        IsolatedStorageService.CreateAppSettings(GeneralSettings);
                        App.InitAppSettings();
                        IsolatedStorageService.CreateAuthenticatedUser(userProfileDetail);
                        App.InitAuthenticatedUser();
                        GetConfiguration();
                    }
                });
        }

        private void SettingsAction()
        {
            NavigationServiceManager.NavigateTo(PageNameEnum.SettingsPage);
        }
        #endregion

        #region private
        private void GetConfiguration()
        {
            //TweetService.GetTweetConfiguration(
            //    configuration =>
            //    {
            //configuration.LastUpdateTime = DateTime.Now;
            //IsolatedStorageService.CreateTweetConfiguration(configuration);
            //App.InitConfiguration();
            HandleMessage(new ToastMessage
            {
                Message = isInitAPI ? "hello, " + App.AuthenticatedUser.DisplayName : "update successfully",
                Complete =
                () =>
                {
                    NavigationServiceManager.NavigateTo(PageNameEnum.HomePage);
                }
            });
            //});
        }
        #endregion
    }
}
