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
using Chicken.ViewModel.Home.Base;
using Chicken.Model;

namespace Chicken.ViewModel.Status.VM
{
    public class StatusDetailViewModel : StatusViewModelBase
    {
        private TweetViewModel tweetViewModel;
        public TweetViewModel TweetViewModel
        {
            get
            {
                return tweetViewModel;
            }
            set
            {
                tweetViewModel = value;
                RaisePropertyChanged("TweetViewModel");
            }
        }

        public StatusDetailViewModel()
        {
            Header = "Detail";
        }

        public override void Refresh()
        {
            TweetService.GetStatusDetail<Tweet>(StatusId,
                tweet =>
                {
                    this.TweetViewModel = new TweetViewModel(tweet);
                    base.Refreshed();
                });
        }

        public override void Click(object parameter)
        {
            base.Click(parameter);
        }
    }
}
