//using System;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
//using System.ComponentModel;
//using System.Threading;

//namespace Chicken.ViewModel
//{
//    public class ViewModelWorker
//    {
//        protected delegate void RefreshEventHandler(object sender, EventArgs e);
//        public RefreshEventHandler RefreshHandler;

//        private BackgroundWorker worker;

//        public ViewModelWorker()
//        {
//            worker = new BackgroundWorker();
//            worker.WorkerSupportsCancellation = true;
//        }

//        public virtual void Refresh()
//        {
//            if (RefreshHandler == null)
//            {
//                return;
//            }
//            if (worker.IsBusy)
//            {
//                Thread.Sleep(1000);
//                return;
//            }
//            else
//            {
//                worker.DoWork += DoRefresh;
//                worker.RunWorkerCompleted += RefreshCompleted;
//                worker.RunWorkerAsync();
//            }
//        }

//        private void DoRefresh(object sender, DoWorkEventArgs e)
//        {
//            Thread.Sleep(2000);
//            if (worker.CancellationPending)
//            {
//                e.Cancel = true;
//                return;
//            }
//            Deployment.Current.Dispatcher.BeginInvoke(
//                () =>
//                {
//                    RefreshHandler(sender, e);
//                });
//        }

//        private void RefreshCompleted(object sender, RunWorkerCompletedEventArgs e)
//        {
//            worker.DoWork -= DoRefresh;
//        }
//    }
//}
