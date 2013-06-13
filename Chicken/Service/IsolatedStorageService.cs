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
        static IsolatedStorageFile fileSystem = IsolatedStorageFile.GetUserStoreForApplication();
        static JsonSerializer jsonSerializer = new JsonSerializer
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        #endregion

        #region const
        const string EmotionsFileName = "emotions.json";
        const string DirectMessageFolderPath = "DirectMessages";
        #endregion

        #region public method
        public static bool CreateObject(Const.PageNameEnum pageName, object value)
        {
            string folderName = pageName.ToString();
            if (!fileSystem.DirectoryExists(folderName))
            {
                fileSystem.CreateDirectory(folderName);
            }
            return SerializeObject(folderName + "\\" + folderName, value);
        }

        public static bool CreateObject(Const.PageNameEnum pageName, string fileName, object value)
        {
            string folderName = pageName.ToString();
            if (!fileSystem.DirectoryExists(folderName))
            {
                fileSystem.CreateDirectory(folderName);
            }
            return SerializeObject(folderName + "\\" + fileName, value);
        }

        public static T GetObject<T>(Const.PageNameEnum pageName)
        {
            string folderName = pageName.ToString();
            return DeserializeObject<T>(folderName + "\\" + folderName);
        }

        public static T GetAndDeleteObject<T>(Const.PageNameEnum pageName)
        {
            string folderName = pageName.ToString();
            return DeserializeObject<T>(folderName + "\\" + folderName, FileOption.DeleteAfterRead);
        }

        public static T GetObject<T>(Const.PageNameEnum pageName, string fileName)
        {
            string folderName = pageName.ToString();
            return DeserializeObject<T>(folderName + "\\" + fileName);
        }

        public static T GetAndDeleteObject<T>(Const.PageNameEnum pageName, string fileName)
        {
            string folderName = pageName.ToString();
            return DeserializeObject<T>(folderName + "\\" + fileName, FileOption.DeleteAfterRead);
        }
        #endregion

        #region method for pages
        public static List<string> GetEmotions()
        {
            List<string> result = new List<string>();
            {
                using (var fileStream = fileSystem.OpenFile(EmotionsFileName, FileMode.OpenOrCreate))
                {
                    if (fileStream == null || fileStream.Length == 0)
                    {
                        var resource = Application.GetResourceStream(new Uri(Const.EMOTIONS_FILE_PATH, UriKind.Relative));
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
            if (!fileSystem.DirectoryExists(DirectMessageFolderPath))
            {
                fileSystem.CreateDirectory(DirectMessageFolderPath);
            }
            #endregion
            string filepath = DirectMessageFolderPath + "\\" + con.User.Id;
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
            Conversation conversation = null;
            string filepath = DirectMessageFolderPath + "\\" + userId;
            if (fileSystem.FileExists(filepath))
            {
                using (var fileStream = fileSystem.OpenFile(filepath, FileMode.Open))
                {
                    using (var reader = new JsonTextReader(new StreamReader(fileStream)))
                    {
                        conversation = jsonSerializer.Deserialize<Conversation>(reader);
                    }
                }
            }
            return conversation;
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
            CreateObject(Const.PageNameEnum.MainPage, Const.LATEST_MESSAGES_FILENAME, latestmsgs);
        }

        public static LatestMessagesModel GetLatestMessages()
        {
            return GetObject<LatestMessagesModel>(Const.PageNameEnum.MainPage, Const.LATEST_MESSAGES_FILENAME);
        }

        #endregion

        #region private method
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
