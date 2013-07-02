using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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
            (d as ImageContainer).SetImageSource(e.NewValue as string);
        }

        private void SetImageSource(string newValue)
        {
            ImageCacheService.SetImageStream(newValue, SetImageSource);
        }

        private void SetImageSource(byte[] data)
        {
            Dispatcher.BeginInvoke(
                () =>
                {
                    Thread.Sleep(67);
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
                        var gif = new MemoryStream(data);
                        gif.Position = 0;
                        var gifImage = new ExtendedImage();
                        gifImage.SetSource(gif);
                        this.GifImage.Source = gifImage;
                    }
                    finally
                    {
                        Debug.WriteLine("remove place hold.");
                        this.Placehold.Visibility = Visibility.Collapsed;
                    }
                });
        }
    }
}
