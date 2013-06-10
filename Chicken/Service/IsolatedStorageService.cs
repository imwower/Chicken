using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using Chicken.Common;
using Newtonsoft.Json;

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
        static JsonSerializer jsonSerializer = new JsonSerializer();
        #endregion

        #region const
        const string EmotionsFileName = "emotions.json";
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

        public static T GetAndDeleteObject<T>(Const.PageNameEnum pageName)
        {
            string folderName = pageName.ToString();
            return DeserializeObject<T>(folderName + "\\" + folderName, FileOption.DeleteAfterRead);
        }

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
