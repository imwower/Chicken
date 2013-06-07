using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ImageTools;
using ImageTools.IO.Gif;
using System.Net;

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
                container.grid.Children.Remove(container.GifImage);
                container.PngImage.Visibility = Visibility.Visible;
                var bitmapImage = pngImage as BitmapImage;
                container.PngImage.Source = bitmapImage;
                container.grid.Background = null;
                bitmapImage.ImageOpened -= pngCompleted;
                bitmapImage.ImageFailed -= pngFailed;
            };

            gifCompleted = delegate(object gifImage, OpenReadCompletedEventArgs gifArgs)
            {
                container.grid.Children.Remove(container.PngImage);
                container.GifImage.Visibility = Visibility.Visible;
                var extendedImage = gifImage as ExtendedImage;
                container.GifImage.Source = extendedImage;
                container.grid.Background = null;
                extendedImage.DownloadCompleted -= gifCompleted;
            };

            #region png image failed action
            pngFailed = delegate(object imageFailed, ExceptionRoutedEventArgs failedArgs)
            {
                container.GifImage.Visibility = Visibility.Visible;
                container.grid.Children.Remove(container.PngImage);
                ExtendedImage gifImage = new ExtendedImage();
                gifImage.UriSource = new Uri(newValue, UriKind.RelativeOrAbsolute);
                gifImage.DownloadCompleted += gifCompleted;
                (imageFailed as BitmapImage).ImageFailed -= pngFailed;
            };
            #endregion
            #endregion

            BitmapImage image = new BitmapImage();
            image.CreateOptions = BitmapCreateOptions.None;
            image.UriSource = new Uri(newValue, UriKind.RelativeOrAbsolute);
            image.ImageOpened += pngCompleted;
            image.ImageFailed += pngFailed;
        }

        public ImageContainer()
        {
            InitializeComponent();
            ImageTools.IO.Decoders.AddDecoder<GifDecoder>();
        }
    }
}
