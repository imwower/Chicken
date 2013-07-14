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
            DependencyProperty.Register("ImageUrl", typeof(string), typeof(ImageContainer), new PropertyMetadata(OnImageSourceChanged));

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

        public static readonly DependencyProperty DownloadCompletedProperty =
            DependencyProperty.Register("DownloadCompleted", typeof(bool), typeof(ImageContainer), null);

        public bool DownloadCompleted
        {
            get
            {
                return (bool)GetValue(DownloadCompletedProperty);
            }
            set
            {
                SetValue(DownloadCompletedProperty, value);
            }
        }

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
            Loaded += ImageContainer_Loaded;
            Unloaded += ImageContainer_Unloaded;
        }

        private void ImageContainer_Loaded(object sender, RoutedEventArgs e)
        {
            ShowImage();
        }

        private void ImageContainer_Unloaded(object sender, RoutedEventArgs e)
        {
            ClearImage();
        }

        #region private method
        private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageContainer).ShowImage();
        }

        private void ShowImage()
        {
            if (!string.IsNullOrEmpty(ImageUrl))
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
            this.PngImage.Source = defaultImage;
            DownloadCompleted = false;
            Debug.WriteLine("clear image.");
        }

        private void SetImageSource(byte[] data)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    ClearImage();
                    #region clear
                    if (data == null)
                        return;
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
                    finally
                    {
                        DownloadCompleted = true;
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
