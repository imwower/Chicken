using System.Windows.Input;
using System.Xml.Linq;
using Chicken.Model;
using Chicken.Service;

namespace Chicken.ViewModel.Settings.VM
{
    public class AboutViewModel : SettingsViewModelBase
    {
        #region properties
        public string CurrentVersion
        {
            get
            {
                return GetVersionNumber();
            }
        }
        private AboutModel aboutModel;
        public AboutModel AboutModel
        {
            get
            {
                return aboutModel;
            }
            set
            {
                aboutModel = value;
                RaisePropertyChanged("AboutModel");
            }
        }
        #endregion

        #region binding
        public ICommand UpdateDescriptionCommand
        {
            get
            {
                return new DelegateCommand(UpdateDescriptionAction);
            }
        }
        #endregion

        public AboutViewModel()
        {
            Header = "About";
            RefreshHandler = this.RefreshAction;
        }

        #region actions
        private void RefreshAction()
        {
            AboutModel = IsolatedStorageService.GetAbout();
            base.Refreshed();
        }

        private void UpdateDescriptionAction()
        { }
        #endregion

        #region private
        private static string GetVersionNumber()
        {
            return XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;
        }
        #endregion
    }
}
