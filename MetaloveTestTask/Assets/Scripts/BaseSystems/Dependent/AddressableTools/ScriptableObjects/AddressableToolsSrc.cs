using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

namespace Scripts.BaseSystems
{
    [CreateAssetMenu(menuName = "Scriptable Obj/Base systems/Addressables tools")]
    public class AddressableToolsSrc : ScriptableObject, IAddressableTools
    {

        [SerializeField, FilterByType(typeof(IThreadTools))]
        private UnityEngine.Object _threadToolsObj;

        [SerializeField]
        private AssetReference _assetReference;

        private List<GameObject> GameObjectList { get; set; } = new List<GameObject>();
        private Dictionary<AssetReference, AssetUnit> AssetRefUnityDictionary { get; set; } = new Dictionary<AssetReference, AssetUnit>();
        private List<AssetUnit> AssetUnitList { get; set; } = new List<AssetUnit>();

        private IThreadTools _iThreadTools;
        private IThreadTools IThreadTools
        {
            get
            {
                if (_iThreadTools == null)
                    _iThreadTools = _threadToolsObj.GetComponent<IThreadTools>();

                return _iThreadTools;
            }
        }

        private struct AssetUnit
        {
            public object Obj { get; }
            public string AssetGuid { get; }
            public AssetReference AssetReference { get; }

            public AssetUnit(AssetReference assetRef, object obj)
            {
                AssetReference = assetRef;
                AssetGuid = assetRef.AssetGUID;
                Obj = obj;
            }
        }

        public void Load()
        {
            GetObjByCallback<GameObject>(
                _assetReference,
                (prefab) =>
                {
                    GameObjectList.Add(Instantiate(prefab));
                    GameObjectList[^1].name = "Obj " + GameObjectList.Count;
                }
            );
        }

        public void Release()
        {
            for (int i = GameObjectList.Count - 1; i >= 0; i--)
            {
                Destroy(GameObjectList[i]);
                GameObjectList.RemoveAt(i);
            }

            RemoveByAssetRef(_assetReference);
        }

        public bool CheckIfExistByAssetRef(AssetReference assetRef) => AssetRefUnityDictionary.ContainsKey(assetRef);

        public async void GetObjByCallback<T>(AssetReference assetRef, Action<T> callback = null)
        {
            if (AssetRefUnityDictionary.ContainsKey(assetRef))
            {
                callback?.Invoke((T)AssetRefUnityDictionary[assetRef].Obj);
                return;
            }

            var asyncOperation = Addressables.LoadAssetAsync<T>(assetRef);

            await asyncOperation.Task;

            var result = asyncOperation.Task.Result;

            callback?.Invoke(result);

            AddAssetUnit(assetRef, result);
        }

        private void AssetLoaded(AsyncOperationHandle<object> handle)
        {
            throw new NotImplementedException();
        }

        public Task<UnityEngine.Object> GetObjByTask(AssetReference assetRef)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveByAssetRef(AssetReference assetRef)
        {
            if (!AssetRefUnityDictionary.ContainsKey(assetRef)) return;

            RemoveAssetRef(assetRef);
        }

        public Task<T> GetObjByTask<T>(AssetReference assetRef)
        {
            throw new NotImplementedException();
        }

        public void RemoveByGameObject<T>(T obj)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfExistByGameObjectRef<T>(T obj)
        {
            throw new NotImplementedException();
        }

        private void AddAssetUnit(AssetReference assetRef, object obj)
        {
            if ((AssetRefUnityDictionary.ContainsKey(assetRef))) return;

            var assetUnit = new AssetUnit(assetRef, obj);

            AssetRefUnityDictionary.Add(assetRef, assetUnit);
            AssetUnitList.Add(assetUnit);
        }

        private void RemoveAssetRef(AssetReference assetRef)
        {
            if (!(AssetRefUnityDictionary.ContainsKey(assetRef))) return;

            var itemToRemove = AssetRefUnityDictionary[assetRef];
            AssetRefUnityDictionary.Remove(assetRef);
            AssetUnitList.Remove(itemToRemove);
        }
    }
}
