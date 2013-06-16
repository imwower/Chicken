using System;
using Chicken.Controls;
using Chicken.Model;
using Chicken.Service.Interface;
using Chicken.ViewModel;
using Microsoft.Phone.Controls;

namespace Chicken.View
{
    public class PageBase : PhoneApplicationPage
    {
        #region message handler
        protected virtual void ToastMessageHandler(ToastMessage message)
        {
            var toast = new ToastPrompt
            {
                Message = message.Message,
            };
            if (message.Complete != null)
            {
                toast.Completed +=
                   (o, e) =>
                   {
                       message.Complete();
                   };
            }
            toast.Show();
        }
        #endregion
    }

    public abstract class PivotPageBase : PageBase, INavigationService
    {
        protected abstract Pivot Pivot { get; }

        protected abstract PivotViewModelBase PivotViewModelBase { get; }

        protected virtual void Init()
        {
            Pivot.LoadedPivotItem += new EventHandler<PivotItemEventArgs>(MainPivot_LoadedPivotItem);
            foreach (var item in PivotViewModelBase.PivotItems)
            {
                item.ToastMessageHandler = ToastMessageHandler;
            }
        }

        protected virtual void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            int selectedIndex = (sender as Pivot).SelectedIndex;
            PivotViewModelBase.MainPivot_LoadedPivotItem(selectedIndex);
        }

        public virtual void ChangeSelectedIndex(int selectedIndex)
        {
            Pivot.SelectedIndex = selectedIndex;
        }
    }
}
