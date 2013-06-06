using System.IO;
using System.IO.IsolatedStorage;
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

        static IsolatedStorageFile fileSystem = IsolatedStorageFile.GetUserStoreForApplication();
        static JsonSerializer jsonSerializer = new JsonSerializer();
        const string DraftFilePath = "Drafts";
        const string DraftFileName = DraftFilePath + "\\draft.json";

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
    }
}
