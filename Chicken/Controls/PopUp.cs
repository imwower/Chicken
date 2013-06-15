// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

// This is from the Coding4Fun Toolkit.
//see http://coding4fun.codeplex.com/ for details.

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Chicken.Controls
{
    public interface IImageSource
    {
        Stretch Stretch { get; set; }
        ImageSource ImageSource { get; set; }
    }

    public interface IImageSourceFull : IImageSource
    {
        double ImageWidth { get; set; }
        double ImageHeight { get; set; }
    }

    public abstract class PopUp<T, TPopUpResult> : Control
    {
        internal DialogService PopUpService;
        private PhoneApplicationPage _startingPage;
        private bool _alreadyFired;

        public bool IsOpen { get { return PopUpService != null && PopUpService.IsOpen; } }
        public bool IsAppBarVisible { get; set; }

        // adjust for SIP
        private bool _isCalculateFrameVerticalOffset;

        protected bool IsCalculateFrameVerticalOffset
        {
            get { return _isCalculateFrameVerticalOffset; }
            set
            {
                _isCalculateFrameVerticalOffset = value;

                if (_isCalculateFrameVerticalOffset)
                {
                    var bind = new System.Windows.Data.Binding("Y");
                    var frame = (Application.Current.RootVisual as Frame);

                    if (frame != null)
                    {
                        var transGroup = frame.RenderTransform as TransformGroup;

                        if (transGroup != null)
                        {
                            bind.Source = transGroup.Children.FirstOrDefault(t => t is TranslateTransform);
                            SetBinding(FrameTransformProperty, bind);
                        }
                    }
                }
            }
        }

        public bool IsOverlayApplied
        {
            get { return _isOverlayApplied; }
            set { _isOverlayApplied = value; }
        }
        private bool _isOverlayApplied = true;

        internal bool IsSetAppBarVisibiilty { get; set; }
        internal TimeSpan MainBodyDelay { get; set; }

        protected internal bool IsBackKeyOverride { get; set; }
        protected DialogService.AnimationTypes AnimationType { get; set; }

        public event EventHandler<PopUpEventArgs<T, TPopUpResult>> Completed;
        public event EventHandler Opened;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (PopUpService != null)
            {
                PopUpService.BackgroundBrush = Overlay;

                PopUpService.ApplyOverlayBackground();
                PopUpService.SetAlignmentsOnOverlay(HorizontalAlignment, VerticalAlignment);
            }
        }

        public virtual void OnCompleted(PopUpEventArgs<T, TPopUpResult> result)
        {
            _alreadyFired = true;

            if (Completed != null)
                Completed(this, result);

            if (PopUpService != null)
                PopUpService.Hide();

            if (PopUpService != null && PopUpService.BackButtonPressed)
                ResetWorldAndDestroyPopUp();
        }

        static void OnFrameTransformPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var sender = source as PopUp<T, TPopUpResult>;

            if (sender == null || sender.PopUpService == null)
                return;

            if (!sender.IsCalculateFrameVerticalOffset)
                return;

            sender.PopUpService.ControlVerticalOffset = -sender.FrameTransform;
            sender.PopUpService.CalculateVerticalOffset();
        }

        public virtual void Show()
        {
            if (IsOpen)
                return;

            if (PopUpService == null)
            {
                PopUpService = new DialogService
                {
                    AnimationType = AnimationType,
                    Child = this,
                    IsBackKeyOverride = IsBackKeyOverride,
                    IsOverlayApplied = IsOverlayApplied,
                    MainBodyDelay = MainBodyDelay,
                };
            }


            // this will happen if the user comes in OnNavigate or 
            // something where the DOM hasn't been created yet.
            if (PopUpService.Page == null)
            {
                Dispatcher.BeginInvoke(Show);

                return;
            }

            if (IsCalculateFrameVerticalOffset)
            {
                PopUpService.ControlVerticalOffset = -FrameTransform;
            }

            PopUpService.Closed += PopUpClosed;
            PopUpService.Opened += PopUpOpened;


            if (!IsAppBarVisible && PopUpService.Page.ApplicationBar != null && PopUpService.Page.ApplicationBar.IsVisible)
            {
                PopUpService.Page.ApplicationBar.IsVisible = false;

                IsSetAppBarVisibiilty = true;
            }

            _startingPage = PopUpService.Page;

            PopUpService.Show();
        }

        void PopUpOpened(object sender, EventArgs e)
        {
            if (Opened != null)
                Opened(sender, e);
        }

        protected virtual TPopUpResult GetOnClosedValue()
        {
            return default(TPopUpResult);
        }

        public void Hide()
        {
            PopUpClosed(this, null);
        }

        void PopUpClosed(object sender, EventArgs e)
        {
            if (!_alreadyFired)
            {
                OnCompleted(new PopUpEventArgs<T, TPopUpResult> { PopUpResult = GetOnClosedValue() });
                return;
            }

            ResetWorldAndDestroyPopUp();
        }

        private void ResetWorldAndDestroyPopUp()
        {
            if (PopUpService != null)
            {
                if (!IsAppBarVisible && IsSetAppBarVisibiilty)
                {
                    _startingPage.ApplicationBar.IsVisible = IsSetAppBarVisibiilty;
                }

                _startingPage = null;

                PopUpService.Child = null;
                PopUpService = null;
            }
        }

        double FrameTransform
        {
            get { return (double)GetValue(FrameTransformProperty); }
            set { SetValue(FrameTransformProperty, value); }
        }

        static readonly DependencyProperty FrameTransformProperty = DependencyProperty.Register(
              "FrameTransform",
              typeof(double),
              typeof(PopUp<T, TPopUpResult>),
              new PropertyMetadata(0.0, OnFrameTransformPropertyChanged));

        public Brush Overlay
        {
            get { return (Brush)GetValue(OverlayProperty); }
            set { SetValue(OverlayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Overlay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverlayProperty =
            DependencyProperty.Register("Overlay", typeof(Brush), typeof(PopUp<T, TPopUpResult>), new PropertyMetadata(new SolidColorBrush()));
    }

    public enum PopUpResult
    {
        Cancelled,
        NoResponse,
        UserDismissed,
        Ok
    }

    public class PopUpEventArgs<T, TPopUpResult> : EventArgs
    {
        public TPopUpResult PopUpResult { get; set; }
        public Exception Error { get; set; }
        public T Result { get; set; }
    }

    public class DialogService
    {
        public enum AnimationTypes
        {
            Slide,
            SlideHorizontal,
            Swivel,
            SwivelHorizontal,
            Fade
        }

        private const string SlideUpStoryboard = @"
        <Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(TranslateTransform.Y)"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""150""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""0"" To=""1"" Duration=""0:0:0.350"">
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        private const string SlideHorizontalInStoryboard = @"
        <Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(TranslateTransform.X)"" >
                    <EasingDoubleKeyFrame KeyTime=""0"" Value=""-150""/>
                    <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
                        <EasingDoubleKeyFrame.EasingFunction>
                            <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                        </EasingDoubleKeyFrame.EasingFunction>
                    </EasingDoubleKeyFrame>
                </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""0"" To=""1"" Duration=""0:0:0.350"" >
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        private const string SlideHorizontalOutStoryboard = @"
        <Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(TranslateTransform.X)"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""150"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""1"" To=""0"" Duration=""0:0:0.25"">
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        private const string SlideDownStoryboard = @"
        <Storyboard  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.RenderTransform).(TranslateTransform.Y)"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""150"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimation Storyboard.TargetProperty=""(UIElement.Opacity)"" From=""1"" To=""0"" Duration=""0:0:0.25"">
                <DoubleAnimation.EasingFunction>
                    <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                </DoubleAnimation.EasingFunction>
            </DoubleAnimation>
        </Storyboard>";

        private const string SwivelInStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimation 
				To="".5""
                Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.CenterOfRotationY)"" />
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationX)"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""-30""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.35"" Value=""0"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseOut"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Opacity)"">
                <DiscreteDoubleKeyFrame KeyTime=""0"" Value=""1"" />
            </DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        private const string SwivelOutStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimation BeginTime=""0:0:0"" Duration=""0"" 
                                Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.CenterOfRotationY)"" 
                                To="".5""/>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Projection).(PlaneProjection.RotationX)"">
                <EasingDoubleKeyFrame KeyTime=""0"" Value=""0""/>
                <EasingDoubleKeyFrame KeyTime=""0:0:0.25"" Value=""45"">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <ExponentialEase EasingMode=""EaseIn"" Exponent=""6""/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=""(UIElement.Opacity)"">
                <DiscreteDoubleKeyFrame KeyTime=""0"" Value=""1"" />
                <DiscreteDoubleKeyFrame KeyTime=""0:0:0.267"" Value=""0"" />
            </DoubleAnimationUsingKeyFrames>
        </Storyboard>";

        private const string FadeInStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimation 
				Duration=""0:0:0.2"" 
				Storyboard.TargetProperty=""(UIElement.Opacity)"" 
                To=""1""/>
        </Storyboard>";

        private const string FadeOutStoryboard =
        @"<Storyboard xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
            <DoubleAnimation 
				Duration=""0:0:0.2""
				Storyboard.TargetProperty=""(UIElement.Opacity)"" 
                To=""0""/>
        </Storyboard>";

        private Panel _popupContainer;
        private Frame _rootVisual;
        private PhoneApplicationPage _page;
        private Grid _childPanel;
        private Grid _overlay;

        public bool IsOverlayApplied
        {
            get { return _isOverlayApplied; }
            set { _isOverlayApplied = value; }
        }
        private bool _isOverlayApplied = true;

        public FrameworkElement Child { get; set; }
        public AnimationTypes AnimationType { get; set; }
        public TimeSpan MainBodyDelay { get; set; }

        public double VerticalOffset { get; set; }
        internal double ControlVerticalOffset { get; set; }
        public bool BackButtonPressed { get; set; }

        public Brush BackgroundBrush { get; set; }

        internal bool IsOpen { get; set; }
        protected internal bool IsBackKeyOverride { get; set; }

        public event EventHandler Closed;
        public event EventHandler Opened;

        // set this to prevent the dialog service from closing on back click
        public bool HasPopup { get; set; }

        internal PhoneApplicationPage Page
        {
            get { return _page ?? (_page = RootVisual.Content as PhoneApplicationPage); }
        }

        internal Frame RootVisual
        {
            get { return _rootVisual ?? (_rootVisual = Application.Current.RootVisual as Frame); }
        }

        internal Panel PopupContainer
        {
            get
            {
                if (_popupContainer == null)
                {
                    //var popups = RootVisual.GetLogicalChildrenByType<Popup>(false).Where(x => x.IsOpen);

                    //if (popups.Any())
                    //{
                    //    for (var i = 0; i < popups.Count(); i++)
                    //    {
                    //        var child = popups.ElementAt(i).Child as Panel;

                    //        if (child == null)
                    //            continue;

                    //        _popupContainer = child;
                    //        break;
                    //    }
                    //}
                    //else
                    {
                        var presenters = RootVisual.GetLogicalChildrenByType<ContentPresenter>(false);

                        for (var i = 0; i < presenters.Count(); i++)
                        {
                            var panels = presenters.ElementAt(i).GetLogicalChildrenByType<Panel>(false);

                            if (!panels.Any())
                                continue;

                            _popupContainer = panels.First();
                            break;
                        }
                    }

                }

                return _popupContainer;
            }
        }

        public DialogService()
        {
            AnimationType = AnimationTypes.Slide;
            BackButtonPressed = false;
        }

        bool _deferredShowToLoaded;
        private void InitializePopup()
        {
            // Add overlay which is the size of RootVisual
            _childPanel = CreateGrid();

            if (IsOverlayApplied)
            {
                _overlay = CreateGrid();
                PreventScrollBinding.SetIsEnabled(_overlay, true);
            }

            ApplyOverlayBackground();

            // Initialize popup to draw the context menu over all controls
            if (PopupContainer != null)
            {
                if (_overlay != null)
                    PopupContainer.Children.Add(_overlay);

                PopupContainer.Children.Add(_childPanel);
                _childPanel.Children.Add(Child);
            }
            else
            {
                _deferredShowToLoaded = true;
                RootVisual.Loaded += RootVisualDeferredShowLoaded;
            }
        }

        internal void ApplyOverlayBackground()
        {
            if (IsOverlayApplied && BackgroundBrush != null)
                _overlay.Background = BackgroundBrush;
        }

        private Grid CreateGrid()
        {
            var grid = new Grid { Name = Guid.NewGuid().ToString() };

            Grid.SetColumnSpan(grid, int.MaxValue);
            Grid.SetRowSpan(grid, int.MaxValue);

            grid.Opacity = 0;

            CalculateVerticalOffset(grid);

            return grid;
        }

        internal void CalculateVerticalOffset()
        {
            CalculateVerticalOffset(_childPanel);
        }

        internal void CalculateVerticalOffset(Panel panel)
        {
            if (panel == null)
                return;

            var sysTrayVerticalOffset = 0;

            if (SystemTray.IsVisible && SystemTray.Opacity < 1 && SystemTray.Opacity > 0)
            {
                sysTrayVerticalOffset += 32;
            }

            panel.Margin = new Thickness(0, VerticalOffset + sysTrayVerticalOffset + ControlVerticalOffset, 0, 0);
        }

        void RootVisualDeferredShowLoaded(object sender, RoutedEventArgs e)
        {
            RootVisual.Loaded -= RootVisualDeferredShowLoaded;
            _deferredShowToLoaded = false;

            Show();
        }

        protected internal void SetAlignmentsOnOverlay(HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            if (_childPanel != null)
            {
                _childPanel.HorizontalAlignment = horizontalAlignment;
                _childPanel.VerticalAlignment = verticalAlignment;
            }
        }

        private static readonly object Lockobj = new object();
        /// <summary>
        /// Shows the context menu.
        /// </summary>
        public void Show()
        {
            lock (Lockobj)
            {
                IsOpen = true;

                InitializePopup();

                if (_deferredShowToLoaded)
                    return;

                if (!IsBackKeyOverride)
                    Page.BackKeyPress += OnBackKeyPress;

                Page.NavigationService.Navigated += OnNavigated;

                RunShowStoryboard(_overlay, AnimationTypes.Fade);
                RunShowStoryboard(_childPanel, AnimationType, MainBodyDelay);

                if (Opened != null)
                    Opened.Invoke(this, null);

            }
        }

        private void RunShowStoryboard(UIElement grid, AnimationTypes animation)
        {
            RunShowStoryboard(grid, animation, TimeSpan.MinValue);
        }

        private void RunShowStoryboard(UIElement grid, AnimationTypes animation, TimeSpan delay)
        {
            if (grid == null)
                return;

            Storyboard storyboard;
            switch (animation)
            {
                case AnimationTypes.SlideHorizontal:
                    storyboard = XamlReader.Load(SlideHorizontalInStoryboard) as Storyboard;
                    grid.RenderTransform = new TranslateTransform();
                    break;

                case AnimationTypes.Slide:
                    storyboard = XamlReader.Load(SlideUpStoryboard) as Storyboard;
                    grid.RenderTransform = new TranslateTransform();
                    break;
                case AnimationTypes.Fade:
                    storyboard = XamlReader.Load(FadeInStoryboard) as Storyboard;
                    break;
                case AnimationTypes.Swivel:
                case AnimationTypes.SwivelHorizontal:
                default:
                    storyboard = XamlReader.Load(SwivelInStoryboard) as Storyboard;
                    grid.Projection = new PlaneProjection();
                    break;
            }

            if (storyboard != null)
            {
                foreach (var storyboardAnimation in storyboard.Children)
                {
                    if (!(storyboardAnimation is DoubleAnimationUsingKeyFrames))
                        continue;

                    var doubleKey = storyboardAnimation as DoubleAnimationUsingKeyFrames;

                    foreach (var frame in doubleKey.KeyFrames)
                    {
                        frame.KeyTime = KeyTime.FromTimeSpan(frame.KeyTime.TimeSpan.Add(delay));
                    }
                }

                Page.Dispatcher.BeginInvoke(() =>
                {
                    foreach (var t in storyboard.Children)
                        Storyboard.SetTarget(t, grid);

                    storyboard.Begin();
                });
            }
        }

        void OnNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.IsNavigationInitiator) //current app initialized navigation?
                Hide();
        }

        public void Hide()
        {
            if (!IsOpen)
                return;

            if (Page != null)
            {
                Page.BackKeyPress -= OnBackKeyPress;
                Page.NavigationService.Navigated -= OnNavigated;

                _page = null;
            }

            RunHideStoryboard(_overlay, AnimationTypes.Fade);
            RunHideStoryboard(_childPanel, AnimationType);
        }

        void RunHideStoryboard(Grid grid, AnimationTypes animation)
        {
            if (grid == null)
                return;

            Storyboard storyboard;

            switch (animation)
            {
                case AnimationTypes.SlideHorizontal:
                    storyboard = XamlReader.Load(SlideHorizontalOutStoryboard) as Storyboard;
                    break;
                case AnimationTypes.Slide:
                    storyboard = XamlReader.Load(SlideDownStoryboard) as Storyboard;
                    break;
                case AnimationTypes.Fade:
                    storyboard = XamlReader.Load(FadeOutStoryboard) as Storyboard;
                    break;
                case AnimationTypes.Swivel:
                case AnimationTypes.SwivelHorizontal:
                default:
                    storyboard = XamlReader.Load(SwivelOutStoryboard) as Storyboard;
                    break;
            }

            try
            {
                if (storyboard != null)
                {
                    storyboard.Completed += HideStoryboardCompleted;

                    foreach (var t in storyboard.Children)
                        Storyboard.SetTarget(t, grid);

                    storyboard.Begin();
                }
            }
            catch (Exception)
            {
                // chances are user nav'ed away
                // attempting to be extremely robust here
                // if this fails, go straight to complete
                // and attempt to remove it from the visual tree
                HideStoryboardCompleted(null, null);
            }
        }

        void HideStoryboardCompleted(object sender, EventArgs e)
        {
            IsOpen = false;

            try
            {
                if (PopupContainer != null && PopupContainer.Children != null)
                {
                    if (_overlay != null)
                        PopupContainer.Children.Remove(_overlay);

                    PopupContainer.Children.Remove(_childPanel);
                }

                _childPanel.Children.Clear();
            }
            catch
            {
                // chances are user nav'ed away
                // attempting to be extremely robust here
                // if this fails, go straight to complete
                // and attempt to remove it from the visual tree
            }

            try
            {
                if (Closed != null)
                    Closed(this, null);

            }
            catch
            {
                // chances are user nav'ed away
                // attempting to be extremely robust here
                // if this fails, go straight to complete
                // and attempt to remove it from the visual tree
            }
        }

        public void OnBackKeyPress(object sender, CancelEventArgs e)
        {
            if (HasPopup)
            {
                e.Cancel = true;
                return;
            }

            if (IsOpen)
            {
                e.Cancel = true;
                BackButtonPressed = true;
                Hide();
            }
        }
    }

    public class PreventScrollBinding
    {
        // Contains the Panorama/Pivot. This extension cannot handle blocking
        // multiple panning controls simultaneously.
        private static FrameworkElement _internalPanningControl;

        // Using a DependencyProperty as the backing store for IsScrollSuspended.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty IsScrollSuspendedProperty =
            DependencyProperty.RegisterAttached("IsScrollSuspended", typeof(bool), typeof(PreventScrollBinding), new PropertyMetadata(false));

        // Using a DependencyProperty as the backing store for LastTouchPoint.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty LastTouchPointProperty =
            DependencyProperty.RegisterAttached("LastTouchPoint", typeof(TouchPoint), typeof(PreventScrollBinding), new PropertyMetadata(null));

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabled);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabled, value);
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnabled =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(PreventScrollBinding), new PropertyMetadata(false, IsEnabledDependencyPropertyChangedCallback));

        private static void IsEnabledDependencyPropertyChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs ea)
        {
            // The element that should prevent the Panorama/Pivot from scrolling
            var blockingElement = dobj as FrameworkElement;

            if (blockingElement == null)
                return;

#if WP8
			blockingElement.UseOptimizedManipulationRouting = false;
#endif

            blockingElement.Unloaded += BlockingElementUnloaded;
            blockingElement.MouseLeftButtonDown += SuspendScroll;
            blockingElement.ManipulationStarted += SuspendScroll;
        }

        private static void BlockingElementUnloaded(object sender, RoutedEventArgs e)
        {
            var blockingElement = sender as FrameworkElement;

            if (blockingElement == null)
                return;

            blockingElement.Unloaded -= BlockingElementUnloaded;
            blockingElement.MouseLeftButtonDown -= SuspendScroll;
            blockingElement.ManipulationStarted -= SuspendScroll;
        }

        private static void SuspendScroll(object sender, RoutedEventArgs e)
        {
            var blockingElement = sender as FrameworkElement;

            // Determines the parent Panorama/Pivot control
            if (_internalPanningControl == null)
                _internalPanningControl = FindAncestor(blockingElement, p => p is Pivot || p is Panorama) as FrameworkElement;

            if (_internalPanningControl != null && (bool)_internalPanningControl.GetValue(IsScrollSuspendedProperty))
                return;
            // When the user touches the control...
            var originalSource = e.OriginalSource as DependencyObject;
            if (FindAncestor(originalSource, dobj => (dobj == blockingElement)) != blockingElement)
                return;

            // Mark the parent Panorama/Pivot for scroll suspension
            // and register for touch frame events
            if (_internalPanningControl != null)
                _internalPanningControl.SetValue(IsScrollSuspendedProperty, true);

            Touch.FrameReported += TouchFrameReported;

            if (blockingElement != null)
                blockingElement.IsHitTestVisible = true;

            if (_internalPanningControl != null)
                _internalPanningControl.IsHitTestVisible = false;
        }

        private static void TouchFrameReported(object sender, TouchFrameEventArgs e)
        {
            if (_internalPanningControl == null)
                return;

            // (When the parent Panorama/Pivot is suspended)
            // Wait for the first touch to end (touchaction up). When it is, restore standard
            // panning behavior, otherwise let the control behave normally (no code for this)
            var lastTouchPoint = _internalPanningControl.GetValue(LastTouchPointProperty) as TouchPoint;
            var isScrollSuspended = (bool)_internalPanningControl.GetValue(IsScrollSuspendedProperty);

            var touchPoint = e.GetTouchPoints(Application.Current.RootVisual);

            if (lastTouchPoint == null || lastTouchPoint != touchPoint.Last())
                lastTouchPoint = touchPoint.Last();

            if (isScrollSuspended)
            {
                // Touch is up, custom behavior is over reset to original values
                if (lastTouchPoint != null && lastTouchPoint.Action == TouchAction.Up)
                {
                    Touch.FrameReported -= TouchFrameReported;
                    _internalPanningControl.IsHitTestVisible = true;
                    _internalPanningControl.SetValue(IsScrollSuspendedProperty, false);
                }
            }
        }

        /// <summary>
        /// Traverses the Visual Tree upwards looking for the ancestor that satisfies the <paramref name="predicate"/>.
        /// </summary>
        /// <param name="dependencyObject">The element for which the ancestor is being looked for.</param>
        /// <param name="predicate">The predicate that evaluates if an element is the ancestor that is being looked for.</param>
        /// <returns>
        /// The ancestor element that matches the <paramref name="predicate"/> or <see langword="null"/>
        /// if the ancestor was not found.
        /// </returns>
        public static DependencyObject FindAncestor(DependencyObject dependencyObject, Func<DependencyObject, bool> predicate)
        {
            if (predicate(dependencyObject))
            {
                return dependencyObject;
            }

            DependencyObject parent = null;
            var frameworkElement = dependencyObject as FrameworkElement;

            if (frameworkElement != null)
            {
                parent = frameworkElement.Parent ?? VisualTreeHelper.GetParent(frameworkElement);
            }

            return parent != null ? FindAncestor(parent, predicate) : null;
        }
    }
}
