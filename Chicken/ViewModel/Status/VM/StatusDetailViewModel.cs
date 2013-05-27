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
using System.Collections.ObjectModel;

namespace Chicken.ViewModel.Status.VM
{
    public class StatusDetailViewModel : StatusViewModelBase
    {
        #region properties
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

        private ObservableCollection<TweetViewModel> conversationList;
        public ObservableCollection<TweetViewModel> ConversationList
        {
            get
            {
                return conversationList;
            }
            set
            {
                conversationList = value;
                RaisePropertyChanged("ConversationList");
            }
        }
        #endregion

        public StatusDetailViewModel()
        {
            Header = "Detail";
        }

        public override void Refresh()
        {
            TweetService.GetStatusDetail<Tweet>(StatusId,
                tweet =>
                {
                    if (tweet != null)
                    {
                        this.TweetViewModel = new TweetViewModel(tweet);
                        if (this.tweetViewModel.InReplyToTweetId != null)
                        {
                            Load();
                        }
                    }
                    base.Refreshed();
                });
        }

        public override void Load()
        {
            if (this.ConversationList == null)
            {
                this.ConversationList = new ObservableCollection<TweetViewModel>();
            }
            base.Loaded();
        }

        public override void Click(object parameter)
        {
            base.Click(parameter);
        }
    }
}
