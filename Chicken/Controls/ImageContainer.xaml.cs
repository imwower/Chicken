using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ImageTools;
using ImageTools.IO.Gif;

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
            if (newValue.LastIndexOf(".gif", StringComparison.OrdinalIgnoreCase) != -1)
            {
                container.GifImage.Visibility = Visibility.Visible;
                container.grid.Children.Remove(container.PngImage);
                ExtendedImage image = new ExtendedImage();
                image.DownloadCompleted +=
                    (ext, ev) =>
                    {
                        container.GifImage.Source = ext as ExtendedImage;
                        container.grid.Background = null;
                    };
                image.UriSource = new Uri(newValue, UriKind.RelativeOrAbsolute);
            }
            else
            {
                container.grid.Children.Remove(container.GifImage);
                container.PngImage.Visibility = Visibility.Visible;
                BitmapImage image = new BitmapImage();
                image.CreateOptions = BitmapCreateOptions.None;
                image.ImageOpened +=
                    (bit, ev) =>
                    {
                        container.PngImage.Source = bit as BitmapImage;
                        container.grid.Background = null;
                    };
                image.UriSource = new Uri(newValue, UriKind.RelativeOrAbsolute);
            }
        }

        public ImageContainer()
        {
            InitializeComponent();
            ImageTools.IO.Decoders.AddDecoder<GifDecoder>();
        }
    }
}
