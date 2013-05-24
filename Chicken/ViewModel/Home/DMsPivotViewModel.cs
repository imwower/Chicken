using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Chicken.Service;

namespace Chicken.ViewModel.Home
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
