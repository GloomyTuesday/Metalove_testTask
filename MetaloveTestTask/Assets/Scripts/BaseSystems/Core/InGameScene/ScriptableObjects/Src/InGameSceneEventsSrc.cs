using UnityEngine;
using System;

namespace Scripts.BaseSystems.Core
{
    [CreateAssetMenu(fileName = "InGameSceneEvents", menuName = "Scriptable Obj/Base systems/Core/In game scene/In game scene events")]
    public class InGameSceneEventsSrc : ScriptableObject, IInGameSceneEvents, IInGameSceneEventsHandler
    {
        /*
        private Action<AssetReference> onLoadInGameSceneAssetRef;
        event Action<AssetReference> InGameSceneEventsHandler.OnLoadInGameSceneAssetRef
        {
            add => onLoadInGameSceneAssetRef = value;
            remove => onLoadInGameSceneAssetRef = value;
        }
        public void LoadInGameSceneAssetRef(AssetReference assetRef) => onLoadInGameSceneAssetRef?.Invoke(assetRef);
        */

        private Action<GameObject> onLoadInGameScene; 
        event Action<GameObject> IInGameSceneEventsHandler.OnLoadInGameScene
        {
            add => onLoadInGameScene = value; 
            remove => onLoadInGameScene = value;
        }
        public void LoadInGameScene(GameObject prefab)
        {
            Debug.Log("\t - InGameSceneEventsSrc \t LoadInGameScene \t inGameSceneName: "+ prefab.name);
            onLoadInGameScene?.Invoke(prefab);
        }


        private Action onLoadPreviousInGameScene;
        event Action IInGameSceneEventsHandler.OnLoadPreviousInGameScene
        {
            add => onLoadPreviousInGameScene = value;
            remove => onLoadPreviousInGameScene = value; 
        }
        public void LoadPreviousInGameScene() => onLoadPreviousInGameScene?.Invoke(); 


    }
}