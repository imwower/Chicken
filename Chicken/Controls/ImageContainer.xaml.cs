using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ImageTools.IO.Gif;

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
                ImageSourceChanged(value);
            }
        }

        private void ImageSourceChanged(string newValue)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                if (newValue.LastIndexOf(".gif", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    this.GifImage.Visibility = Visibility.Visible;
                    this.PngImage.Visibility = Visibility.Collapsed;
                    ImageTools.ExtendedImage image = new ImageTools.ExtendedImage();
                    image.UriSource = new Uri(newValue, UriKind.RelativeOrAbsolute);
                    this.GifImage.Source = image;
                }
                else
                {
                    this.GifImage.Visibility = Visibility.Collapsed;
                    this.PngImage.Visibility = Visibility.Visible;
                    BitmapImage image = new BitmapImage(new Uri(newValue, UriKind.RelativeOrAbsolute));
                    this.PngImage.Source = image;
                }
            }
        }

        public ImageContainer()
        {
            InitializeComponent();
            ImageTools.IO.Decoders.AddDecoder<GifDecoder>();
        }
    }
}
