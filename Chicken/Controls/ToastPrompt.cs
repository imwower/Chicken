﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

// This is from the Coding4Fun Toolkit.
//see http://coding4fun.codeplex.com/ for details.

using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chicken.Controls
{
    public class ToastPrompt : PopUp<string, PopUpResult>, IDisposable, IImageSourceFull
    {
        protected Image ToastImage;
        private const string ToastImageName = "ToastImage";
        private Timer _timer;

        private TranslateTransform _translate;

        public ToastPrompt()
        {
            DefaultStyleKey = typeof(ToastPrompt);

            IsAppBarVisible = true;
            IsBackKeyOverride = true;
            IsCalculateFrameVerticalOffset = true;

            IsOverlayApplied = false;

            AnimationType = DialogService.AnimationTypes.SlideHorizontal;

            ManipulationStarted += ToastPromptManipulationStarted;
            ManipulationDelta += ToastPromptManipulationDelta;
            ManipulationCompleted += ToastPromptManipulationCompleted;

            Opened += ToastPromptOpened;
        }

        void ToastPromptManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            PauseTimer();
        }

        void ToastPromptManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            _translate.X += e.DeltaManipulation.Translation.X;

            if (_translate.X < 0)
                _translate.X = 0;
        }

        void ToastPromptManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (e.TotalManipulation.Translation.X > 200 || e.FinalVelocities.LinearVelocity.X > 1000)
            {
                OnCompleted(new PopUpEventArgs<string, PopUpResult> { PopUpResult = PopUpResult.UserDismissed });
            }
            else if (e.TotalManipulation.Translation.X < 20)
            {
                OnCompleted(new PopUpEventArgs<string, PopUpResult> { PopUpResult = PopUpResult.Ok });
            }
            else
            {
                _translate.X = 0;
                StartTimer();
            }
        }

        private void StartTimer()
        {
            if (_timer == null)
                _timer = new Timer(TimerTick);

            _timer.Change(TimeSpan.FromMilliseconds(MillisecondsUntilHidden), TimeSpan.FromMilliseconds(-1));
        }

        private void PauseTimer()
        {
            if (_timer != null)
                _timer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetRenderTransform();

            ToastImage = GetTemplateChild(ToastImageName) as Image;

            if (ToastImage != null && ImageSource != null)
            {
                ToastImage.Source = ImageSource;
                SetImageVisibility(ImageSource);
            }

            SetTextOrientation(TextWrapping);
        }

        private void SetRenderTransform()
        {
            _translate = new TranslateTransform();
            RenderTransform = _translate;
        }

        public override void Show()
        {
            if (!IsTimerEnabled)
                return;

            base.Show();

            SetRenderTransform();
            PreventScrollBinding.SetIsEnabled(this, true);
        }

        void ToastPromptOpened(object sender, EventArgs e)
        {
            StartTimer();
        }

        void TimerTick(object state)
        {
            Dispatcher.BeginInvoke(() => OnCompleted(new PopUpEventArgs<string, PopUpResult> { PopUpResult = PopUpResult.NoResponse }));
        }

        public override void OnCompleted(PopUpEventArgs<string, PopUpResult> result)
        {
            PreventScrollBinding.SetIsEnabled(this, false);

            PauseTimer();
            Dispose();

            base.OnCompleted(result);
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        public int MillisecondsUntilHidden
        {
            get { return (int)GetValue(MillisecondsUntilHiddenProperty); }
            set { SetValue(MillisecondsUntilHiddenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MillisecondsUntilHidden.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MillisecondsUntilHiddenProperty =
            DependencyProperty.Register("MillisecondsUntilHidden", typeof(int), typeof(ToastPrompt), new PropertyMetadata(4000));

        public bool IsTimerEnabled
        {
            get { return (bool)GetValue(IsTimerEnabledProperty); }
            set { SetValue(IsTimerEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsTimerEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTimerEnabledProperty =
            DependencyProperty.Register("IsTimerEnabled", typeof(bool), typeof(ToastPrompt), new PropertyMetadata(true));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ToastPrompt), new PropertyMetadata(""));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(ToastPrompt), new PropertyMetadata(""));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ToastPrompt),
            new PropertyMetadata(OnImageSource));

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Stretch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ToastPrompt),
            new PropertyMetadata(Stretch.None));

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(ToastPrompt), new PropertyMetadata(double.NaN));

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight", typeof(double), typeof(ToastPrompt), new PropertyMetadata(double.NaN));

        public Orientation TextOrientation
        {
            get { return (Orientation)GetValue(TextOrientationProperty); }
            set { SetValue(TextOrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextOrientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextOrientationProperty =
            DependencyProperty.Register("TextOrientation", typeof(Orientation), typeof(ToastPrompt), new PropertyMetadata(Orientation.Horizontal));

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextWrapping.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(ToastPrompt), new PropertyMetadata(TextWrapping.NoWrap, OnTextWrapping));

        private static void OnTextWrapping(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as ToastPrompt;

            if (sender == null || sender.ToastImage == null)
                return;

            sender.SetTextOrientation((TextWrapping)e.NewValue);
        }

        private static void OnImageSource(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as ToastPrompt;

            if (sender == null || sender.ToastImage == null)
                return;

            sender.SetImageVisibility(e.NewValue as ImageSource);
        }

        private void SetImageVisibility(ImageSource source)
        {
            ToastImage.Visibility = (source == null) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void SetTextOrientation(TextWrapping value)
        {
            if (value == TextWrapping.Wrap)
            {
                TextOrientation = Orientation.Vertical;
            }
        }
    }
}
