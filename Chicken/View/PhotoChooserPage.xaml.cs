using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using Chicken.Common;
using Chicken.Service;

namespace Chicken.View
{
    public partial class PhotoChooserPage : PhoneApplicationPage
    {
        private PhotoChooserTask photoChooser;
        private Stream chosenPhotoStream;

        public PhotoChooserPage()
        {
            InitializeComponent();
            photoChooser = new PhotoChooserTask();
            photoChooser.Completed += new EventHandler<PhotoResult>(photoChooser_Completed);
            photoChooser.ShowCamera = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            photoChooser.Show();
        }

        void photoChooser_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                this.FileName.Text = e.OriginalFileName.Substring(e.OriginalFileName.LastIndexOf("\\" + 1));
                BitmapImage source = new BitmapImage();
                e.ChosenPhoto.CopyTo(chosenPhotoStream);
                source.SetSource(e.ChosenPhoto);
                this.PhotoPanel.Source = source;
            }
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            if (chosenPhotoStream != null)
            {
                NavigationServiceManager.NavigateTo(Const.NewTweetPage, Const.ChosenPhotoStream, chosenPhotoStream);
            }
        }
    }
}