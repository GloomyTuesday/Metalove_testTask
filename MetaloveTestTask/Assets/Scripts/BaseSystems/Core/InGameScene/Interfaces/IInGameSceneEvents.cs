using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    public interface IInGameSceneEvents
    {
     //   public void LoadInGameSceneAssetRef(AssetReference assetRef);
        public void LoadInGameScene(GameObject prefab);
        public void LoadPreviousInGameScene();
    }
}
