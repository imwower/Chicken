using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service.Interface;
using Newtonsoft.Json;
using System.Text;

namespace Chicken.Service.Implementation
{
    public class TweetService : ITweetService
    {
        #region properties
        #region properties
        private static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };
        private static JsonSerializer jsonSerializer = JsonSerializer.Create(jsonSettings);
        #endregion
        #endregion

        #region private method
        private void HandleWebRequest<T>(string url, Action<T> callBack, string method = Const.HTTPGET)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = method.ToString();
            request.BeginGetResponse(
                (result) =>
                {
                    HttpWebRequest requestResult = (HttpWebRequest)result.AsyncState;
                    WebResponse response = null;
                    StreamReader streamReader = StreamReader.Null;
                    T output = default(T);
#if !RELEASE
                    response = requestResult.EndGetResponse(result);
                    streamReader = new StreamReader(response.GetResponseStream());
                    string s = streamReader.ReadToEnd();
                    output = JsonConvert.DeserializeObject<T>(s, jsonSettings);
#else
                    try
                    {
                        response = requestResult.EndGetResponse(result);
                        using (streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            using (var reader = new JsonTextReader(streamReader))
                            {
                                output = jsonSerializer.Deserialize<T>(reader);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                    finally
                    {
#endif
                    if (callBack != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(
                            () =>
                            {
                                callBack(output);
                            });
                    }
                    if (streamReader != null)
                    {
                        streamReader.Close();
                        streamReader.Dispose();
                        streamReader = null;
                    }
                    request = null;
                    requestResult = null;
                    if (response != null)
                    {
                        response.Close();
                        response.Dispose();
                        response = null;
                    }
#if RELEASE
                    }
#endif
                },
              request);
        }

        public static IDictionary<string, object> CheckSinceIdAndMaxId(IDictionary<string, object> parameters)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            if (parameters.ContainsKey(Const.SINCE_ID) ||
                parameters.ContainsKey(Const.MAX_ID))
            {
                parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE_PLUS_ONE);
            }
            else
            {
                parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            }
            return parameters;
        }

        #endregion

