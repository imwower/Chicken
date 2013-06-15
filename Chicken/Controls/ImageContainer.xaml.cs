using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageTools;
using ImageTools.Controls;
using ImageTools.IO;
using ImageTools.IO.Bmp;
using ImageTools.IO.Gif;
using ImageTools.IO.Png;

namespace Chicken.Controls
{
    public partial class ImageContainer : UserControl
    {
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(ImageContainer),
            new PropertyMetadata(ImageSourceChanged));

        public string ImageSource
        {
            get
            {
                return (string)GetValue(ImageSourceProperty);
            }
            set
            {
                SetValue(ImageSourceProperty, value);
            }
        }

        private static void ImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            #region
            var container = sender as ImageContainer;
            if (container == null)
            {
                return;
            }
            string newValue = e.NewValue as string;
            if (string.IsNullOrEmpty(newValue))
            {
                return;
            }
            #endregion

            #region action
            EventHandler<RoutedEventArgs> pngCompleted = null;
            OpenReadCompletedEventHandler gifCompleted = null;
            EventHandler<ExceptionRoutedEventArgs> pngFailed = null;

            pngCompleted = delegate(object pngImage, RoutedEventArgs gifArgs)
            {
                Image pngImageControl = new Image();
                pngImageControl.Stretch = Stretch.Fill;
                var bitmapImage = pngImage as BitmapImage;
                pngImageControl.Source = bitmapImage;
                container.grid.Children.Add(pngImageControl);
                container.grid.Children.Remove(container.placehold);
                bitmapImage.ImageOpened -= pngCompleted;
                bitmapImage.ImageFailed -= pngFailed;
            };

            gifCompleted = delegate(object gifImage, OpenReadCompletedEventArgs gifArgs)
            {
                AnimatedImage gifImageControl = new AnimatedImage();
                gifImageControl.Stretch = Stretch.Fill;
                var extendedImage = gifImage as ExtendedImage;
                gifImageControl.Source = extendedImage;
                container.grid.Children.Add(gifImageControl);
                container.grid.Children.Remove(container.placehold);
                extendedImage.DownloadCompleted -= gifCompleted;
            };

            #region png image failed action
            pngFailed = delegate(object imageFailed, ExceptionRoutedEventArgs failedArgs)
            {
                ExtendedImage gifImage = new ExtendedImage();
                gifImage.DownloadCompleted += new OpenReadCompletedEventHandler(gifCompleted);
                (imageFailed as BitmapImage).ImageFailed -= pngFailed;

                gifImage.UriSource = new Uri(newValue, UriKind.RelativeOrAbsolute);
            };
            #endregion
            #endregion

            BitmapImage image = new BitmapImage();
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.ImageOpened += new EventHandler<RoutedEventArgs>(pngCompleted);
            image.ImageFailed += new EventHandler<ExceptionRoutedEventArgs>(pngFailed);

            image.UriSource = new Uri(newValue, UriKind.RelativeOrAbsolute);
        }

        public ImageContainer()
        {
            InitializeComponent();
            Decoders.AddDecoder<BmpDecoder>();
            Decoders.AddDecoder<PngDecoder>();
            Decoders.AddDecoder<GifDecoder>();
        }
    }
}
