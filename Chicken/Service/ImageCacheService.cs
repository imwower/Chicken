using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private const int MAX_CACHE_ITEM = 50;
        private static int cache_index = 0;
        private static Dictionary<string, WeakData> imageCacheDic = new Dictionary<string, WeakData>(MAX_CACHE_ITEM);
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
            if (imageCacheDic.ContainsKey(imageUrl)
                && imageCacheDic[imageUrl].Data != null)
            {
                Debug.WriteLine("CACHED. " + imageUrl);
                callBack((byte[])imageCacheDic[imageUrl].Data);
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
                var worker = new BackgroundWorker
                {
                    WorkerSupportsCancellation = true,
                };
                worker.RunWorkerCompleted += DownloadImageComplete;
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
                var sameWorks = pendingWorkList.Where(w => w.ImageUrl == pendingwork.ImageUrl).ToList();
                Debug.WriteLine("remove {0} same works.", sameWorks.Count);
                foreach (var same in sameWorks)
                {
                    pendingwork.CallBack += same.CallBack;
                    pendingWorkList.Remove(same);
                }
                #endregion
                Debug.WriteLine("add new work. url: {0}", pendingwork.ImageUrl);
                pendingWorkList.Add(pendingwork);
            }
            autoResetEvent.Set();
        }

        private static void DoWork(object argument, DoWorkEventArgs e)
        {
            while (!isStop)
            {
                if (pendingWorkList.Count == 0)
                {
                    Debug.WriteLine("wait for pending work...");
                    autoResetEvent.WaitOne();
                    Debug.WriteLine("start to wok. number is: {0}", pendingWorkList.Count);
                    continue;
                }
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

        private static void DownloadImage(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(random.Next(500));
            if (e.Cancel)
                return;
            var pendingwork = e.Argument as PendingWork;
            pendingwork.Request = WebRequest.CreateHttp(pendingwork.ImageUrl + "?random=" + DateTime.Now.Ticks.ToString("x"));
            pendingwork.Request.BeginGetResponse(DownloadImage, pendingwork);
        }

        private static void DownloadImage(IAsyncResult result)
        {
            var pendingwork = (PendingWork)result.AsyncState;
            try
            {
                var response = pendingwork.Request.EndGetResponse(result);
                using (Stream stream = response.GetResponseStream())
                {
                    var memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    var data = memoryStream.ToArray();
                    AddImageCache(pendingwork.ImageUrl, data);
                }
                response.Close();
                pendingwork.CallBack((byte[])imageCacheDic[pendingwork.ImageUrl].Data);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                pendingwork.Request.Abort();
                if (pendingwork.RetryCount <= MAX_WORKER_COUNT)
                {
                    pendingwork.RetryCount += 1;
                    AddWork(pendingwork);
                }
            }
        }

        private static void DownloadImageComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            (sender as BackgroundWorker).DoWork -= DownloadImage;
        }

        private static void AddImageCache(string imageUrl, byte[] data)
        {
            lock (cacheLocker)
            {
                cache_index += 1;
                imageCacheDic[imageUrl] = new WeakData(cache_index, data);
                Debug.WriteLine("add cache. url: {0}", imageUrl);
                if (imageCacheDic.Count > MAX_CACHE_ITEM)
                {
                    var items = imageCacheDic.OrderBy(d => d.Value.Index).Take(20).ToList();
                    foreach (var item in items)
                        imageCacheDic.Remove(item.Key);
                }
            }
        }
        #endregion

        private class PendingWork
        {
            public HttpWebRequest Request { get; set; }

            public int RetryCount { get; set; }

            public string ImageUrl { get; set; }

            public Action<byte[]> CallBack { get; set; }
        }

        private class WeakData
        {
            private int index;
            //private WeakReference data;
            private byte[] data;

            public WeakData(int index, byte[] data)
            {
                this.index = index;
                //this.data = new WeakReference(data);
                this.data = data;
            }

            public int Index
            {
                get
                {
                    return index;
                }
            }

            public byte[] Data
            {
                get
                {
                    //if (data.IsAlive)
                    //return (byte[])data.Target;
                    //return null;
                    return data;
                }
            }
        }
    }
}
