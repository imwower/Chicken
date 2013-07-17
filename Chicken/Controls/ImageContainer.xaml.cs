using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Chicken.Service;
using ImageTools;
using ImageTools.IO;
using ImageTools.IO.Bmp;
using ImageTools.IO.Gif;
using ImageTools.IO.Png;

namespace Chicken.Controls
{
    public partial class ImageContainer : UserControl
    {
        #region private static properties
        private static BitmapImage defaultImage = new BitmapImage(new Uri("/Images/dark/cat.png", UriKind.Relative));
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

        /// <summary>
        /// just display first frame of gif image,
        /// in case of memory leak,
        /// and performance issue.
        /// </summary>
        public ImageContainer()
        {
            InitializeComponent();
            Decoders.AddDecoder<BmpDecoder>();
            Decoders.AddDecoder<PngDecoder>();
            Decoders.AddDecoder<GifDecoder>();
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
            this.PngImage.Source = defaultImage;
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
            this.PngImage.Source = null;
            DownloadCompleted = false;
            Debug.WriteLine("clear image.");
        }

        private void SetImageSource(byte[] data)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    if (data == null)
                        return;
                    #region jpeg/png
                    try
                    {
                        Debug.WriteLine("set png image. length: {0}", data.Length);
                        using (var memStream = new MemoryStream(data))
                        {
                            memStream.Position = 0;
                            var bitmapImage = new BitmapImage();
                            bitmapImage.SetSource(memStream);
                            this.PngImage.Source = bitmapImage;
                        }
                        DownloadCompleted = true;
                    }
                    #endregion
                    #region others
                    catch
                    {
                        Debug.WriteLine("set gif image. length: {0}", data.Length);
                        var memStream = new MemoryStream(data);
                        memStream.Position = 0;
                        var gifImage = new ExtendedImage();
                        gifImage.SetSource(memStream);
                        gifImage.LoadingCompleted += ExtendedImageLoadCompleted;
                    }
                    #endregion
                });
        }

        private void ExtendedImageLoadCompleted(object sender, EventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    var gifImage = sender as ExtendedImage;
                    this.PngImage.Source = gifImage.ToBitmap();
                    gifImage.LoadingCompleted -= ExtendedImageLoadCompleted;
                    gifImage = null;
                    DownloadCompleted = true;
                });
        }
        #endregion
    }
}
