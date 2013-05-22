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
using System.Threading;

namespace Chicken.ViewModel.Profile
{
    public class ProfileViewModelBase : ViewModelBase
    {
        #region properties
        public string UserId;
        #endregion

        #region binding Command
        //public ICommand RefreshCommand
        //{
        //    get
        //    {
        //        return new DelegateCommand(RefreshDispatcher);
        //    }
        //}
        #endregion

        #region dispatcher
        //private void RefreshDispatcher(object parameter)
        //{
        //    IsLoading = true;
        //    timer = new Timer(
        //        (obj) =>
        //        {
        //            Deployment.Current.Dispatcher.BeginInvoke(
        //                () =>
        //                {
        //                    Refresh(parameter as string);
        //                    IsLoading = false;
        //                });
        //        }, null, 1000, -1);
        //}
        #endregion

        #region virtual method
        //public virtual void Refresh(string userId)
        //{
        //}
        #endregion
    }
}
