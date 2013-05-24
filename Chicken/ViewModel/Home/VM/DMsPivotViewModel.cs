using System.Collections.ObjectModel;
using Chicken.Service;
using Chicken.ViewModel.Home.Base;

namespace Chicken.ViewModel.Home.VM
{
    public class DMsPivotViewModel : HomeViewModelBase
    {
        private ObservableCollection<DirectMessageViewModel> dmList;
        public ObservableCollection<DirectMessageViewModel> DMList
        {
            get
            {
                return dmList;
            }
            set
            {
                dmList = value;
                RaisePropertyChanged("DMList");
            }
        }

        public DMsPivotViewModel()
        {
            Header = "Messages";
            DMList = new ObservableCollection<DirectMessageViewModel>();
        }

        public override void Refresh()
        {
            var messages = TweetService.GetDirectMessages();
            foreach (var message in messages)
            {
                DMList.Add(new DirectMessageViewModel(message));
            }
            base.Refreshed();
        }
    }
}
