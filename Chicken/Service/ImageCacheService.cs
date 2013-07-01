﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private static int pendingWorkIndex = 0;
        private static Dictionary<string, PendingWork> pendingWorkDic = new Dictionary<string, PendingWork>();
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
            if (imageCacheDic.ContainsKey(imageUrl))
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("CACHED. " + imageUrl);
#endif
                callBack(imageCacheDic[imageUrl]);
                return;
            }
            #region init
            if (!isInit)
            {
                Init();
            }
            #endregion
            var pendingWork = new PendingWork
            {
                ImageUrl = imageUrl,
                CallBack = callBack
            };
            AddWork(pendingWork);
        }

        private static void Init()
        {
            for (int i = 0; i < MAX_WORKER_COUNT; i++)
            {
                var worker = new BackgroundWorker { WorkerSupportsCancellation = true };
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
                if (!pendingWorkDic.ContainsKey(pendingwork.ImageUrl))
                {
                    pendingwork.Index = pendingWorkIndex += 1;
                    pendingWorkDic.Add(pendingwork.ImageUrl, pendingwork);
                }
            }
            autoResetEvent.Set();
        }

        private static void DoWork(object argument, DoWorkEventArgs e)
        {
            while (!isStop)
            {
                autoResetEvent.WaitOne();
                lock (pendingWorkLocker)
                {
                    var pendinglist = pendingWorkDic.Values.OrderBy(v => v.Index).ToList();
                    for (int i = 0; i < workers.Count; i++)
                    {
                        if (pendinglist.Count == 0)
                            break;
                        var worker = workers[i];
                        if (!worker.IsBusy)
                        {
                            worker.DoWork -= DownloadImage;
                            worker.DoWork += DownloadImage;
                            var pendingwork = pendingWorkDic.First(kvp => kvp.Value.Index == pendinglist.First().Index);
#if DEBUG
                            System.Diagnostics.Debug.WriteLine("worker {0} starts to work.", i);
#endif
                            worker.RunWorkerAsync(pendingwork.Value);
                            pendingWorkDic.Remove(pendingwork.Key);
                            pendinglist.RemoveAt(0);
                        }
                    }
                }
            }
        }

        private static void DownloadImage(object argument, DoWorkEventArgs e)
        {
            var pendingwork = e.Argument as PendingWork;
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(pendingwork.ImageUrl);
                request.BeginGetResponse(
                    result =>
                    {
                        HttpWebRequest r = result.AsyncState as HttpWebRequest;
                        HttpWebResponse response = (HttpWebResponse)r.EndGetResponse(result);
                        Stream stream = response.GetResponseStream();
                        AddImageCache(pendingwork.ImageUrl, stream);
                        pendingwork.CallBack(imageCacheDic[pendingwork.ImageUrl]);
                    }, request);
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }
        }

        private static void AddImageCache(string imageUrl, Stream stream)
        {
            lock (cacheLocker)
            {
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                imageCacheDic[imageUrl] = memoryStream.ToArray();
#if DEBUG
                System.Diagnostics.Debug.WriteLine("add cache. url: {0}; length: {1} ", imageUrl, imageCacheDic[imageUrl].Length);
#endif
            }
        }

        private class PendingWork
        {
            public int Index { get; set; }

            public string ImageUrl { get; set; }

            public Action<byte[]> CallBack { get; set; }
        }
    }
}
