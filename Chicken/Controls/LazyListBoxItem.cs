using System;
using System.Windows.Controls;

namespace Chicken.Controls
{
    public enum LazyDataLoadState
    {
        /// <summary>
        /// No additional data is loaded
        /// </summary>
        Unloaded,

        /// <summary>
        /// Data that is quick to load and doesn't use much memory is loaded
        /// </summary>
        /// <remarks>
        /// The item is in the virtual list but is not (yet) visible
        /// </remarks>
        Minimum,

        /// <summary>
        /// All fast data has been loaded, and slow items are loading
        /// </summary>
        /// <remarks>
        /// The item is in the list and visible, and all content is loading
        /// </remarks>
        Loading,

        /// <summary>
        /// All data has been loaded.
        /// </summary>
        /// <remarks>
        /// The item is in the list and visible, and all content has loaded
        /// </remarks>
        Loaded,

        /// <summary>
        /// Large data items have been released, only small ones (regardless of speed) are loaded
        /// </summary>
        /// <remarks>
        /// The item is in the virtual list but is no longer visible
        /// </remarks>
        Cached,

        /// <summary>
        /// All items except the large, slow ones are loaded (fast ones are loaded; small ones were cached)
        /// </summary>
        /// <remarks>
        /// The item is in the list and has become visible again after being invisible
        /// </remarks>
        Reloading,
    }

    /// <summary>
    /// Container used to hold items in a LazyListBox
    /// </summary>
    public class LazyListBoxItem : ListBoxItem
    {
        /// <summary>
        /// Used to check validity of the cached visibility state
        /// </summary>
        int visibleSnapshotVersion = -1;

        /// <summary>
        /// The LazyListBox that owns this item
        /// </summary>
        internal LazyListBox Owner { get; set; }

        /// <summary>
        /// Cached value of whether the item is visible or not
        /// </summary>
        bool cachedIsVisible = false;

        /// <summary>
        /// Whether or not the item is visible
        /// </summary>
        /// <remarks>
        /// An item that isn't virtualized is by definition not visible. Items that are virtualized may
        /// still be invisible if they are scrolled off the screen
        /// </remarks>
        public bool IsVisible
        {
            get
            {
                // Can't be visible if we have no owner
                if (Owner == null)
                    return false;

                // If the cached value is not out of date, return it
                if (visibleSnapshotVersion == Owner.VisibleSnapshotVersion)
                    return cachedIsVisible;

                // Otherwise, check if we exist in the visible items list
                if (Owner.GetVisibleItemsAsMutableList().Contains(this))
                    cachedIsVisible = true;
                else
                    cachedIsVisible = false;

                // Remember the current version for next time
                visibleSnapshotVersion = Owner.VisibleSnapshotVersion;
                return cachedIsVisible;
            }
        }

        /// <summary>
        /// Forces the item to be visible or invisible
        /// </summary>
        /// <param name="isVisible">True if the item is visible, false otherwise</param>
        /// <param name="currentSnapshotVersion">Version information used for cache purposes</param>
        internal void SetIsVisible(bool isVisible, int currentSnapshotVersion)
        {
            // We have special behaviour if the item is an ILazyDataItem
            ILazyDataItem lazyItem = DataContext as ILazyDataItem;

            cachedIsVisible = isVisible;
            visibleSnapshotVersion = currentSnapshotVersion;

            if (isVisible)
            {
                // If it's a special item, tell it to change states based on its current state
                if (lazyItem != null)
                {
                    LazyDataLoadState currentState = lazyItem.CurrentState;

                    // It was cached, so tell it to re-load
                    if (currentState == LazyDataLoadState.Cached)
                        lazyItem.GoToState(LazyDataLoadState.Reloading);

                    // It was loading and we asked it to pause; unpause it
                    else if (currentState == LazyDataLoadState.Loading || currentState == LazyDataLoadState.Reloading)
                    {
                        if (lazyItem.IsPaused)
                            lazyItem.Unpause();
                    }

                    // Otherwise, tell it to load unless it is already loading
                    else if (currentState != LazyDataLoadState.Loaded)
                        lazyItem.GoToState(LazyDataLoadState.Loading);
                }

                // Set the loaded item template, if it exists
                if (Owner.LoadedItemTemplate != null)
                    ContentTemplate = Owner.LoadedItemTemplate;
            }
            else
            {
                bool alreadyResetTemplate = false;

                // If it's a special item, tell it to change state
                if (lazyItem != null)
                {
                    // if the slow items are already loaded, switch to the cached template (vs the default template)
                    if (lazyItem.CurrentState == LazyDataLoadState.Loaded)
                    {
                        lazyItem.GoToState(LazyDataLoadState.Cached);
                        if (Owner.CachedItemTemplate != null)
                        {
                            ContentTemplate = Owner.CachedItemTemplate;
                            alreadyResetTemplate = true;
                        }
                    }
                }

                // If we didn't set it to the CachedItemTemplate, then reset to the ItemTemplate
                if (alreadyResetTemplate != true && Owner.Template != null)
                    ContentTemplate = Owner.ItemTemplate;
            }
        }

