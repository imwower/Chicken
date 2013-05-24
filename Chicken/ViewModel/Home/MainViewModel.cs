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
using System.Collections.Generic;
using Microsoft.Phone.Controls;
using Chicken.Common;

namespace Chicken.ViewModel.Home
{
    public class MainViewModel : PivotViewModelBase
    {
        public MainViewModel()
        {
            var baseViewModelList = new List<ViewModelBase>
            {
                new HomeViewModel(),
                new MentionsViewModel(),
                new DMsPivotViewModel(),
            };
            PivotItems = new ObservableCollection<ViewModelBase>(baseViewModelList);
        }

        #region virtual method
        /// <summary>
        /// navigate to profile page
        /// </summary>
        /// <param name="parameter">user id</param>
        public override void Click(object parameter)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>(1);
            parameters.Add(TwitterHelper.USER_ID, parameter);
            string uri = TwitterHelper.GenerateRelativeUri(TwitterHelper.ProfilePage, parameters);
            TwitterHelper.NavigateTo(uri);
        }
        #endregion

        public void MainPivot_LoadedPivotItem(int selectedIndex)
        {
            if (!PivotItems[selectedIndex].IsInited)
            {
                PivotItems[selectedIndex].Refresh();
                PivotItems[selectedIndex].IsInited = true;
            }
        }
    }
}
