using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.ViewModel.Home.VM;

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

        public void MainPivot_LoadedPivotItem()
        {
            if (!PivotItems[SelectedIndex].IsInited)
            {
                PivotItems[SelectedIndex].Refresh();
            }
        }
    }
}
