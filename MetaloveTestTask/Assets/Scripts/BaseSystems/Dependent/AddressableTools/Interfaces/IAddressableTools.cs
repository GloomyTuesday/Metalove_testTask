using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Scripts.BaseSystems
{
    public interface IAddressableTools 
    {
        
        public Task<T> GetObjByTask<T>(AssetReference assetRef);
        public void GetObjByCallback<T>(AssetReference assetRef, Action<T> callback);
        public void RemoveByAssetRef(AssetReference assetRef);
        public void RemoveByGameObject<T>(T obj);
        public bool CheckIfExistByAssetRef(AssetReference assetRef);
        public bool CheckIfExistByGameObjectRef<T>(T obj);
        
    }
}
