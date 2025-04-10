using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Scripts.BaseSystems.FileIOAndBinary
{
    [CreateAssetMenu(fileName = "DirectoryTools", menuName = "Scriptable Obj/Base systems/Core/File IO and binary/Directory tools")]
    public class DirectoryToolsSrc : 
        ScriptableObject,
        IDirectoryTools, 
        IAssetsDirectoryTools
    {

        [SerializeField, FilterByType(typeof(IThreadTools))]
        private Object _threadToolsOj;

        private IThreadTools _iThreadTools; 
        private IThreadTools IThreadTools
        {
            get
            {
                if (_iThreadTools == null)
                    _iThreadTools = _threadToolsOj.GetComponent<IThreadTools>();

                return _iThreadTools; 
            }
        }

        #region Methods that can be used only in Unity editor

#if UNITY_EDITOR
        private IAssetsDirectoryTools _iAssetsDirectoryTools;
        private IAssetsDirectoryTools IAssetsDirectoryTools
        {
            get
            {
                if (_iAssetsDirectoryTools == null)
                    _iAssetsDirectoryTools = this;

                return _iAssetsDirectoryTools;
            }
        }
#endif

        /// <summary>
        /// Can be used only in Unity editor 
        /// </summary>
        string IAssetsDirectoryTools.GetAssetDatabaseParentDirectoryName(Object asset)
        {
            var splitResult = new string[0];
#if UNITY_EDITOR
            string[] separatingStrings = { @"\", @"/" };
            var assetPath = IAssetsDirectoryTools.GetAssetDatabasePath(asset);
            splitResult = assetPath.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

            if (splitResult.Length > 1)
                return splitResult[^2];
#endif
            return "";
        }

        /// <summary>
        /// Can be used only in Unity editor  
        /// </summary>
        string IAssetsDirectoryTools.GetAssetParentDirectoryName(Object asset)
        {
            var splitResult = new string[0];
#if UNITY_EDITOR
            string[] separatingStrings = { @"\", @"/" };

            Debug.Log(" ");
            Debug.Log(" ");
            for (int i = 0; i < separatingStrings.Length; i++)
            {
                Debug.Log("\t\t\t [ " + i + " ] " + separatingStrings[i]);
            }

            var assetPath = IAssetsDirectoryTools.GetAssetDatabasePath(asset);
            splitResult = assetPath.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

            if (splitResult.Length > 1)
                return splitResult[^2];
#endif
            return "";
        }

        /// <summary>
        /// Can be used only in Unity editor 
        /// </summary>
        string IAssetsDirectoryTools.GetAssetNamespace(Object asset)
        {
#if UNITY_EDITOR
            var assetPath = IAssetsDirectoryTools.GetAssetDatabasePath(asset);
            var textLines = System.IO.File.ReadAllLines(assetPath);
            var namespaceStr = "namespace";

            for (int i = 0; i < textLines.Length; i++)
            {
                var splitResult = textLines[i].Split(' ');

                if (splitResult.Length != 2) continue;
                if (splitResult[0] != namespaceStr) continue;

                return splitResult[1];
            }

            Debug.LogWarning("\t Couldn't find namespace for asset: " + asset.name + "\nRow with namespace should contain only keyword namespace and it's name ");
#endif
            return "";
        }

        /// <summary>
        /// Canbe used only in Unity editor
        /// </summary>
        string IAssetsDirectoryTools.GetAssetDatabasePath(Object obj)
        {
            string path = "";
#if UNITY_EDITOR
            path = AssetDatabase.GetAssetPath(obj);
#endif
            return path;
        }

        Object IAssetsDirectoryTools.GetObjectFromAssetDataPath(string path)
        {
#if UNITY_EDITOR
            #region Checking for and cutting "Assets/" from the path
            string assetsWord = "Assets/";
            string supposedAssetWord = path.Remove(assetsWord.Length, path.Length - assetsWord.Length);

            if (supposedAssetWord == assetsWord)
                path = path.Remove(0, assetsWord.Length);
            #endregion

            var directoryToVerify = Application.dataPath + "/" + path;
            directoryToVerify = directoryToVerify.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            directoryToVerify = Path.GetDirectoryName(directoryToVerify);

            if (System.IO.Directory.Exists(directoryToVerify))
            {
                var pathCompleted = "Assets/" + path;
                pathCompleted = pathCompleted.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

                var assetGuid = AssetDatabase.AssetPathToGUID(pathCompleted);

                if (string.IsNullOrWhiteSpace(assetGuid)) return null;

                var obj = IAssetsDirectoryTools.GetObjectFromGuid(assetGuid);
                return obj;
            }

            Debug.LogError("Directory: " + directoryToVerify + "\t doesn't exist.");
#endif
            return null;
        }

        /// <summary>
        /// Canbe used only in Unity editor
        /// </summary>
        Object[] IAssetsDirectoryTools.GetAllObjectsFromAssetDataPath(
            string mainPath,
            AssetObjectTypeId objectTypeToLookFor
            )
        {
            List<Object> objectList = new List<Object>();
#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(mainPath))
            {
                #region Checking for and cutting "Assets/" from the path
                string assetsWord = "Assets/";
                string supposedAssetWord = mainPath.Remove(assetsWord.Length, mainPath.Length - assetsWord.Length);

                if (supposedAssetWord == assetsWord)
                    mainPath = mainPath.Remove(0, assetsWord.Length);
                #endregion

                var pathToVerify = Application.dataPath + "/" + mainPath;
                pathToVerify = pathToVerify.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

                if (System.IO.Directory.Exists(pathToVerify))
                {
                    var filter = "t:Object";
                    var pathCompleted = "Assets/" + mainPath;
                    pathCompleted = pathCompleted.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

                    var guidArray = AssetDatabase.FindAssets(filter, new[] { pathCompleted });

                    foreach (var guidItem in guidArray)
                    {
                        var element = IAssetsDirectoryTools.GetObjectFromGuid(guidItem);
                        if (element != null)
                        {
                            switch (objectTypeToLookFor)
                            {
                                case AssetObjectTypeId.UnityEngine_GameObject:
                                    {
                                        if (element.GetType() == typeof(GameObject))
                                            objectList.Add(element);
                                    }
                                    break;

                                case AssetObjectTypeId.UnityEditor_MonoScript:
                                    {
                                        if (element.GetType() == typeof(MonoScript))
                                            objectList.Add(element);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
#endif
            return objectList.ToArray();
        }

        /// <summary>
        /// Canbe used only in Unity editor
        /// </summary>
        Object[] IAssetsDirectoryTools.GetArrayWithUniqueObjects(
            Object[] sdirectorySourceForNewObjects,
            AssetObjectTypeId objectTypeToLookFor
            )
        {
            var list = new List<Object>();
#if UNITY_EDITOR
            if (sdirectorySourceForNewObjects == null) return list.ToArray();
            if (sdirectorySourceForNewObjects.Length < 1) return list.ToArray();

            List<string> pathList = new List<string>();

            for (int i = 0; i < sdirectorySourceForNewObjects.Length; i++)
                pathList.Add(AssetDatabase.GetAssetPath(sdirectorySourceForNewObjects[i]));

            var objArray = IAssetsDirectoryTools.CompleteArrayWithUniqueObjects(list.ToArray(), pathList.ToArray(), objectTypeToLookFor);

            if (objArray == null || objArray == null) return list.ToArray();

            list.AddRange(objArray);
#endif
            return list.ToArray();
        }

        /// <summary>
        /// Canbe used only in Unity editor
        /// </summary>
        Object[] IAssetsDirectoryTools.CompleteArrayWithUniqueObjects(
            Object[] objectArrayToCompleteWith,
            string[] sourceForNewObjects,
            AssetObjectTypeId objectTypeToLookFor
            )
        {
            if (sourceForNewObjects == null) return objectArrayToCompleteWith;

            List<Object> objectList = new List<Object>();
#if UNITY_EDITOR
            if (objectArrayToCompleteWith != null)
                objectList.AddRange(objectArrayToCompleteWith);

            foreach (var pathItem in sourceForNewObjects)
            {
                if (pathItem.Length < 1) continue;

                var objectCollections = IAssetsDirectoryTools.GetAllObjectsFromAssetDataPath(pathItem, objectTypeToLookFor);

                if (objectCollections == null || objectCollections == null) continue;

                foreach (var objectItem in objectCollections)
                {
                    if (!objectList.Contains(objectItem))
                    {
                        objectList.Add(objectItem);
                    }
                }
            }
#endif
            return objectList.ToArray();
        }

        /// <summary>
        /// Canbe used only in Unity editor
        /// </summary>
        Object IAssetsDirectoryTools.GetObjectFromGuid(string guid)
        {
            Object element = null;
#if UNITY_EDITOR
            element = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(UnityEngine.Object));
#endif
            return element;
        }

        #endregion

        async Task<string[]> IDirectoryTools.GetAllDirectoriesAndSubdirectories(string directoryToCheck)
        {
            if (directoryToCheck == default || directoryToCheck.Length < 1)
                Debug.Log("\t - DirectoryToolsSrc \t GetAllDirectoryes() \t directory is EMPTY");

            return await Task.Run(() => Directory.GetDirectories(directoryToCheck)).ConfigureAwaitAuto();
        }

        async Task<string[]> IDirectoryTools.GetAllFilePathsFromDirectories(string directories, string fileFormat)
        {
            List<string> files = new List<string>();
            if (!Directory.Exists(directories)) return files.ToArray();

            var filesArray = await Task.Run(() => System.IO.Directory.GetFiles(directories));

            foreach (var item in filesArray)
            {
                if (CheckFilePathForFormat(item, filesArray))
                    files.Add(item);
            }

            return files.ToArray();
        }

        async Task<string[]> IDirectoryTools.GetAllFilePathsFromDirectories(string[] directories, string fileFormat)
        {
            List<string> filePaths = new List<string>();

            foreach (var item in directories)
            {
                if (!Directory.Exists(item)) continue;

                var files = await Task.Run(() => System.IO.Directory.GetFiles(item));
                List<string> filteredFileList = new List<string>();

                for (int i = 0; i < files.Length; i++)
                {
                    if (CheckFilePathForFormat(files[i], fileFormat))
                        filteredFileList.Add(files[i]);
                }

                filePaths.AddRange(filteredFileList);
            }

            return filePaths.ToArray();
        }

        async Task<string[]> IDirectoryTools.GetAllFilePathsFromDirectory(string directory, string fileFormat)
        {
            if (!Directory.Exists(directory)) return new string[0];

            var files = await Task.Run(() => Directory.GetFiles(directory));
            List<string> filteredFileList = new List<string>();

            for (int i = 0; i < files.Length; i++)
            {
                if (CheckFilePathForFormat(files[i], fileFormat))
                    filteredFileList.Add(files[i]);
            }

            return filteredFileList.ToArray(); 
        }

        async Task<string[]> IDirectoryTools.GetAllFilePathsFromDirectory(string directory, string[] fileFormat)
        {
            if (!Directory.Exists(directory)) return new string[0];

            var files = await Task.Run(() => Directory.GetFiles(directory));

            List<string> filteredFileList = new List<string>();

            for (int i = 0; i < files.Length; i++)
            {
                if(CheckFilePathForFormat(files[i],fileFormat))
                    filteredFileList.Add(files[i]);
            }

            return filteredFileList.ToArray();
        }


        private bool CheckFilePathForFormat(string file, string[] format)
        {
            foreach (var item in format)
                if (CheckFilePathForFormat(file, item))
                    return true;

            return false; 
        }

        private bool CheckFilePathForFormat(string file, string format)
        {
            if (format==null || format == "") return true;

            if (Path.GetExtension(file).Equals(format, System.StringComparison.OrdinalIgnoreCase))
                return true;

            return false; 
        }
    }

}
