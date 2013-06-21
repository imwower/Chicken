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
            if (App.Settings == null)
            {
                Header = "Init API settings";
                isInitAPI = true;
                GeneralSettings = new GeneralSettings
                {
                    APISettings = new APIProxy
                    {
                        Url = "https://blog-lonzhu.rhcloud.com/weixin/o/UT3I3O/1.1/"
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
            if (string.IsNullOrEmpty(GeneralSettings.APISettings.Url))
            {
                return;
            }
            IsLoading = true;
            if (!GeneralSettings.APISettings.Url.EndsWith("/"))
            {
                GeneralSettings.APISettings.Url += "/";
            }
            TweetService.TestAPIUrl<UserProfileDetail>(GeneralSettings.APISettings.Url,
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
                            HandleMessage(new ToastMessage
                            {
                                Message = isInitAPI ? "hello, " + userProfileDetail.ScreenName : "update successfully",
                                Complete =
                                () =>
                                {
                                    NavigationServiceManager.NavigateTo(PageNameEnum.HomePage);
                                }
                            });
                        }
                    });
        }
        #endregion
    }
}
