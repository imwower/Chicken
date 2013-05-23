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
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Chicken.Model;
using Chicken.Common;

namespace Chicken.Service
{
    public class MockedService : ITweetService
    {
        public void GetLastedTweets<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            var reader = System.Windows.Application.GetResourceStream(new Uri("SampleData/hometimeline.json", UriKind.Relative));
            StreamReader streamReader = new StreamReader(reader.Stream);
            string output = streamReader.ReadToEnd();
            var tweets = JsonConvert.DeserializeObject<T>(output);
            HandleWebRequest<T>(callBack, tweets);
        }

        public void GetUserProfile<T>(string userId, Action<T> callBack)
        {
            throw new NotImplementedException();
        }

        private void HandleWebRequest<T>(string url, Action<T> callBack)
        {
            var reader = Application.GetResourceStream(new Uri("SampleData/hometimeline.json", UriKind.Relative));
            StreamReader streamReader = new StreamReader(reader.Stream);
            string output = streamReader.ReadToEnd();
            var tweets = JsonConvert.DeserializeObject<T>(output);
            if (callBack != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(
                    () =>
                    {
                        callBack(parameter);
                    });
            }
        }
    }
}
