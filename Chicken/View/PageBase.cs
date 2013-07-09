using System;
using System.Windows.Navigation;
using Chicken.Controls;
using Chicken.Model;
using Chicken.Service.Interface;
using Chicken.ViewModel;
using Microsoft.Phone.Controls;

namespace Chicken.View
{
    public class PageBase : PhoneApplicationPage
    {
        #region static property
        private static ToastPrompt prompt;
        private static Action promptComplete;
        #endregion

        #region message handler
        public static void HandleMessage(ToastMessage message)
        {
            if (prompt == null)
                prompt = new ToastPrompt();
            prompt.Message = message.Message;
            if (message.Complete != null)
            {
                promptComplete = message.Complete;
                prompt.Completed += prompt_Completed;
            }
            prompt.Show();
        }

        private static void prompt_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (promptComplete == null)
                return;
            promptComplete();
            promptComplete = null;
        }
        #endregion
    }

    public class PivotPageBase : PageBase, INavigationService
    {
        #region property
        protected virtual Pivot Pivot { get; private set; }
        protected virtual PivotViewModelBase PivotViewModelBase { get; private set; }
        #endregion

        protected virtual void Init()
        {
            if (Pivot != null)
                Pivot.LoadedPivotItem += MainPivot_LoadedPivotItem;
        }

        protected virtual void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            if (PivotViewModelBase != null)
            {
                PivotViewModelBase.SelectedIndex = (sender as Pivot).SelectedIndex;
                PivotViewModelBase.MainPivot_LoadedPivotItem();
            }
        }

        public virtual void ChangeSelectedIndex(int selectedIndex)
        {
            if (Pivot != null)
                Pivot.SelectedIndex = selectedIndex;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (Pivot != null)
                Pivot.LoadedPivotItem -= MainPivot_LoadedPivotItem;
            base.OnNavigatedFrom(e);
        }
    }
}
