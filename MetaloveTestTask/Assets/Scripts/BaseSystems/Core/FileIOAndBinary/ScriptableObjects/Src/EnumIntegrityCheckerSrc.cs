using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Scripts.BaseSystems.FileIOAndBinary
{
    [CreateAssetMenu(menuName = "Scriptable Obj/Base systems/Core/File IO and binary/Enum integrity checker tools")]
    class EnumIntegrityCheckerSrc : ScriptableObject
    {

#if UNITY_EDITOR
        [SerializeField]
        private bool _check;
        [SerializeField, Space(30)]
        private AssemblyGroup[] _assemblyGroups;
        [SerializeField, Space(30), Header("Result")]
        private List<UnityEngine.Object> _checkedFiles = new List<UnityEngine.Object>();

        [SerializeField, FilterByType(typeof(IAssetsDirectoryTools)), Space(20)]
        private UnityEngine.Object _directoryToolsObj;
        [SerializeField]
        private IAssetsDirectoryTools _iAssetsDirectoryTools;
        private IAssetsDirectoryTools IAssetsDirectoryTools
        {
            get
            {
                if (_iAssetsDirectoryTools == null)
                    _iAssetsDirectoryTools = _directoryToolsObj.GetComponent<IAssetsDirectoryTools>();

                return _iAssetsDirectoryTools;
            }
        }
#endif

        [Serializable]
        private class AssemblyGroup
        {
            public string _directoryDescriptionName;
            [SerializeField, Space(20)]
            public UnityEngine.Object _assemblyFile;
            public DirectoryGroupDescription[] _directoryDescription;
        }

        [Serializable]
        private class DirectoryGroupDescription
        {
            public string _directoryDescription;
            public UnityEngine.Object _directoryObject;
        }

        private class EnumDescription
        {
            public string _typeName;
            public UnityEngine.Object _filesWithEnum;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_check) _check = false;

            _checkedFiles.Clear();

            if (_assemblyGroups == null) return;

            Dictionary<UnityEngine.Object, bool> checkedFilesDictionary = new Dictionary<UnityEngine.Object, bool>();

            // key   - .cs file with enum
            // value - file .cs namespace  + .cs file name + assembly name;
            Dictionary<UnityEngine.Object, string> enumFilesWithTypeNameDictionary = new Dictionary<UnityEngine.Object, string>();

            FillAssemblyGroupDirectoryDescription(_assemblyGroups);

            foreach (var item in _assemblyGroups)
            {
                item._directoryDescriptionName = item._assemblyFile.name;

                var assetFileArray = GetAllAssetsFromAssemblyGroup(item);

                UnityEngine.Object[] enumToCheckArray = IAssetsDirectoryTools.GetArrayWithUniqueObjects(assetFileArray, AssetObjectTypeId.UnityEditor_MonoScript);

                CompleteDictionaryWithEnumeFileAndTypeName
                    (
                    item._assemblyFile.name,
                    enumToCheckArray,
                    enumFilesWithTypeNameDictionary
                    );
            }

            foreach (var item in enumFilesWithTypeNameDictionary)
            {
                var type = Type.GetType(item.Value);
                var methodInfo = typeof(EnumTool).GetMethod(nameof(EnumTool.CheckEnumIntegrity));
                var genericMethodInfo = methodInfo.MakeGenericMethod(type);
                var result = genericMethodInfo.Invoke(null, new object[] { });
                _checkedFiles.Add(item.Key);
            }
        }
#endif

#if UNITY_EDITOR
        private void FillAssemblyGroupDirectoryDescription(AssemblyGroup[] assemblyGroupArray)
        {
            foreach (var assemblyGroupItem in assemblyGroupArray)
            {
                foreach (var directoryItem in assemblyGroupItem._directoryDescription)
                {
                    var directoryDescription = IAssetsDirectoryTools.GetAssetDatabaseParentDirectoryName(directoryItem._directoryObject);

                    directoryItem._directoryDescription = directoryDescription;
                }
            }
        }

        private UnityEngine.Object[] GetAllAssetsFromAssemblyGroup(AssemblyGroup assemblyGroupArray)
        {
            List<UnityEngine.Object> assetList = new List<UnityEngine.Object>();

            for (int i = 0; i < assemblyGroupArray._directoryDescription.Length; i++)
            {
                var directoryObject = assemblyGroupArray._directoryDescription[i]._directoryObject;
                assetList.Add(directoryObject);
            }

            return assetList.ToArray();
        }

        //  
        private void CompleteDictionaryWithEnumeFileAndTypeName(
            string assemblyName,
            UnityEngine.Object[] assetsToVerifi,
            Dictionary<UnityEngine.Object, string> enumFilesWithTypeDescriptionDictionary
            )
        {
            for (int i = 0; i < assetsToVerifi.Length; i++)
            {
                if (enumFilesWithTypeDescriptionDictionary.ContainsKey(assetsToVerifi[i])) continue;

                var namespaceStr = IAssetsDirectoryTools.GetAssetNamespace(assetsToVerifi[i]);
                var _typeName = namespaceStr + "." + assetsToVerifi[i].name + "," + assemblyName;
                enumFilesWithTypeDescriptionDictionary.Add(assetsToVerifi[i], _typeName);
            }
        }

        private void ClearCollectionFromDuplicates(ref UnityEngine.Object[] collection)
        {
            List<UnityEngine.Object> clearCollection = new List<UnityEngine.Object>();
            Dictionary<string, bool> verifiedTypesDictionary = new Dictionary<string, bool>();

            for (int i = 0; i < collection.Length; i++)
            {
                if (collection[i] == null) continue;

                if (!verifiedTypesDictionary.ContainsKey(collection[i].name))
                {
                    clearCollection.Add(collection[i]);
                    verifiedTypesDictionary.Add(collection[i].name, true);
                }
            }
            collection = clearCollection.ToArray();
        }
#endif
    }

    public class EnumTool
    {
        public static void CheckEnumIntegrity<T>()
        {
            var enumValues = (T[])Enum.GetValues(typeof(T));

            var duplicateValues = enumValues.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();

            if (duplicateValues != null)
                if (duplicateValues.Length > 0)
                {
                    string str = "";
                    str += " ----- \t Check enum: " + typeof(T) + " ----- ";

                    str += "\n Enum PrefabName contains elements with same values.";
                    str += " \n Check elements with values: ";

                    for (int i = 0; i < duplicateValues.Length; i++)
                    {
                        str += "\n " + duplicateValues[i] + "\t";
                    }
                    str += "\n---------------------------------------";
                    Debug.LogError(str);
                }
        }
    }
}
