using System.IO;
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
            ImageCacheService.SetImageStream(newValue, SetImageStream);
        }

        private void SetImageStream(Stream imageStream)
        {
            Dispatcher.BeginInvoke(
                () =>
                {
                    var stream = new MemoryStream();
                    imageStream.Position = 0;
                    imageStream.CopyTo(stream);
                    try
                    {
#if DEBUG
                        System.Diagnostics.Debug.WriteLine("set png image.");
#endif
                        stream.Position = 0;
                        var bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(stream);
                        this.PngImage.Source = bitmapImage;
                        this.Grid.Children.Remove(this.GifImage);
                    }
                    catch
                    {
#if DEBUG
                        System.Diagnostics.Debug.WriteLine("set gif image.");
#endif
                        var gifImage = new ExtendedImage();
                        stream.Position = 0;
                        gifImage.SetSource(stream);
                        this.GifImage.Source = gifImage;
                        this.Grid.Children.Remove(this.PngImage);
                    }
                    finally
                    {
                        this.Grid.Children.Remove(this.Placehold);
                        this.UpdateLayout();
                    }
                });
        }
    }
}