        #region Home Page
        public void GetTweets<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            string url = TwitterHelper.GenerateUrlParams(Const.STATUSES_HOMETIMELINE, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetMentions<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            string url = TwitterHelper.GenerateUrlParams(Const.STATUSES_MENTIONS_TIMELINE, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetDirectMessages<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            parameters.Add(Const.DIRECT_MESSAGE_SKIP_STATUS, Const.DEFAULT_VALUE_TRUE);
            string url = TwitterHelper.GenerateUrlParams(Const.DIRECT_MESSAGES, parameters);
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region profile page
        public void GetUserProfileDetail<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.USER_ID, userId);
            string url = TwitterHelper.GenerateUrlParams(Const.USERS_SHOW, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetUserTweets<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            parameters.Add(Const.USER_ID, userId);
            string url = TwitterHelper.GenerateUrlParams(Const.USER_TIMELINE, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetFollowingIds<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.USER_ID, userId);
            parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            string url = TwitterHelper.GenerateUrlParams(Const.USER_FOLLOWING_IDS, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetFollowerIds<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.USER_ID, userId);
            parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            string url = TwitterHelper.GenerateUrlParams(Const.USER_FOLLOWER_IDS, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetUserProfiles<T>(string userIds, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.USER_ID, userIds);
            parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            parameters.Add(Const.INCLUDE_ENTITIES, Const.DEFAULT_VALUE_FALSE);
            string url = TwitterHelper.GenerateUrlParams(Const.USERS_LOOKUP, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetUserFavorites<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            parameters.Add(Const.USER_ID, userId);
            string url = TwitterHelper.GenerateUrlParams(Const.USER_FAVORITE, parameters);
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region Status Page
        public void GetStatusDetail<T>(string statusId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.ID, statusId);
            string url = TwitterHelper.GenerateUrlParams(Const.STATUSES_SHOW, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetStatusRetweetIds<T>(string statusId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.ID, statusId);
            parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            string url = TwitterHelper.GenerateUrlParams(Const.STATUSES_RETWEET_IDS, parameters);
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region new tweet
        public void PostNewTweet<T>(string text, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.STATUS, text);
            string url = TwitterHelper.GenerateUrlParams(Const.STATUS_POST_NEW_TWEET, parameters);
            HandleWebRequest<T>(url, callBack, Const.HTTPPOST);
        }

        public void PostNewTweet<T>(string text, Stream mediaStream, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            if (mediaStream == null)
            {
                PostNewTweet<T>(text, callBack, parameters);
                return;
            }
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.STATUS, text);
            string url = Const.API_IMAGE + Const.STATUS_POST_NEW_TWEET_WITH_MEDIA;
            HandleWebRequestStream<T>(url, mediaStream, callBack, parameters);
        }
        #endregion

        private void HandleWebRequestStream<T>(string url, Stream mediaStream, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            //url = "http://posttestserver.com/post.php";
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = Const.HTTPPOST;
            request.ContentType = "multipart/form-data;boundary=" + Const.BOUNDARY;
            request.BeginGetRequestStream(
                requestResult =>
                {
                    #region builder
                    HttpWebRequest requestStreamResult = requestResult.AsyncState as HttpWebRequest;
                    Stream outputStream = requestStreamResult.EndGetRequestStream(requestResult);
                    //
                    StringBuilder builder = new StringBuilder();
                    //status:
                    foreach (var param in parameters)
                    {
                        builder.Append("--").AppendLine(Const.BOUNDARY);
                        builder.Append("Content-Disposition:form-data;name=\"").Append(param.Key).Append("\"").AppendLine();
                        builder.AppendLine(HttpUtility.UrlEncode(param.Value.ToString()));
                    }
                    string status = builder.ToString();
                    byte[] statusBytes = Encoding.UTF8.GetBytes(status);
                    outputStream.Write(statusBytes, 0, status.Length);
                    builder.Clear();
                    //media:
                    builder.Append("--").AppendLine(Const.BOUNDARY);
                    builder.Append("Content-Disposition:form-data;name=\"media[]\";filename=\"media.png\"").AppendLine();
                    builder.AppendLine("Content-Type:application/octet-stream");
                    builder.AppendLine("Content-Transfer-Encoding:binary");
                    string media = builder.ToString();
                    byte[] mediaBytes = Encoding.UTF8.GetBytes(media);
                    outputStream.Write(mediaBytes, 0, media.Length);
                    builder.Clear();
                    //file stream:
                    byte[] buffer = new byte[mediaStream.Length];
                    mediaStream.Read(buffer, 0, buffer.Length);
                    outputStream.Write(buffer, 0, buffer.Length);
                    builder.AppendLine().Append("--").Append(Const.BOUNDARY).AppendLine("--");
                    string end = builder.ToString();
                    byte[] endBytes = Encoding.UTF8.GetBytes(end);
                    outputStream.Write(endBytes, 0, end.Length);
                    builder.Clear();
                    //
                    outputStream.Close();
                    // 
                    #endregion
                    requestStreamResult.BeginGetResponse(
                (result) =>
                {
                    #region response
                    HttpWebRequest responseResult = (HttpWebRequest)result.AsyncState;
                    WebResponse response = null;
                    StreamReader streamReader = StreamReader.Null;
                    T output = default(T);
#if !RELEASE
                    response = responseResult.EndGetResponse(result);
                    streamReader = new StreamReader(response.GetResponseStream());
                    string s = streamReader.ReadToEnd();
                    output = JsonConvert.DeserializeObject<T>(s, jsonSettings);
#else
                    try
                    {
                        response = requestResult.EndGetResponse(result);
                        using (streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            using (var reader = new JsonTextReader(streamReader))
                            {
                                output = jsonSerializer.Deserialize<T>(reader);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                    finally
                    {
#endif
                    if (callBack != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(
                            () =>
                            {
                                callBack(output);
                            });
                    }
                    if (streamReader != null)
                    {
                        streamReader.Close();
                        streamReader.Dispose();
                        streamReader = null;
                    }
                    request = null;
                    responseResult = null;
                    if (response != null)
                    {
                        response.Close();
                        response.Dispose();
                        response = null;
                    }
#if RELEASE
                    }
#endif
                    #endregion
                },
              requestStreamResult);
                }, request);
        }


        public void PostNewTweet<T>(ViewModel.NewTweet.Base.NewTweetViewModel newTweet, Action<object> callBack)
        {
            throw new NotImplementedException();
        }


        public void UpdateProfileImage(string base64string)
        {
            //string url = Const.API + "account/update_profile_image.json?image=" + base64string;
            //HandleWebRequest<User>(url,null,Const.HTTPPOST);
        }
    }
}

