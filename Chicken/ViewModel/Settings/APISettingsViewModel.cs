using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;

namespace Chicken.ViewModel.Settings
{
    public class APISettingsViewModel : PivotItemViewModelBase
    {
        #region event handler
        public Action BeforeSaveHandler;
        #endregion

        #region properties
        private APIProxy setting;
        public APIProxy APISetting
        {
            get
            {
                return setting;
            }
            set
            {
                setting = value;
                RaisePropertyChanged("APISetting");
            }
        }
        private int selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                APITypeChangedAction();
                RaisePropertyChanged("SelectedIndex");
            }
        }
        public APIProxy[] DefaultAPIs
        {
            get
            {
                return APIProxy.DefaultAPIs;
            }
        }
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

        public APISettingsViewModel()
        {
            RefreshHandler = this.RefreshAction;
        }

        #region actions
        private void RefreshAction()
        {
            var settings = IsolatedStorageService.GetAppSettings();
            if (settings != null && settings.APISettings != null)
            {
                APISetting = settings.APISettings;
                SelectedIndex = DefaultAPIs.ToList().IndexOf(App.Settings.APISettings);
            }
            else
            {
                APISetting = new APIProxy
                {
                    Name = APIProxy.DefaultAPIs[0].Name,
                    DisplayName = APIProxy.DefaultAPIs[0].DisplayName,
                    Url = Const.testAPI
                };
            }
            base.Refreshed();
        }

        private void SaveAction()
        {
            if (IsLoading || string.IsNullOrEmpty(APISetting.Url))
                return;
            APISetting.Url = APISetting.Url.Trim('/', '\r', '\n', ' ') + "/";
            if (string.IsNullOrEmpty(APISetting.Url))
                return;
            IsLoading = true;
            if (BeforeSaveHandler != null)
                BeforeSaveHandler();
            TweetService.TestAPIUrl(APISetting.Url,
                userProfileDetail =>
                {
                    IsLoading = false;
                    List<ErrorMessage> errors = userProfileDetail.Errors;
                    if (userProfileDetail.HasError)
                        return;
                    App.InitAPISettings(APISetting);
                    IsolatedStorageService.CreateAuthenticatedUser(userProfileDetail);
                    App.InitAuthenticatedUser();
                    var message = new ToastMessage();
                    message.Message = LanguageHelper.GetString("Toast_Msg_HelloUser", App.AuthenticatedUser.DisplayName);
                    message.Complete = () => NavigationServiceManager.NavigateTo(Const.HomePage);
                    App.HandleMessage(message);
                });
        }

        private void SettingsAction()
        {
            IsLoading = false;
            NavigationServiceManager.NavigateTo(Const.SettingsPage);
        }

        private void APITypeChangedAction()
        {
            if (IsInited)
            {
                APISetting = DefaultAPIs[selectedIndex];
            }
        }
        #endregion
    }
}