        /// <summary>
        /// Tell the item to pause any work it is doing, typically because the list is scrolling
        /// </summary>
        internal void Pause()
        {
            // Only makes sense for special items
            if (DataContext is ILazyDataItem)
                (DataContext as ILazyDataItem).Pause();
        }

#if DEBUG_INSTANCE_TRACKING
    // define DEBUG_INSTANCE_TRACKING to see how items are created and destroyed

    /// <summary>
    /// The next sequential ID for instance tracking
    /// </summary>
    static int nexId = 0;

    /// <summary>
    /// Property used for instance tracking
    /// </summary>
    internal int Id { get; private set; }

    ~LazyListBoxItem()
    {
      Debug.WriteLine("** Destroying LazyListBoxItem #" + Id);
    }
#endif

        /// <summary>
        /// Create a new LazyListBoxItem
        /// </summary>
        public LazyListBoxItem()
        {
            DefaultStyleKey = typeof(LazyListBoxItem);

#if DEBUG_INSTANCE_TRACKING
      // define DEBUG_INSTANCE_TRACKING to see how items are created and destroyed
      Id = nexId++;
      Debug.WriteLine("** Creating LazyListBoxItem #" + Id);
#endif
        }
    }

    /// <summary>
    /// Interface for a lazy data item
    /// </summary>
    public interface ILazyDataItem
    {
        /// <summary>
        /// Asks the item to move to a specific state.
        /// </summary>
        /// <remarks>
        /// The state machine is well-defined and can be enforced by using the LazyDataItemStateManagerHelper class
        /// </remarks>
        /// <param name="state">The state to go to</param>
        void GoToState(LazyDataLoadState state);

        /// <summary>
        /// Requests that the application pause any current work
        /// </summary>
        /// <remarks>
        /// Not all items will be able to support pausing. It is most critical to pause operations on the UI thread
        /// </remarks>
        void Pause();

        /// <summary>
        /// Unpauses a previously-paused item
        /// </summary>
        void Unpause();

        /// <summary>
        /// Returns whether or not the item is currently paused
        /// </summary>
        bool IsPaused { get; }

        /// <summary>
        /// Returns the current state of the item
        /// </summary>
        LazyDataLoadState CurrentState { get; }

        /// <summary>
        /// Raised when the state of the item changes
        /// </summary>
        /// <remarks>
        /// Avoid hooking this event if possible, and be sure to un-register from it when no longer needed
        /// </remarks>
        event EventHandler<LazyDataItemStateChangedEventArgs> CurrentStateChanged;

        /// <summary>
        /// Raised when the pause state of the item changes
        /// </summary>
        /// <remarks>
        /// Avoid hooking this event if possible, and be sure to un-register from it when no longer needed
        /// </remarks>
        event EventHandler<LazyDataItemPausedStateChangedEventArgs> PauseStateChanged;
    }
}
