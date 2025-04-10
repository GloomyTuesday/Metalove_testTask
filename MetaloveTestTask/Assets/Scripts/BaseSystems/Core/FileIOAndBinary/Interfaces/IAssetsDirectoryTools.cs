using UnityEngine;

namespace Scripts.BaseSystems.FileIOAndBinary
{
    public interface IAssetsDirectoryTools
    {
        public Object GetObjectFromAssetDataPath(string path);

        public Object[] GetAllObjectsFromAssetDataPath(
            string mainPath,
            AssetObjectTypeId objectTypeToLookFor = AssetObjectTypeId.UnityEngine_GameObject
            );

        public Object[] GetArrayWithUniqueObjects(
            Object[] sdirectorySourceForNewObjects,
            AssetObjectTypeId objectTypeToLookFor = AssetObjectTypeId.UnityEngine_GameObject
            );

        public Object[] CompleteArrayWithUniqueObjects(
            Object[] objectArrayToCompleteWith,
            string[] sourceForNewObjects,
            AssetObjectTypeId objectTypeToLookFor = AssetObjectTypeId.UnityEngine_GameObject
            );

        public Object GetObjectFromGuid(string guid);

        public string GetAssetDatabasePath(Object obj);

        public string GetAssetDatabaseParentDirectoryName(Object asset);

        public string GetAssetParentDirectoryName(Object asset);

        public string GetAssetNamespace(Object asset);
    }
}
