using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace Chicken.Service
{
    public class ImageCacheService
    {
        #region properties
        private static AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        private static bool isInit;
        private static bool isStop;
        private static List<PendingWork> pendingWorkList = new List<PendingWork>();
        private static Dictionary<string, byte[]> imageCacheDic = new Dictionary<string, byte[]>();
        private static Random random = new Random();
        private static readonly object pendingWorkLocker = new object();
        private static readonly object cacheLocker = new object();
        private static BackgroundWorker doWorker = new BackgroundWorker();
        private const int MAX_WORKER_COUNT = 5;
        private static List<BackgroundWorker> workers = new List<BackgroundWorker>(MAX_WORKER_COUNT);
        #endregion

        public static void SetImageStream(string imageUrl, Action<byte[]> callBack)
        {
            #region if cached
            if (imageCacheDic.ContainsKey(imageUrl))
            {
                Debug.WriteLine("CACHED. " + imageUrl);
                callBack(imageCacheDic[imageUrl]);
                return;
            }
            #endregion
            #region init
            if (!isInit)
            {
                Init();
            }
            #endregion
            #region add to pending work list
            var pendingWork = new PendingWork
            {
                ImageUrl = imageUrl,
                CallBack = callBack
            };
            AddWork(pendingWork);
            #endregion
        }

        #region private method
        private static void Init()
        {
            for (int i = 0; i < MAX_WORKER_COUNT; i++)
            {
                var worker = new BackgroundWorker { WorkerSupportsCancellation = true };
                worker.RunWorkerCompleted += DoWorkCompleted;
                workers.Add(worker);
            }
            doWorker.DoWork += DoWork;
            doWorker.RunWorkerAsync();
            isInit = true;
        }

        private static void AddWork(PendingWork pendingwork)
        {
            lock (pendingWorkLocker)
            {
                #region remove same task
                //var sameWorks = pendingWorkList.Where(w => w.ImageUrl == pendingwork.ImageUrl).ToList();
                //Debug.WriteLine("remove {0} same works.", sameWorks.Count);
                //foreach (var same in sameWorks)
                //{
                //    pendingwork.CallBack += same.CallBack;
                //    pendingWorkList.Remove(same);
                //}
                #endregion
                pendingWorkList.Add(pendingwork);
            }
            autoResetEvent.Set();
        }

        private static void DoWork(object argument, DoWorkEventArgs e)
        {
            while (!isStop)
            {
                autoResetEvent.WaitOne();
                if (e.Cancel)
                {
                    continue;
                }
                Thread.Sleep(random.Next(1000));
                lock (pendingWorkLocker)
                {
                    for (int i = 0; i < workers.Count; i++)
                    {
                        if (pendingWorkList.Count == 0)
                            break;
                        var worker = workers[i];
                        if (!worker.IsBusy)
                        {
                            worker.DoWork += DownloadImage;
                            var pendingwork = pendingWorkList[0];
                            pendingWorkList.RemoveAt(0);
                            Debug.WriteLine("worker {0} starts to work. url: {1}", i, pendingwork.ImageUrl);
                            worker.RunWorkerAsync(pendingwork);
                        }
                    }
                }
            }
        }

        private static void DownloadImage(object argument, DoWorkEventArgs e)
        {
            var pendingwork = e.Argument as PendingWork;
            HttpWebRequest request = WebRequest.CreateHttp(pendingwork.ImageUrl + "?random=" + DateTime.Now.Ticks.ToString("x"));
            request.BeginGetResponse(
                result =>
                {
                    try
                    {
                        HttpWebRequest r = result.AsyncState as HttpWebRequest;
                        HttpWebResponse response = (HttpWebResponse)r.EndGetResponse(result);
                        Stream stream = response.GetResponseStream();
                        AddImageCache(pendingwork.ImageUrl, stream);
                        pendingwork.CallBack(imageCacheDic[pendingwork.ImageUrl]);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                }, request);
        }

        private static void DoWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            (sender as BackgroundWorker).DoWork -= DownloadImage;
        }

        private static void AddImageCache(string imageUrl, Stream stream)
        {
            lock (cacheLocker)
            {
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                imageCacheDic[imageUrl] = memoryStream.ToArray();
                Debug.WriteLine("add cache. url: {0}; length: {1} ", imageUrl, imageCacheDic[imageUrl].Length);
            }
        }
        #endregion

        private class PendingWork
        {
            public string ImageUrl { get; set; }

            public Action<byte[]> CallBack { get; set; }
        }
    }
}
