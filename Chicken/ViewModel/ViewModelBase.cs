using System.Windows.Input;

namespace Chicken.ViewModel
{
    public class ViewModelBase : NotificationObject
    {
        #region event handler
        protected delegate void LoadEventHandler();
        protected LoadEventHandler LoadHandler;
        protected LoadEventHandler RefreshHandler;
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
        public virtual void Load()
        {
            if (LoadHandler != null)
            {
                LoadHandler();
            }
        }

        public virtual void Refresh()
        {
            if (RefreshHandler != null)
            {
                RefreshHandler();
            }
        }

        public virtual void ScrollToTop()
        {
            if (ScrollToTopHandler != null)
            {
                ScrollToTopHandler();
            }
        }

        public virtual void ScrollToBottom()
        {
            if (ScrollToBottomHandler != null)
            {
                ScrollToBottomHandler();
            }
        }
        #endregion
    }
}
