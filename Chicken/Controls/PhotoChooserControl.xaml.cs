﻿using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;

namespace Chicken.Controls
{
    public partial class PhotoChooserControl : UserControl
    {
        #region private properites
        private PhotoChooserTask photoChooser;
        private Stream chosenPhotoStream = Stream.Null;
        private string fileName;
        #endregion

        #region event handler
        public delegate void CancelAddImageEventHandler();
        public CancelAddImageEventHandler CancelAddImageHandler;
        public delegate void AddImageFinishedEventHandler(string fileName, Stream stream);
        public AddImageFinishedEventHandler AddImageHandler;
        #endregion

        public PhotoChooserControl()
        {
            InitializeComponent();
            var size = App.GetScreenSize();
            this.Height = size.Height;
            this.Width = size.Width;
            photoChooser = new PhotoChooserTask();
            photoChooser.Completed += new EventHandler<PhotoResult>(photoChooser_Completed);
            photoChooser.ShowCamera = true;
            photoChooser.Show();
        }

        void photoChooser_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                this.grid.Children.Remove(this.PlaceHold);
                BitmapImage source = new BitmapImage();
                chosenPhotoStream = e.ChosenPhoto;
                source.SetSource(e.ChosenPhoto);
                this.PhotoPanel.Source = source;
                this.fileName = e.OriginalFileName.Substring(e.OriginalFileName.LastIndexOf("\\") + 1);
            }
            else
            {
                if (CancelAddImageHandler != null)
                {
                    CancelAddImageHandler();
                }
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (AddImageHandler != null && chosenPhotoStream != null)
            {
                Dispatcher.BeginInvoke(
                    () =>
                        AddImageHandler(fileName, chosenPhotoStream));
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            this.fileName = string.Empty;
            photoChooser.Show();
        }
    }
}
