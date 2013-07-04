﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service;
using Chicken.Service.Interface;
using Chicken.ViewModel.Base;
using Chicken.ViewModel.Status.VM;

namespace Chicken.ViewModel.Status
{
    public class StatusViewModel : PivotViewModelBase
    {
        #region properites
        private TweetViewModel tweet;
        #endregion

        #region binding
        public ICommand AddToFavoriteCommand
        {
            get
            {
                return new DelegateCommand(AddToFavorite);
            }
        }

        public ICommand RetweetCommand
        {
            get
            {
                return new DelegateCommand(Retweet);
            }
        }

        public ICommand ReplyCommand
        {
            get
            {
                return new DelegateCommand(Reply);
            }
        }

        public ICommand QuoteCommand
        {
            get
            {
                return new DelegateCommand(Quote);
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new DelegateCommand(DeleteAction);
            }
        }
        #endregion

        #region services
        public ITweetService TweetService = TweetServiceManager.TweetService;
        #endregion

        public StatusViewModel()
        {
            Title = "Status";
            var baseViewModelList = new List<PivotItemViewModelBase>()
            {
                new StatusDetailViewModel(),
                new StatusRetweetsViewModel(),
                new StatusViewModelBase(),
            };
            PivotItems = new ObservableCollection<PivotItemViewModelBase>(baseViewModelList);
        }

        public override void MainPivot_LoadedPivotItem(int selectedIndex)
        {
            if (!IsInit)
            {
                PivotItems[selectedIndex].IsLoading = true;
                string statusId = IsolatedStorageService.GetObject<string>(PageNameEnum.StatusPage);
                TweetService.GetStatusDetail<Tweet>(statusId,
                    data =>
                    {
                        if (data.User.Id == App.AuthenticatedUser.Id)
                        {
                            data.IsSentByMe = true;
                        }
                        this.tweet = new TweetViewModel(data);
                        SwitchAppBar(selectedIndex);
                        IsInit = true;
                    });
            }
            else
            {
                SwitchAppBar(selectedIndex);
            }
        }

        #region actions
        private void AddToFavorite()
        {
            (PivotItems[SelectedIndex] as StatusViewModelBase).AddFavorite();
        }

        private void Retweet()
        {
            (PivotItems[SelectedIndex] as StatusViewModelBase).Retweet();
        }

        private void Reply()
        {
            (PivotItems[SelectedIndex] as StatusViewModelBase).Reply();
        }

        private void Quote()
        {
            (PivotItems[SelectedIndex] as StatusViewModelBase).Quote();
        }

        private void DeleteAction()
        {
            (PivotItems[SelectedIndex] as StatusViewModelBase).Delete();
        }
        #endregion

        #region private
        private void SwitchAppBar(int selectedIndex)
        {
            if (tweet.IsSentByMe)
            {
                State = AppBarState.StatusPageWithDelete;
            }
            else
            {
                State = AppBarState.StatusPageDefault;
            }
            (PivotItems[selectedIndex] as StatusViewModelBase).Tweet = tweet;
            base.MainPivot_LoadedPivotItem(selectedIndex);
        }
        #endregion
    }
}
