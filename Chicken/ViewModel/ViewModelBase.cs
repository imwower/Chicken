using System.Windows.Input;

namespace Chicken.ViewModel
{
    public class ViewModelBase : NotificationObject
    {
        #region event handler
        protected delegate void LoadEventHandler();
        protected delegate void ClickEventHandler(object parameter);
        protected LoadEventHandler RefreshHandler;
        protected LoadEventHandler LoadHandler;
        #endregion

        public ViewModelBase()
        {
        }

        #region binding Command
        public ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(Refresh);
            }
        }

        public ICommand LoadCommand
        {
            get
            {
                return new DelegateCommand(Load);
            }
        }
        #endregion

        #region public method
        public virtual void Refresh()
        {
            if (RefreshHandler == null)
            {
                return;
            }
            RefreshHandler();
        }

        public virtual void Load()
        {
            if (LoadHandler == null)
            {
                return;
            }
            LoadHandler();
        }
        #endregion
    }
}
