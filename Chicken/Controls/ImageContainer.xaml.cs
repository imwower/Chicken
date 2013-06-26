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
            DependencyProperty.Register("ImageSource", typeof(string), typeof(ImageContainer), null);

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

        public ImageContainer()
        {
            InitializeComponent();
            Decoders.AddDecoder<BmpDecoder>();
            Decoders.AddDecoder<PngDecoder>();
            Decoders.AddDecoder<GifDecoder>();
        }

        private void png_ImageOpened(object sender, RoutedEventArgs e)
        {
            //this.grid.Children.Remove(this.placehold);
            this.placehold.Visibility = Visibility.Collapsed;
            //this.UpdateLayout();
        }

        private void png_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            BitmapImage bitmapImage = (BitmapImage)(sender as Image).Source;
            ExtendedImage gifImage = new ExtendedImage();
            gifImage.DownloadCompleted += gifImage_DownloadCompleted;
            gifImage.UriSource = bitmapImage.UriSource;
        }

        private void gifImage_DownloadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            var gifImage = sender as ExtendedImage;
            this.GifImage.Source = gifImage;
            //this.grid.Children.Remove(this.placehold);
            this.placehold.Visibility = Visibility.Collapsed;
            //this.UpdateLayout();
            gifImage.DownloadCompleted -= gifImage_DownloadCompleted;
        }
    }
}
