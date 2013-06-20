using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using Chicken.Common;
using Chicken.Model;
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
        #endregion

        #region const
        private const string SAVED_DATA_FOLDER_PATH = "Data";
        private const string EMOTIONS_FILE_NAME = "Emotions.json";

        private const string LATEST_MESSAGES_FILE_NAME = "LatestDirectMessages.json";
        private const string DIRECT_MESSAGES_FOLDERPATH = "DirectMessages";

        private const string AUTHENTICATED_USER_FILE_NAME = "AuthenticatedUser.json";

        private const string GENERALSETTINGS_FILE_NAME = "GeneralSettings.json";
        #endregion

        #region method for pages
        public static List<string> GetEmotions()
        {
            List<string> result = new List<string>();
            CheckDataFolderPath();
            string emotionsfilepath = SAVED_DATA_FOLDER_PATH + "/" + EMOTIONS_FILE_NAME;
            using (var fileStream = fileSystem.OpenFile(emotionsfilepath, FileMode.OpenOrCreate))
            {
                if (fileStream == null || fileStream.Length == 0)
                {
                    var resource = Application.GetResourceStream(new Uri(emotionsfilepath, UriKind.Relative));
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
            {
                fileSystem.CreateDirectory(DIRECT_MESSAGES_FOLDERPATH);
            }
            #endregion
            string filepath = DIRECT_MESSAGES_FOLDERPATH + "\\" + con.User.Id + ".json";
            if (!fileSystem.FileExists(filepath))
            {
                #region directly serialize
                using (var fileStream = fileSystem.OpenFile(filepath, FileMode.Create))
                {
                    {
                        using (TextWriter writer = new StreamWriter(fileStream))
                        {
                            jsonSerializer.Serialize(writer, con);
                        }
                    }
                }
                #endregion
            }
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
            con = null;
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
            {
                fileSystem.CreateDirectory(DIRECT_MESSAGES_FOLDERPATH);
            }
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
            CheckDataFolderPath();
            string filepath = SAVED_DATA_FOLDER_PATH + "/" + AUTHENTICATED_USER_FILE_NAME;
            SerializeObject(filepath, authenticatedUser);
        }

        public static UserProfileDetail GetAuthenticatedUser()
        {
            string filepath = SAVED_DATA_FOLDER_PATH + "/" + AUTHENTICATED_USER_FILE_NAME;
            return DeserializeObject<UserProfileDetail>(filepath, FileOption.OnlyRead);
        }

        public static void CreateAppSettings(GeneralSettings settings)
        {
            CheckDataFolderPath();
            string filepath = SAVED_DATA_FOLDER_PATH + "/" + GENERALSETTINGS_FILE_NAME;
            SerializeObject(filepath, settings);
        }

        public static GeneralSettings GetAppSettings()
        {
            string filepath = SAVED_DATA_FOLDER_PATH + "/" + GENERALSETTINGS_FILE_NAME;
            return DeserializeObject<GeneralSettings>(filepath, FileOption.OnlyRead);
        }

        #endregion

        #region public method
        public static bool CreateObject(PageNameEnum pageName, object value)
        {
            string folderName = pageName.ToString();
            if (!fileSystem.DirectoryExists(folderName))
            {
                fileSystem.CreateDirectory(folderName);
            }
            return SerializeObject(folderName + "\\" + folderName, value);
        }

        public static T GetObject<T>(PageNameEnum pageName)
        {
            string folderName = pageName.ToString();
            return DeserializeObject<T>(folderName + "\\" + folderName);
        }

        public static T GetAndDeleteObject<T>(PageNameEnum pageName)
        {
            string folderName = pageName.ToString();
            return DeserializeObject<T>(folderName + "\\" + folderName, FileOption.DeleteAfterRead);
        }
        #endregion

        #region private method
        private static void CheckDataFolderPath()
        {
            if (!fileSystem.DirectoryExists(SAVED_DATA_FOLDER_PATH))
            {
                fileSystem.CreateDirectory(SAVED_DATA_FOLDER_PATH);
            }
        }

        private static bool SerializeObject(string fileName, object value)
        {
            if (fileSystem.FileExists(fileName))
            {
                fileSystem.DeleteFile(fileName);
            }
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
