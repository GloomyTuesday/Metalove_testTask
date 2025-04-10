using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Scripts.BaseSystems.FileIOAndBinary
{
    [CreateAssetMenu(fileName = "FileIOCsvTools", menuName = "Scriptable Obj/Base systems/Core/File IO and binary/File .CSV input output tools")]
    public class FileIOCsvToolsSrc : ScriptableObject, IFileIOCsvTools
    {
        private const string FILE_EXTENTION = ".csv";

        public void DeleteCsvFromPath(string directory, string filename)
        {
            string path = Path.Combine(directory, filename, FILE_EXTENTION);

            if (File.Exists(path))
            {
                File.Delete(path);
                return;
            }
            Debug.Log("\t Path doesn 't exist: " + path + "\t file name: " + filename);
        }

        public void DeleteCsvFromPersistentDataPath(string directory, string filename)
        {
            DeleteCsvFromPath(Path.Combine(Application.persistentDataPath, directory), filename); 
        }

        public Task<string[][]> LoadCsv(string directory, string filename)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string path = Path.Combine(directory, filename);

            return LoadCsvAsync(path);
        }

        public Task<string[][]> LoadCsv(string fullFilePathWithName)
        {
            return LoadCsvAsync(fullFilePathWithName);
        }

        public async Task<string[][]> LoadCsvAsync(string fullFilePathWithName)
        { 
            var pathDataend = fullFilePathWithName.Substring(fullFilePathWithName.Length - 3);
            string path;

            if (pathDataend == FILE_EXTENTION)
                path = Path.GetFullPath(fullFilePathWithName); 
            else
                path = Path.Combine(fullFilePathWithName, FILE_EXTENTION);


            List<List<string>> lists = new List<List<string>>();
            bool endOfFile = false;

            using (var streamReader = new StreamReader(path))
            {
                while (!endOfFile)
                {
                    var dataString = await streamReader.ReadLineAsync();

                    if (dataString == null)
                    {
                        endOfFile = true;
                        break;
                    }

                    var dataValues = dataString.Split(',');

                    lists.Add(new List<string>(dataValues));
                }
            }

            var stringsCollection = new string[lists.Count][];

            for (int i = 0; i < lists.Count; i++)
            {
                stringsCollection[i] = lists[i].ToArray();
            }

            return stringsCollection;
        }

        public Task<string[][]> LoadCsvFromPersistentDataPath(string directory, string filename)
        {
            return LoadCsvAsync( Path.Combine(Application.persistentDataPath,directory, filename)) ;
        }

        public Task<string[][]> LoadCsvFromPersistentDataPath(string filePathWithName)
        {
            var path = Path.Combine(Application.persistentDataPath, filePathWithName);
            return LoadCsvAsync(path);
        }

        public bool SaveCsvInPersistentDataPath(string directory, string filename, string[][] dataTable)
        {
            var modifiedDirectoryPath = Path.Combine(Application.persistentDataPath, directory);
            return SaveCsv(modifiedDirectoryPath, filename, dataTable); 
        }

        public bool SaveCsv(string directory, string filename, string[][] dataTable)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string path;
            path = Path.Combine(directory, filename, FILE_EXTENTION);

            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    for (int i = 0; i < dataTable.Length; i++)
                    {
                        string line = "";

                        for (int j = 0; j < dataTable[i].Length; j++)
                            line = string.Join(",", dataTable[i][j]);

                        writer.WriteLine(line);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Could not save .csv file {filename} : {ex.Message}");
                return false;
            }
        }

    }
}
