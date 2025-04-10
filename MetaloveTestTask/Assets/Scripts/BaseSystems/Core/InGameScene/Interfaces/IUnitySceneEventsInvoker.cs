namespace Scripts.BaseSystems.Core
{
    public interface IUnitySceneEventsInvoker 
    {
        public void LoadAsyncUnitySceneByObj(string unitySceneName, bool ignoreIfLoaded = true);
        public void LoadPreviousUnityScene();
        public string GetCurrentSceneName();
        public string GetPreviousUnitySceneName();
        public int? GetUnitySceneStackSize();
    }
}
