using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using Chicken.Common;
using Chicken.Model;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chicken.Service
{
    public class IsolatedStorageService
    {
        enum FileOption
        {
            OnlyRead,
            DeleteAfterRead,
        }

        #region private static
        private static IsolatedStorageFile fileSystem = IsolatedStorageFile.GetUserStoreForApplication();
        private static JsonSerializer jsonSerializer = new JsonSerializer
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        private static IDictionary<string, object> state = PhoneApplicationService.Current.State;
        #endregion

        #region const
        private const string SAVED_DATA_FOLDER_PATH = "Data";

        private const string STATE_FILE_NAME = "State.json";

        private const string EMOTIONS_FILE_NAME = "Emotions.json";

        private const string LATEST_MESSAGES_FILE_NAME = "LatestDirectMessages.json";
        private const string DIRECT_MESSAGES_FOLDERPATH = "DirectMessages";

        private const string AUTHENTICATED_USER_FILE_NAME = "AuthenticatedUser.json";

        private const string GENERALSETTINGS_FILE_NAME = "GeneralSettings.json";

        private const string ABOUT_FILE_NAME = "About.json";

        private const string TWEET_CONFIGURATION_FILE_NAME = "TweetConfiguration.json";

        private const string HOME_TWEETS = "HomeTweets.json";
        private const string HOME_MENTIONS = "HomeMentions.json";
        #endregion

        #region method for pages
        public static List<string> GetEmotions()
        {
            List<string> result = new List<string>();
            CheckDataFolderPath();
            string filepath = SAVED_DATA_FOLDER_PATH + "/" + EMOTIONS_FILE_NAME;
            using (var fileStream = fileSystem.OpenFile(filepath, FileMode.OpenOrCreate))
            {
                if (fileStream == null || fileStream.Length == 0)
                {
                    var resource = Application.GetResourceStream(new Uri(filepath, UriKind.Relative));
                    resource.Stream.CopyTo(fileStream);
                }
                using (var streamReader = new StreamReader(fileStream))
                {
                    fileStream.Position = 0;
                    while (!streamReader.EndOfStream)
                    {
                        string s = streamReader.ReadLine();
                        if (!string.IsNullOrEmpty(s))
                        {
                            result.Add(s.Trim());
                        }
                    }
                }
            }
            return result;
        }

        public static void AddMessages(Conversation conversation)
        {
            #region init
            var con = new Conversation
            {
                User = conversation.Messages[0].User,
            };
            foreach (var m in conversation.Messages)
            {
                con.Messages.Add(new DirectMessage
                {
                    IsSentByMe = m.IsSentByMe,
                    Id = m.Id,
                    Text = m.Text,
                    CreatedDate = m.CreatedDate,
                    Entities = m.Entities,
                });
            }
            #endregion
            #region create folder
            if (!fileSystem.DirectoryExists(DIRECT_MESSAGES_FOLDERPATH))
                fileSystem.CreateDirectory(DIRECT_MESSAGES_FOLDERPATH);
            #endregion
            string filepath = DIRECT_MESSAGES_FOLDERPATH + "\\" + con.User.Id + ".json";
            #region directly serialize
            if (!fileSystem.FileExists(filepath))
            {
                using (var fileStream = fileSystem.OpenFile(filepath, FileMode.Create))
                {
                    {
                        using (TextWriter writer = new StreamWriter(fileStream))
                        {
                            jsonSerializer.Serialize(writer, con);
                        }
                    }
                }
            }
            #endregion
            else
            {
                #region serialize object
                JObject jobject = null;
                string tempfilepath = filepath + "____";
                using (var fileStream = fileSystem.OpenFile(filepath, FileMode.Open))
                {
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        using (var reader = new JsonTextReader(streamReader))
                        {
                            jobject = (JObject)JToken.ReadFrom(reader);
                            jobject["User"] = JObject.FromObject(con.User, jsonSerializer);

                            foreach (var msg in con.Messages)
                            {
                                jobject["Messages"].Last.AddAfterSelf(JObject.FromObject(msg, jsonSerializer));
                            }
                        }
                    }
                }
                #endregion
                #region write to temp file
                using (var outStream = fileSystem.OpenFile(tempfilepath, FileMode.Create))
                {
                    using (var jsonWriter = new JsonTextWriter(new StreamWriter(outStream)))
                    {
                        jobject.WriteTo(jsonWriter);
                    }
                }
                #endregion
                #region rename
                fileSystem.DeleteFile(filepath);
                fileSystem.MoveFile(tempfilepath, filepath);
                #endregion
            }
        }

        public static Conversation GetMessages(string userId)
        {
            string filepath = DIRECT_MESSAGES_FOLDERPATH + "\\" + userId + ".json";
            return DeserializeObject<Conversation>(filepath, FileOption.OnlyRead);
        }

        public static void AddLatestMessages(LatestMessagesModel latestMessages)
        {
            #region init
            var latestmsgs = new LatestMessagesModel
            {
                MaxId = latestMessages.MaxId,
                SinceId = latestMessages.SinceId,
                MaxIdByMe = latestMessages.MaxIdByMe,
                SinceIdByMe = latestMessages.SinceIdByMe,
            };
            foreach (var msg in latestMessages.Messages)
            {
                latestmsgs.Messages.Add(msg.Key, new DirectMessage
                {
                    IsSentByMe = msg.Value.IsSentByMe,
                    Id = msg.Value.Id,
                    Text = msg.Value.Text,
                    Entities = msg.Value.Entities,
                    CreatedDate = msg.Value.CreatedDate,
                    User = msg.Value.User
                });
            }
            #endregion
            #region create folder
            if (!fileSystem.DirectoryExists(DIRECT_MESSAGES_FOLDERPATH))
                fileSystem.CreateDirectory(DIRECT_MESSAGES_FOLDERPATH);
            #endregion
            string filepath = DIRECT_MESSAGES_FOLDERPATH + "\\" + LATEST_MESSAGES_FILE_NAME;
            SerializeObject(filepath, latestmsgs);
        }

        public static LatestMessagesModel GetLatestMessages()
        {
            string filepath = DIRECT_MESSAGES_FOLDERPATH + "\\" + LATEST_MESSAGES_FILE_NAME;
            return DeserializeObject<LatestMessagesModel>(filepath, FileOption.OnlyRead);
        }

        public static void CreateAuthenticatedUser(UserProfileDetail authenticatedUser)
        {
            CheckDataFolderPathAndSerializeOjbect(AUTHENTICATED_USER_FILE_NAME, authenticatedUser);
        }

        public static UserProfileDetail GetAuthenticatedUser()
        {
            string filepath = SAVED_DATA_FOLDER_PATH + "\\" + AUTHENTICATED_USER_FILE_NAME;
            return DeserializeObject<UserProfileDetail>(filepath, FileOption.OnlyRead);
        }

        public static void CreateAppSettings(GeneralSettings settings)
        {
            CheckDataFolderPathAndSerializeOjbect(GENERALSETTINGS_FILE_NAME, settings);
        }

        public static GeneralSettings GetAppSettings()
        {
            string filepath = SAVED_DATA_FOLDER_PATH + "\\" + GENERALSETTINGS_FILE_NAME;
            return DeserializeObject<GeneralSettings>(filepath, FileOption.OnlyRead);
        }
        
        public static void CreateTweetConfiguration(TweetConfiguration configuration)
        {
            CheckDataFolderPathAndSerializeOjbect(TWEET_CONFIGURATION_FILE_NAME, configuration);
        }

        public static TweetConfiguration GetTweetConfiguration()
        {
            string filepath = SAVED_DATA_FOLDER_PATH + "\\" + TWEET_CONFIGURATION_FILE_NAME;
            return DeserializeObject<TweetConfiguration>(filepath, FileOption.OnlyRead);
        }

        public static TweetList GetHomeTweets()
        {
            string filepath = SAVED_DATA_FOLDER_PATH + "\\" + HOME_TWEETS;
            return DeserializeObject<TweetList>(filepath);
        }

        /// <summary>
        /// let me do the hard work
        /// </summary>
        /// <param name="tweets"></param>
        public static void AddHomeTweets(TweetList tweets)
        {
            var list = GetHomeTweets();
            if (list == null || list.Count == 0)
                list = new TweetList();
            for (int i = tweets.Count - 1; i >= 0; i--)
            {
                list.Insert(0, tweets[i]);
                if (list.Count >= Const.DEFAULT_COUNT_VALUE)
                    list.RemoveAt(list.Count - 1);
            }
            CheckDataFolderPathAndSerializeOjbect(HOME_TWEETS, list);
        }

        public static TweetList GetMentions()
        {
            string filepath = SAVED_DATA_FOLDER_PATH + "\\" + HOME_MENTIONS;
            return DeserializeObject<TweetList>(filepath);
        }

        /// <summary>
        /// let me do the hard work
        /// </summary>
        /// <param name="tweets"></param>
        public static void AddMentions(TweetList tweets)
        {
            var list = GetMentions();
            if (list == null || list.Count == 0)
                list = new TweetList();
            for (int i = tweets.Count - 1; i >= 0; i--)
            {
                list.Insert(0, tweets[i]);
                if (list.Count >= Const.DEFAULT_COUNT_VALUE)
                    list.RemoveAt(list.Count - 1);
            }
            CheckDataFolderPathAndSerializeOjbect(HOME_MENTIONS, list);
        }
        #endregion

        #region public method
        public static void CreateObject(string key, object value)
        {
            state[key] = value;
        }

        public static T GetObject<T>(string key)
        {
            if (state.ContainsKey(key))
                return (T)state[key];
            return default(T);
        }

        public static T GetAndDeleteObject<T>(string key)
        {
            if (state.ContainsKey(key))
            {
                object value = state[key];
                state.Remove(key);
                return (T)value;
            }
            return default(T);
        }
        #endregion

        #region private method
        private static void CheckDataFolderPath()
        {
            if (!fileSystem.DirectoryExists(SAVED_DATA_FOLDER_PATH))
                fileSystem.CreateDirectory(SAVED_DATA_FOLDER_PATH);
        }

        private static void CheckDataFolderPathAndSerializeOjbect(string filename, object data)
        {
            CheckDataFolderPath();
            string filepath = SAVED_DATA_FOLDER_PATH + "\\" + filename;
            SerializeObject(filepath, data);
        }

        private static bool SerializeObject(string fileName, object value)
        {
            if (fileSystem.FileExists(fileName))
                fileSystem.DeleteFile(fileName);
            using (IsolatedStorageFileStream fileStream = fileSystem.OpenFile(fileName, FileMode.Create))
            {
                using (TextWriter writer = new StreamWriter(fileStream))
                {
                    jsonSerializer.Serialize(writer, value);
                    writer.Flush();
                }
            }
            return true;
        }

        private static T DeserializeObject<T>(string fileName, FileOption option = FileOption.OnlyRead)
        {
            var result = default(T);
            if (fileSystem.FileExists(fileName))
            {
                using (IsolatedStorageFileStream fileStream = fileSystem.OpenFile(fileName, FileMode.Open))
                {
                    using (JsonTextReader reader = new JsonTextReader(new StreamReader(fileStream)))
                    {
                        result = jsonSerializer.Deserialize<T>(reader);
                    }
                }
                if (option == FileOption.DeleteAfterRead)
                {
                    fileSystem.DeleteFile(fileName);
                }
            }
            return result;
        }
        #endregion
    }
}
