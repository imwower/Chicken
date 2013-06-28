using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ImageTools;
using ImageTools.IO;
using ImageTools.IO.Bmp;
using ImageTools.IO.Gif;
using ImageTools.IO.Png;

namespace Chicken.Controls
{
    public partial class ImageContainer : UserControl
    {
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(ImageContainer), new PropertyMetadata(OnSourceChanged));

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

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageContainer).OnSourceChanged(e.NewValue as string);
        }

        private void OnSourceChanged(string newValue)
        {
            this.PngImage.ImageOpened -= png_ImageOpened;
            this.PngImage.ImageFailed -= png_ImageFailed;
            this.GifImage.LoadingCompleted -= gifImage_DownloadCompleted;

            this.PngImage.Source = new BitmapImage(new Uri(newValue));
            this.PngImage.ImageOpened += png_ImageOpened;
            this.PngImage.ImageFailed += png_ImageFailed;
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
            this.GifImage.Source = new ExtendedImage { UriSource = bitmapImage.UriSource };
            this.GifImage.LoadingCompleted += gifImage_DownloadCompleted;

            this.PngImage.ImageOpened -= png_ImageOpened;
            this.PngImage.ImageFailed -= png_ImageFailed;
        }

        private void gifImage_DownloadCompleted(object sender, EventArgs e)
        {
            //this.grid.Children.Remove(this.placehold);
            this.placehold.Visibility = Visibility.Collapsed;
            //this.UpdateLayout();
            this.GifImage.LoadingCompleted -= gifImage_DownloadCompleted;
        }
    }
}
