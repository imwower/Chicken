using System;
using Chicken.Controls;

namespace Chicken.ViewModel
{
    public class VisibleObject : NotificationObject, ILazyDataItem
    {
        #region private
        private bool isVisible;
        private bool isPaused;
        private LazyDataLoadState currentState;
        #endregion

        /// <summary>
        /// should manually change value,
        /// to raise property changed event
        /// </summary>
        public virtual bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                RaisePropertyChanged("IsVisible");
            }
        }

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
                    IsVisible = true;
                    break;
                case LazyDataLoadState.Unloaded:
                    IsVisible = false;
                    break;
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
