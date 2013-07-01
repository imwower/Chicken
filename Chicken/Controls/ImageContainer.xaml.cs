using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ImageTools;
using ImageTools.IO;
using ImageTools.IO.Bmp;
using ImageTools.IO.Gif;
using ImageTools.IO.Png;
using Chicken.Service;

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
            ThreadPool.SetMaxThreads(5, 5);
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
            //
            ImageCacheService.SetImageStreamHandler += this.SetImageStream;
            ImageCacheService.SetImageStream(newValue);
        }

        //private void DownloadImage(object state)
        //{
        //    string url = state as string;
        //    HttpWebRequest request = WebRequest.CreateHttp(url);
        //    request.BeginGetResponse(
        //        result =>
        //        {
        //            HttpWebRequest r = result.AsyncState as HttpWebRequest;
        //            HttpWebResponse response = (HttpWebResponse)r.EndGetResponse(result);
        //            Stream stream = response.GetResponseStream();
        //            AddCache(url, stream);
        //            SetImageStream(url);
        //        }, request);
        //}

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

        //        private static void AddCache(string key, Stream stream)
        //        {
        //            lock (locker)
        //            {
        //                var memoryStream = new MemoryStream();
        //                stream.CopyTo(memoryStream);
        //                cache[key] = memoryStream;
        //#if DEBUG
        //                System.Diagnostics.Debug.WriteLine("add cache: " + key);
        //#endif
        //            }
        //        }
    }
}
