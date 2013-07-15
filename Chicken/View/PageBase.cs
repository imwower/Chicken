using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
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

        #region private method
        private static void prompt_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (promptComplete != null)
                promptComplete();
            promptComplete = null;
            prompt.Completed -= prompt_Completed;
        }

        /// <summary>
        /// for home page and api setting page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Page_OnBackKeyPress(object sender, CancelEventArgs e)
        {
            int count = NavigationService.BackStack.Count();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                    NavigationService.RemoveBackEntry();
            }

            if (MessageBox.Show(LanguageHelper.GetString("Msg_ConfirmToExit"), "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
        #endregion
    }

    public class PivotPageBase : PageBase, INavigationService
    {
        #region property
        protected Pivot Pivot { get; set; }
        protected PivotViewModelBase PivotViewModelBase { get; set; }
        #endregion

        protected virtual void Init()
        {
            //if (Pivot != null)
            Pivot.LoadedPivotItem += MainPivot_LoadedPivotItem;
            this.DataContext = PivotViewModelBase;
        }

        protected virtual void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            //if (PivotViewModelBase != null)
            {
                PivotViewModelBase.SelectedIndex = (sender as Pivot).SelectedIndex;
                PivotViewModelBase.MainPivot_LoadedPivotItem();
            }
        }

        public virtual void ChangeSelectedIndex(int selectedIndex)
        {
            //if (Pivot != null)
            Pivot.SelectedIndex = selectedIndex;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //if (Pivot != null)
            Pivot.LoadedPivotItem -= MainPivot_LoadedPivotItem;
            base.OnNavigatedFrom(e);
        }
    }
}
