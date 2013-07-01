using System.Linq;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;

namespace Chicken.Service
{
    public class ImageCacheService
    {
        #region properties
        private static AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        private static bool isStop;
        private static int pendingWorkIndex = 0;
        private static Dictionary<string, int> pendingWorkDic = new Dictionary<string, int>();
        private static Dictionary<string, Stream> imageCacheDic = new Dictionary<string, Stream>();
        //private static Random random = new Random();
        private static readonly object pendingWorkLocker = new object();
        private static readonly object cacheLocker = new object();
        private static List<BackgroundWorker> workers = new List<BackgroundWorker>(5);
        #endregion

        #region event handler
        public static delegate void SetImageStreamEventHandler(Stream imageStream);
        public static SetImageStreamEventHandler SetImageStreamHandler;
        #endregion

        public static void SetImageStream(string url)
        {
            int index = url.IndexOf("?random=");
            string imageUrl = url.Substring(0, url.Length - index);
            string uniqueId = url.Substring(index);

            if (imageCacheDic.ContainsKey(imageUrl))
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("CACHED. " + imageUrl);
#endif
                if (SetImageStreamHandler != null)
                    SetImageStreamHandler(imageCacheDic[imageUrl]);
                return;
            }
            AddWork(url, imageUrl, uniqueId);
            if (pendingWorkIndex == 0)
                autoResetEvent.Set();
            //Thread.Sleep(random.Next(1000));
        }

        private static void AddWork(string url, string imageUrl, string uniqueId)
        {
            autoResetEvent.WaitOne();
            lock (pendingWorkLocker)
            {
                pendingWorkIndex += 1;
                pendingWorkDic.Add(url, pendingWorkIndex);
            }
            autoResetEvent.Set();
        }

        private static void DoWork()
        {
            autoResetEvent.WaitOne();
            lock (pendingWorkLocker)
            {
                var pendinglist = pendingWorkDic.Values.OrderBy(v => v).ToList();
                for (int i = 0; i < workers.Count; i++)
                {
                    if (pendinglist.Count == 0)
                        break;
                    if (workers[i] == null)
                        workers[i] = new BackgroundWorker { WorkerSupportsCancellation = true };
                    if (!workers[i].IsBusy)
                    {
                        workers[i].DoWork -= DownloadImage;
                        workers[i].DoWork += DownloadImage;
                        var pendingwork = pendingWorkDic.First(kvp => kvp.Value == pendinglist.First());
                        workers[i].RunWorkerAsync(pendingwork.Key);
                        pendingWorkDic.Remove(pendingwork.Key);
                        pendinglist.Remove(pendingwork.Value);
                    }
                }
            }
            autoResetEvent.Set();
        }

        private static void DownloadImage(object sender, DoWorkEventArgs e)
        {
            string url = sender as string;
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.BeginGetResponse(
                result =>
                {
                    HttpWebRequest r = result.AsyncState as HttpWebRequest;
                    HttpWebResponse response = (HttpWebResponse)r.EndGetResponse(result);
                    Stream stream = response.GetResponseStream();
                    AddImageCache(url, stream);
                    if (SetImageStreamHandler != null)
                        SetImageStreamHandler(imageCacheDic[url]);
                }, request);
        }

        private static void AddImageCache(string imageUrl, Stream stream)
        {
            lock (cacheLocker)
            {
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                imageCacheDic[imageUrl] = memoryStream;
#if DEBUG
                System.Diagnostics.Debug.WriteLine("add cache: " + imageUrl);
#endif
            }
        }
    }
}
