using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Chicken.Service;
using NGif;

namespace Chicken.Controls
{
    public partial class ImageContainer : UserControl
    {
        #region private static properties
        private static BitmapImage defaultImage = new BitmapImage(new Uri("/Images/dark/cat.png", UriKind.Relative));
        private GifDecoder decoder;
        private DispatcherTimer timer;
        private int index;
        #endregion

        #region properties
        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register("ImageUrl", typeof(string), typeof(ImageContainer), null);

        public string ImageUrl
        {
            get
            {
                return (string)GetValue(ImageUrlProperty);
            }
            set
            {
                SetValue(ImageUrlProperty, value);
            }
        }

        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(ImageContainer), new PropertyMetadata(false, OnIsVisibleChanged));

        public bool IsVisible
        {
            get
            {
                return (bool)GetValue(IsVisibleProperty);
            }
            set
            {
                SetValue(IsVisibleProperty, value);
            }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(byte[]), typeof(ImageContainer), new PropertyMetadata(OnSourceChanged));

        public byte[] ImageSource
        {
            get
            {
                return (byte[])GetValue(ImageSourceProperty);
            }
            set
            {
                SetValue(ImageSourceProperty, value);
            }
        }

        public bool UseDefaultImage { get; set; }

        public Stretch Stretch
        {
            get
            {
                return this.PngImage.Stretch;
            }
            set
            {
                this.PngImage.Stretch = value;
            }
        }
        #endregion

        public ImageContainer()
        {
            InitializeComponent();
            UseDefaultImage = true;
            Loaded += ImageContainer_Loaded;
            Unloaded += ImageContainer_Unloaded;
        }

        private void ImageContainer_Loaded(object sender, RoutedEventArgs e)
        {
            //directly change visibility
            //do not use property changed event
            ShowImage();
        }

        private void ImageContainer_Unloaded(object sender, RoutedEventArgs e)
        {
            IsVisible = false;
        }

        #region private method
        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var container = (ImageContainer)d;
            bool isVisible = (bool)e.NewValue;
            if (isVisible)
                container.ShowImage();
            else
                container.ClearImage();
        }

        private void ShowImage()
        {
            if (string.IsNullOrEmpty(ImageUrl))
            {
                if (UseDefaultImage)
                    this.PngImage.Source = defaultImage;
            }
            else
                ImageCacheService.SetImageStream(ImageUrl, SetImageSource);
        }

        private void ClearImage()
        {
            if (timer != null)
            {
                timer.Tick -= DisplayGifImage;
                timer.Stop();
                timer = null;
            }
            this.PngImage.Source = null;
            decoder = null;
            Debug.WriteLine("clear image.");
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageContainer).SetImageSource(e.NewValue as byte[]);
        }

        private void SetImageSource(byte[] data)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    ClearImage();
                    #region clear
                    if (data == null)
                    {
                        //if (UseDefaultImage)
                        //    this.PngImage.Source = defaultImage;
                        return;
                    }
                    #endregion
                    #region set image source
                    try
                    {
                        Debug.WriteLine("set png image. length: {0}", data.Length);
                        var png = new MemoryStream(data);
                        png.Position = 0;
                        var bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(png);
                        this.PngImage.Source = bitmapImage;
                    }
                    catch
                    {
                        Debug.WriteLine("set gif image. length: {0}", data.Length);
                        using (var memStream = new MemoryStream(data))
                        {
                            memStream.Position = 0;
                            decoder = new NGif.GifDecoder();
                            decoder.Read(memStream);
                            if (decoder.FrameCount == 1)
                                this.PngImage.Source = decoder.GetImage();
                            else
                                DisplayGifImage();
                        }
                    }
                    #endregion
                });
        }

        #region use timer to display gif image
        private void DisplayGifImage()
        {
            timer = new DispatcherTimer();
            timer.Tick += DisplayGifImage;
            UpdateInterval();
            timer.Start();
        }

        private void UpdateInterval()
        {
            var interval = decoder.GetDelay(index);
            interval = interval == 0 ? 100 : interval;
            timer.Interval = TimeSpan.FromMilliseconds(interval);
        }

        private void DisplayGifImage(object sender, EventArgs e)
        {
            if (index < decoder.FrameCount)
            {
                this.PngImage.Source = decoder.GetFrame(index);
                index++;
                if (index > decoder.FrameCount - 1)
                    index = 0;
                UpdateInterval();
            }
        }
        #endregion
        #endregion
    }
}
