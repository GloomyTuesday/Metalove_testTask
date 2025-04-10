using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System;

namespace Scripts.BaseSystems.FileIOAndBinary
{
    [CreateAssetMenu(menuName = "Scriptable Obj/Base systems/Core/File IO and binary/File byinput input and output tools")]
    public class FileIOToolsSrc : ScriptableObject, IFileIOTools
    {
        [SerializeField, FilterByType(typeof(ISerializationTools))]
        private UnityEngine.Object _serializationToolsObj;

        private ISerializationTools _iSerializationTools;
        private ISerializationTools ISerializationTools
        {
            get
            {
                if (_iSerializationTools == null)
                    _iSerializationTools = _serializationToolsObj.GetComponent<ISerializationTools>();

                return _iSerializationTools;
            }
        }

        public bool SaveBinaryInPersistentDataPath<T>(string directory, string filenameWithExtension, T content )
        {
            string path;
            path = Path.Combine(Application.persistentDataPath, directory);
            return SaveBinary(path, filenameWithExtension, content);
        }

        public bool SaveBinary<T>(string fullPath, T content)
        {
            string directory = Path.GetDirectoryName(fullPath);

            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            var byteArray = ISerializationTools.Serialize(content);

            try
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    Debug.Log("\t -FileIOToolsSrc \t Saving binary data to: " + fullPath + " with byte array size: " + byteArray.Length);

                    formatter.Serialize(stream, byteArray);
                    stream.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error saving binary file: " + e.Message);
                return false;
            }
        }

        public Task<T> LoadBinaryFromPersistentDataPath<T>(string directory, string filenameWithExtension) where T : class
        {
            var path = Path.Combine(Application.persistentDataPath, directory, filenameWithExtension);
            return LoadBinary<T>(path);
        }

        public Task<T> LoadBinaryFromPersistentDataPath<T>(string fullFilePathWithNameAndExtension) where T : class
        {
            var path = Path.Combine(Application.persistentDataPath, fullFilePathWithNameAndExtension);
            return LoadBinary<T>(path); 
        }

        public bool SaveBinary<T>(string directory, string filenameWithExtension, T content)
        {
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            string path;
            path = Path.Combine(directory, filenameWithExtension);

            object obj = content;
            var byteArray = ISerializationTools.Serialize(obj);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formater = new BinaryFormatter();

                Debug.Log("\t -FileIOToolsSrc \t Saveing byte array size: " + byteArray.Length);

                formater.Serialize(stream, byteArray);
                stream.Close();
            }
            return true;
        }


        public Task<T> LoadBinary<T>(string directory, string filenameWithExtension) 
            where T : class
        {
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            string path = Path.Combine(directory, filenameWithExtension);

            return LoadBinary<T>(path); 
        }

        public async Task<T> LoadBinary<T>(string filePath)
            where T : class
        {
            filePath = Path.GetFullPath(filePath);

            var awaitable = Task.Run(
             () =>
             {
                 T content = default;

                 if (System.IO.File.Exists(filePath))
                 {
                     BinaryFormatter formatter = new BinaryFormatter();

                     using (FileStream stream = new FileStream(filePath, FileMode.Open))
                     {
                         if (stream.Length > 0)
                         {
                             var container = (byte[])formatter.Deserialize(stream);
                             content = ISerializationTools.Deserialize<T>(container);

                         }
                     }
                 }

                 return content;
             }
            );

            return await awaitable.ConfigureAwaitAuto();
        }

        public async Task<bool> SaveToJsonInPersistentDataPath<T>(T content, string directory, string filename, string extension, bool prettyPrint = false)
        {
            var path = Path.Combine(Application.persistentDataPath, directory, filename+extension);
            return await SaveToJson(content, path , prettyPrint); 
        }

        public async Task<bool> SaveToJson<T>(T content, string directory, string filename, string extension, bool prettyPrint = false)
        {
            try
            {
                var path = Path.Combine(directory, filename+extension);
                return await SaveToJson(content , path, prettyPrint); 
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error saving object to JSON file: " + e.Message);
            }

            return false; 
        }

        public async Task<bool> SaveToJson<T>(T content, string path, bool prettyPrint = false)
        {
            try
            {
                Debug.Log("\t Save json: "+ path); 
                string json = JsonUtility.ToJson(content, prettyPrint);
                await Task.Run(() => System.IO.File.WriteAllText(path, json));
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error saving object to JSON file: " + e.Message);
            }

            return false;
        }

        public Task<T> LoadJSonFromPersistentDataPath<T>(string directory, string filename, string extension ) where T : class
        {
            var path = Path.Combine(Application.persistentDataPath, directory, filename+extension);

            return LoadJSon<T>(path);
        }

        public Task<T> LoadJSon<T>(string directory, string filename, string extension) where T : class
        {
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            string path;
            path = Path.Combine(directory, filename+extension);

            return LoadJSon<T>(path); 
        }

        public async Task<T> LoadJSon<T>(string fullFilePathWithExtension) where T : class
        {
            if (!System.IO.File.Exists(fullFilePathWithExtension))
            {
                Debug.LogWarning("There is no file with path: "+ fullFilePathWithExtension);
                return null; 
            }

            try
            {
                string json = await Task.Run(() => System.IO.File.ReadAllText(fullFilePathWithExtension)).ConfigureAwaitAuto();
                T data = JsonUtility.FromJson<T>(json);
                return data;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error loading JSON file: " + e.Message);
                return null;
            }
        }

        public void DeleteFileFromPersistentDataPath(string directory, string filenameWithExtension)
        {
            DeleteFileFromPath(Path.Combine(Application.persistentDataPath, directory), filenameWithExtension);
        }

        public void DeleteFileFromPath(string directory, string filenameWithExtension)
        {
            string path = Path.Combine(directory, filenameWithExtension);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return; 
            }

            Debug.Log("\t Path doesn't exist: "+ path+"\t file name: "+ filenameWithExtension);
        }

    }
}
