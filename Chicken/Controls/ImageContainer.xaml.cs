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

            var bitmapImage = new BitmapImage
            {
                UriSource = new Uri(newValue + "?random=" + DateTime.Now.Ticks.ToString("x")),
                CreateOptions = BitmapCreateOptions.BackgroundCreation
            };
            this.PngImage.Source = bitmapImage;
            this.PngImage.ImageOpened += png_ImageOpened;
            this.PngImage.ImageFailed += png_ImageFailed;

            this.source.Text = "BEGIN. " + bitmapImage.UriSource.OriginalString;

        }

        private void png_ImageOpened(object sender, RoutedEventArgs e)
        {
            this.placehold.Visibility = Visibility.Collapsed;
            this.UpdateLayout();

            var bitmapImage = (sender as Image).Source as BitmapImage;
            this.source.Text += "END. " + bitmapImage.PixelHeight;
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
            this.placehold.Visibility = Visibility.Collapsed;
            this.UpdateLayout();
            this.GifImage.LoadingCompleted -= gifImage_DownloadCompleted;
        }
    }
}
