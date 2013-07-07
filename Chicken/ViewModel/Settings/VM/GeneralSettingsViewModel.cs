using System.Windows.Input;
using Chicken.Model;
using Chicken.Service;

namespace Chicken.ViewModel.Settings.VM
{
    public class GeneralSettingsViewModel : SettingsViewModelBase
    {
        #region properties
        private APIProxy setting;
        public APIProxy APISettings
        {
            get
            {
                return setting;
            }
            set
            {
                setting = value;
                RaisePropertyChanged("APISettings");
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
                if (language != value)
                {
                    language = value;
                    LanguageChangedAction();
                    RaisePropertyChanged("Language");
                }
            }
        }
        public Language[] defaultLanguages;
        public Language[] DefaultLanguages
        {
            get
            {
                return defaultLanguages;
            }
            set
            {
                defaultLanguages = value;
                RaisePropertyChanged("DefaultLanguages");
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
            Header = "General";
            RefreshHandler = this.RefreshAction;
        }

        #region actions
        private void RefreshAction()
        {
            if (App.Settings != null && App.Settings.APISettings != null)
                APISettings = App.Settings.APISettings;
            DefaultLanguages = Language.DefaultLanguages;
            base.Refreshed();
        }

        private void ClearCacheAction()
        {
            //TODO:clear cache
        }

        private void EditAPISettingsAction()
        {
            IsLoading = false;
            NavigationServiceManager.NavigateTo(Common.PageNameEnum.APISettingsPage);
        }

        private void LanguageChangedAction()
        {
            this.SetLanguage(Language);
        }
        #endregion
    }
}
