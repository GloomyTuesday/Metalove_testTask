using System;

namespace Scripts.BaseSystems.Core
{
    internal interface IUnitySceneEventsHandler 
    {
        public event Action<string,bool> OnLoadAsyncUnitySceneByObj;
        public event Action OnLoadPreviousUnityScene;
        public event Func<string> OnGetCurrentSceneName;
        public event Func<string> OnGetPreviousUnitySceneName;
        public event Func<int> OnGetUnitySceneStackSize;
    }
}
