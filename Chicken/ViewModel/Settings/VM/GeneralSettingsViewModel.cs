using System.Linq;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;

namespace Chicken.ViewModel.Settings.VM
{
    public class GeneralSettingsViewModel : SettingsViewModelBase
    {
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
        private Language language = App.Settings.CurrentLanguage;
        public Language Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
                RaisePropertyChanged("Language");
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
                LanguageChangedAction();
                RaisePropertyChanged("SelectedIndex");
            }
        }
        public Language[] DefaultLanguages
        {
            get
            {
                return Language.DefaultLanguages;
            }
        }
        #endregion

        #region binding
        public ICommand ClearCacheCommand
        {
            get
            {
                return new DelegateCommand(ClearCacheAction);
            }
        }

        public ICommand EditAPISettingsCommand
        {
            get
            {
                return new DelegateCommand(EditAPISettingsAction);
            }
        }
        #endregion

        public GeneralSettingsViewModel()
        {
            RefreshHandler = this.RefreshAction;
        }

        #region actions
        private void RefreshAction()
        {
            if (App.Settings != null && App.Settings.APISettings != null)
                APISetting = App.Settings.APISettings;
            SelectedIndex = DefaultLanguages.ToList().IndexOf(App.Settings.CurrentLanguage);
            base.Refreshed();
        }

        private void ClearCacheAction()
        {
            if (IsLoading)
                return;
            IsLoading = true;
            IsolatedStorageService.ClearCache();
            App.HandleMessage(new ToastMessage
            {
                Message = LanguageHelper.GetString("Toast_Msg_ClearCacheSuccessfully"),
                Complete = () => IsLoading = false
            });
        }

        private void EditAPISettingsAction()
        {
            IsLoading = false;
            NavigationServiceManager.NavigateTo(Const.APISettingsPage);
        }

        private void LanguageChangedAction()
        {
            if (IsInited)
                App.InitLanguage(DefaultLanguages[selectedIndex]);
        }
        #endregion
    }
}
