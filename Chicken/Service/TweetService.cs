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
using Chicken.Model;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace Chicken.Service
{
    public class TweetService : ITweetService
    {

        public List<Tweet> GetHomeLineTweets()
        {
            var reader = System.Windows.Application.GetResourceStream(new Uri("SampleData/hometimeline.json", UriKind.Relative));
            StreamReader streamReader = new StreamReader(reader.Stream);
            string output = streamReader.ReadToEnd();
            var tweets = JsonConvert.DeserializeObject<List<Tweet>>(output);
            return tweets;
        }
    }
}
