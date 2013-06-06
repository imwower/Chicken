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
        protected LoadEventHandler ScrollToTopHandler;
        protected LoadEventHandler ScrollToBottomHandler;
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

        public ICommand TopCommand
        {
            get
            {
                return new DelegateCommand(ScrollToTop);
            }
        }

        public ICommand BottomCommand
        {
            get
            {
                return new DelegateCommand(ScrollToBottom);
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

        public virtual void ScrollToTop()
        {
            if (ScrollToTopHandler == null)
            {
                return;
            }
            ScrollToTopHandler();
        }

        public virtual void ScrollToBottom()
        {
            if (ScrollToBottomHandler == null)
            {
                return;
            }
            ScrollToBottomHandler();
        }
        #endregion
    }
}
