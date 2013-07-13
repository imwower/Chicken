using System;
using Chicken.Controls;

namespace Chicken.ViewModel
{
    public class LazyDataItem : NotificationObject, ILazyDataItem
    {
        #region private
        private bool isPaused;
        private LazyDataLoadState currentState;
        #endregion

        #region ILazyDataItem
        public void GoToState(LazyDataLoadState state)
        {
            currentState = state;
            switch (state)
            {
                case LazyDataLoadState.Minimum:
                case LazyDataLoadState.Cached:
                case LazyDataLoadState.Reloading:
                case LazyDataLoadState.Loading:
                case LazyDataLoadState.Unloaded:
                case LazyDataLoadState.Loaded:
                    break;
            }
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Unpause()
        {
            isPaused = false;
        }

        public bool IsPaused
        {
            get
            {
                return isPaused;
            }
        }

        public LazyDataLoadState CurrentState
        {
            get
            {
                return currentState;
            }
        }

        public event EventHandler<LazyDataItemStateChangedEventArgs> CurrentStateChanged;

        public event EventHandler<LazyDataItemPausedStateChangedEventArgs> PauseStateChanged;
        #endregion
    }
}
