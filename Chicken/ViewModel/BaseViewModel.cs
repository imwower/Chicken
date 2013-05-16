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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Controls;

namespace Chicken.ViewModel
{
    public abstract class BaseViewModel : NotificationObject
    {
        ScrollViewer scrollViewer;

        #region properties

        private string header;
        public string Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
                RaisePropertyChanged("Header");
            }
        }
        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        private ObservableCollection<TweetViewModel> tweetList = new ObservableCollection<TweetViewModel>();
        public ObservableCollection<TweetViewModel> TweetList
        {
            get
            {
                return tweetList;
            }
            set
            {
                if (value != tweetList)
                {
                    tweetList = value;
                    RaisePropertyChanged("TweetList");
                }
            }
        }

        #endregion

        public BaseViewModel()
        {
            TweetList = new ObservableCollection<TweetViewModel>();
        }

        public ICommand TextBlock_LayoutUpdatedCommand
        {
            get
            {
                return new DelegateCommand(TextBlock_LayoutUpdated);
            }
        }

        public virtual void GetNewTweets()
        { }

        private void TextBlock_LayoutUpdated(object sender)
        {
            scrollViewer = Utils.FindVisualElement<ScrollViewer>(sender as DependencyObject);
            if (scrollViewer != null)
            {
                scrollViewer.MouseMove += new MouseEventHandler(scrollViewer_MouseMove);
                scrollViewer.MouseLeftButtonUp += new MouseButtonEventHandler(scrollViewer_MouseLeftButtonUp);
            }
        }

        private void scrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            UIElement scrollContent = (UIElement)scrollViewer.Content;
            CompositeTransform ct = scrollContent.RenderTransform as CompositeTransform;

        }

        private void scrollViewer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //ctf.TranslateY = Height = 0;
        }
    }
}
