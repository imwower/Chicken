using System;
using System.Diagnostics;
using System.IO;
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
            DependencyProperty.Register("ImageSource", typeof(byte[]), typeof(ImageContainer), new PropertyMetadata(OnSourceChanged));

        public byte[] ImageSource
        {
            get
            {
                return (byte[])GetValue(ImageSourceProperty);
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
            (d as ImageContainer).SetImageSource(e.NewValue as byte[]);
        }

        private void SetImageSource(byte[] data)
        {
            Dispatcher.BeginInvoke(
                () =>
                {
                    #region clear
                    if (data == null)
                    {
                        if (this.PngImage.Source != null)
                            this.PngImage.Source = null;
                        if (this.GifImage.Source != null)
                        {
                            this.GifImage.Stop();
                            this.GifImage.Source = null;
                        }
                        this.PngImage.Source = defaultImage;
                        Debug.WriteLine("clear image.");
                        return;
                    }
                    #endregion
                    #region set image source
                    try
                    {
                        Debug.WriteLine("set png image. length: {0}", data.Length);
                        var png = new MemoryStream(data);
                        png.Position = 0;
                        var bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(png);
                        this.PngImage.Source = bitmapImage;
                        if (this.GifImage.Source != null)
                        {
                            this.GifImage.Stop();
                            this.GifImage.Source = null;
                        }
                    }
                    catch
                    {
                        Debug.WriteLine("set gif image. length: {0}", data.Length);
                        var gif = new MemoryStream(data);
                        gif.Position = 0;
                        var gifImage = new ExtendedImage();
                        gifImage.SetSource(gif);
                        this.GifImage.Source = gifImage;
                        this.PngImage.Source = null;
                    }
                    finally
                    {
                        Debug.WriteLine("remove place hold.");
                    }
                    #endregion
                });
        }

        private static BitmapImage defaultImage = new BitmapImage(new Uri("/Images/dark/cat.png", UriKind.Relative));
    }
}
