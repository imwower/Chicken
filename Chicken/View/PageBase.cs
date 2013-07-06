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
        protected virtual Pivot Pivot { get; private set; }
        protected virtual PivotViewModelBase PivotViewModelBase { get; private set; }

        protected virtual void Init()
        {
            if (Pivot != null)
                Pivot.LoadedPivotItem += MainPivot_LoadedPivotItem;
        }

        protected virtual void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            if (PivotViewModelBase != null)
            {
                int selectedIndex = (sender as Pivot).SelectedIndex;
                PivotViewModelBase.MainPivot_LoadedPivotItem(selectedIndex);
            }
        }

        public virtual void ChangeSelectedIndex(int selectedIndex)
        {
            if (Pivot != null)
                Pivot.SelectedIndex = selectedIndex;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (Pivot != null)
                Pivot.LoadedPivotItem -= MainPivot_LoadedPivotItem;
        }
    }
}
