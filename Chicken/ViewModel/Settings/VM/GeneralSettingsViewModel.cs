using System.Windows.Input;
using Chicken.Model;
using Chicken.Service;

namespace Chicken.ViewModel.Settings.VM
{
    public class GeneralSettingsViewModel : SettingsViewModelBase
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
            GeneralSettings = App.Settings;
            base.Refreshed();
        }

        private void ClearCacheAction()
        {
            //TODO
        }

        private void EditAPISettingsAction()
        {
            IsLoading = false;
            NavigationServiceManager.NavigateTo(Common.PageNameEnum.APISettingsPage);
        }
        #endregion
    }
}
