using UnityEngine;
using System; 

namespace Scripts.BaseSystems.Core
{
    [CreateAssetMenu(fileName = "UnitySceneEvents", menuName = "Scriptable Obj/Base systems/Core/In game scene/Unity scene events")]
    public class UnitySceneEventsSrc : ScriptableObject, IUnitySceneEventsInvoker, IUnitySceneEventsHandler
    {
        private Action<string,bool> onLoadAsyncUnitySceneByObj;
        event Action<string,bool> IUnitySceneEventsHandler.OnLoadAsyncUnitySceneByObj
        {
            add => onLoadAsyncUnitySceneByObj = value;
            remove => onLoadAsyncUnitySceneByObj = value;
        }
        void IUnitySceneEventsInvoker.LoadAsyncUnitySceneByObj(string sceneObj, bool reloadIfSceneIsAlreadyLoaded)=> 
            onLoadAsyncUnitySceneByObj?.Invoke(sceneObj, reloadIfSceneIsAlreadyLoaded);


        private Action onLoadPreviousUnityScene;
        event Action IUnitySceneEventsHandler.OnLoadPreviousUnityScene
        {
            add => onLoadPreviousUnityScene = value;
            remove => onLoadPreviousUnityScene = value;
        }
        void IUnitySceneEventsInvoker.LoadPreviousUnityScene() => onLoadPreviousUnityScene?.Invoke();


        private Func<string> onGetCurrentSceneName;
        event Func<string> IUnitySceneEventsHandler.OnGetCurrentSceneName
        {
            add => onGetCurrentSceneName = value;
            remove => onGetCurrentSceneName = value;
        }
        string IUnitySceneEventsInvoker.GetCurrentSceneName() => onGetCurrentSceneName?.Invoke();


        private Func<string> onGetPreviousUnitySceneName;
        event Func<string> IUnitySceneEventsHandler.OnGetPreviousUnitySceneName
        {
            add => onGetPreviousUnitySceneName = value;
            remove => onGetPreviousUnitySceneName = value;
        }
        string IUnitySceneEventsInvoker.GetPreviousUnitySceneName() => onGetPreviousUnitySceneName?.Invoke();


        private Func<int> onGetUnitySceneStackSize;
        event Func<int> IUnitySceneEventsHandler.OnGetUnitySceneStackSize
        {
            add => onGetUnitySceneStackSize = value;
            remove => onGetUnitySceneStackSize = value;
        }
        int? IUnitySceneEventsInvoker.GetUnitySceneStackSize() => onGetUnitySceneStackSize?.Invoke();
    }
}
