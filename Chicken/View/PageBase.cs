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

    public class PivotPageBase : PageBase, INavigationService
    {
        protected virtual Pivot Pivot { get; private set; }

        protected virtual PivotViewModelBase PivotViewModelBase { get; private set; }

        protected virtual void Init()
        {
            if (Pivot != null)
            {
                Pivot.LoadedPivotItem += new EventHandler<PivotItemEventArgs>(MainPivot_LoadedPivotItem);
                foreach (var item in PivotViewModelBase.PivotItems)
                {
                    item.ToastMessageHandler = ToastMessageHandler;
                }
            }
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
            {
                Pivot.SelectedIndex = selectedIndex;
            }
        }
    }
}
