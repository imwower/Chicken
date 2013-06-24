using System;
using System.Collections.Generic;
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

            #region cache
            if (pngCache.ContainsKey(newValue))
            {
                Image pngImageControl = new Image();
                pngImageControl.Stretch = Stretch.Fill;
                pngImageControl.Source = pngCache[newValue];
                container.grid.Children.Add(pngImageControl);
                container.grid.Children.Remove(container.placehold);
                return;
            }
            else if (gifCache.ContainsKey(newValue))
            {
                AnimatedImage gifImageControl = new AnimatedImage();
                gifImageControl.Stretch = Stretch.Fill;
                gifImageControl.Source = gifCache[newValue];
                container.grid.Children.Add(gifImageControl);
                container.grid.Children.Remove(container.placehold);
                return;
            }
            #endregion

            BitmapImage image = new BitmapImage();
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.ImageOpened += container.png_ImageOpened;
            image.ImageFailed += container.png_ImageFailed;
            image.UriSource = new Uri(newValue, UriKind.RelativeOrAbsolute);
        }

        #region static properties
        private static readonly object pnglocker = new object();
        private static readonly object giflocker = new object();
        private static Dictionary<string, BitmapImage> pngCache = new Dictionary<string, BitmapImage>();
        private static Dictionary<string, ExtendedImage> gifCache = new Dictionary<string, ExtendedImage>();
        #endregion

        public ImageContainer()
        {
            InitializeComponent();
            Decoders.AddDecoder<BmpDecoder>();
            Decoders.AddDecoder<PngDecoder>();
            Decoders.AddDecoder<GifDecoder>();
        }

        private void png_ImageOpened(object sender, RoutedEventArgs e)
        {
            Image pngImageControl = new Image();
            pngImageControl.Stretch = Stretch.Fill;
            var bitmapImage = sender as BitmapImage;
            pngImageControl.Source = bitmapImage;
            this.grid.Children.Add(pngImageControl);
            //this.grid.Children.Remove(this.placehold);
            this.placehold.Visibility = Visibility.Collapsed;
            this.UpdateLayout();

            lock (pnglocker)
            {
                pngCache[bitmapImage.UriSource.OriginalString] = bitmapImage;
            }
            bitmapImage.ImageOpened -= png_ImageOpened;
            bitmapImage.ImageFailed -= png_ImageFailed;
        }

        private void png_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var bitmapImage = sender as BitmapImage;
            ExtendedImage gifImage = new ExtendedImage();
            gifImage.DownloadCompleted += gifImage_DownloadCompleted;
            gifImage.UriSource = bitmapImage.UriSource;
            bitmapImage.ImageOpened -= png_ImageOpened;
            bitmapImage.ImageFailed -= png_ImageFailed;
        }

        private void gifImage_DownloadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            var gifImage = sender as ExtendedImage;
            AnimatedImage gifImageControl = new AnimatedImage();
            gifImageControl.Stretch = Stretch.Fill;
            gifImageControl.Source = gifImage;
            this.grid.Children.Add(gifImageControl);
            //this.grid.Children.Remove(this.placehold);
            this.placehold.Visibility = Visibility.Collapsed;
            this.UpdateLayout();

            lock (giflocker)
            {
                gifCache[gifImage.UriSource.OriginalString] = gifImage;
            }
            gifImage.DownloadCompleted -= gifImage_DownloadCompleted;
        }
    }
}
