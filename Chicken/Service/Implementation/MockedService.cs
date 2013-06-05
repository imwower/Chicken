using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service.Interface;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using Chicken.ViewModel.NewTweet.Base;

namespace Chicken.Service.Implementation
{
    public class MockedService : ITweetService
    {
        #region properties
        private static JsonSerializer jsonSerializer = new JsonSerializer();
        private static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Populate
        };
        #endregion

        #region private method
        private void HandleWebRequest<T>(string url, Action<T> callBack, string method = Const.HTTPGET)
        {
            var streamInfo = Application.GetResourceStream(new Uri(url, UriKind.Relative));
            var result = default(T);
            using (var reader = new StreamReader(streamInfo.Stream))
            {
                string s = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<T>(s, jsonSettings);
            }
            if (callBack != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(
                    () =>
                    {
                        callBack(result);
                    });
            }
        }
        #endregion

        #region Home Page
        public void GetTweets<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/hometimeline.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetMentions<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/mentions.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetDirectMessages<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/direct_messages.json";
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region profile page
        public void GetUserProfileDetail<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/userProfile.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetUserTweets<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/user_timeline.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetFollowingIds<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/followingIds.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetFollowerIds<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/followingIds.json";
            //string url = "SampleData/errors.json";
            HandleWebRequest(url, callBack);
        }

        public void GetUserProfiles<T>(string userIds, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/lookup.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetUserFavorites<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/user_favourites.json";
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region status page
        public void GetStatusDetail<T>(string statusId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/tweet.json";
            HandleWebRequest<T>(url, callBack);
        }
        public void GetStatusRetweetIds<T>(string statusId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/followingIds.json";
            HandleWebRequest(url, callBack);
        }

        #endregion

        #region new tweet
        public void PostNewTweet<T>(string text, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/tweet.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void PostNewTweet<T>(string text, Stream mediaStream, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            //parameters = TwitterHelper.GetDictionary(parameters);
            //parameters.Add(Const.STATUS, text);
            //string url = Const.API_IMAGE + Const.STATUS_POST_NEW_TWEET_WITH_MEDIA;
            //HandleWebRequestStream<T>(url, mediaStream, callBack, parameters);
        }

        public void PostNewTweet<T>(NewTweetViewModel newTweet, Action<object> callBack)
        {
            string url = Const.API_IMAGE + Const.STATUS_POST_NEW_TWEET_WITH_MEDIA;
            HandleWebRequestStream(url, typeof(T), newTweet, callBack);
        }

        #endregion

        private void HandleWebRequestStream(string url, Type outputType, NewTweetViewModel newTweet, Action<object> callBack)
        {
            //url = "http://posttestserver.com/post.php";
            url = Const.API_IMAGE + Const.STATUS_POST_NEW_TWEET_WITH_MEDIA;
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.Method = Const.HTTPPOST;
            DTO dto = new DTO
            {
                Request = wr,
                Data = newTweet,
                CallBack = callBack,
            };
            wr.BeginGetRequestStream(GetRequestStream, dto);
        }

        private void GetRequestStream(IAsyncResult iac)
        {
            var dto = iac.AsyncState as DTO;
            HttpWebRequest wr = dto.Request;
            NewTweetViewModel data = dto.Data as NewTweetViewModel;

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            //wr.Method = "POST";
            Stream rs = wr.EndGetRequestStream(iac);
            Dictionary<string, string> nvc = new Dictionary<string, string>();
            nvc.Add("id", "TTR");
            nvc.Add("btn-submit-photo", "Upload");

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }

            rs.Write(boundarybytes, 0, boundarybytes.Length);
            //FILE:
            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "media[]", data.FileName, "image/jpg");
            byte[] headerbytes = Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            data.MediaStream.Position = 0;
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = data.MediaStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            data.MediaStream.Close();
            byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Flush();
            rs.Close();

            wr.BeginGetResponse(GetResponse, dto);
        }

        private void GetResponse(IAsyncResult iac)
        {
            var dto = iac.AsyncState as DTO;
            HttpWebRequest wr = dto.Request;
            NewTweetViewModel data = dto.Data as NewTweetViewModel;

            var response = wr.EndGetResponse(iac);
            var stream = response.GetResponseStream();
            string responseString = new StreamReader(stream).ReadToEnd();
        }

        public void UpdateProfileImage(string base64string)
        {
            string url = Const.API + "account/update_profile_image.json";
            //url = "http://posttestserver.com/post.php";
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.Method = Const.HTTPPOST;
            wr.BeginGetRequestStream(
                result =>
                {
                    string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                    byte[] boundarybytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
                    wr.ContentType = "multipart/form-data; boundary=" + boundary;
                    Stream rs = wr.EndGetRequestStream(result);
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\";filename=\"image.jpg\"\r\n"
                        + "Content-Type: image/jpg\r\n\r\n{1}";
                    string formitem = string.Format(formdataTemplate, "image", base64string);
                    byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                    byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                    rs.Write(trailer, 0, trailer.Length);
                    rs.Flush();
                    rs.Close();

                    wr.BeginGetResponse(
                        response =>
                        {
                            var res = wr.EndGetResponse(response);
                            var stream = res.GetResponseStream();
                            string responseString = new StreamReader(stream).ReadToEnd();
                            Console.ReadLine();
                        }, wr);

                }, wr);
        }
    }

    public class DTO
    {
        public HttpWebRequest Request;
        public object Data;
        public Action<object> CallBack;
    }
}
