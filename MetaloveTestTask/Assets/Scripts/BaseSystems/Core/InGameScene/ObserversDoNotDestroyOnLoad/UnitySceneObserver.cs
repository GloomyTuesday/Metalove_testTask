using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;

#if UNITY_EDITOR
  using UnityEditor;
#endif

namespace Scripts.BaseSystems.Core
{
    public class UnitySceneObserver : MonoBehaviour, IReady
    {
        internal static UnitySceneObserver _instance;

        [SerializeField]
        [Uneditable]
        private bool _isFirstSceneLoaded;
        private bool IsFirstSceneLoaded { get => _isFirstSceneLoaded; set => _isFirstSceneLoaded = value; }

        [Space(15)]
        [SerializeField, FilterByType(typeof(IUnitySceneEventsHandler))]
        private Object _unitySceneEventsObj;

        public bool Ready { get; private set; }

        private Stack UnitySceneStack { get; set; } = new Stack();
        private int UnitySceneStackSize { get; set; }

        private string _currentUnityScene; 
        private string CurrentUnitySceneName
        {
            get
            {
                if (_currentUnityScene == null || _currentUnityScene == default)
                    _currentUnityScene = SceneManager.GetActiveScene().name; 

                return _currentUnityScene; 
            }

            set
            {
                _currentUnityScene = value;
            }
        }

        private IUnitySceneEventsHandler _iUnitySceneEventsHandler; 
        private IUnitySceneEventsHandler IUnitySceneEventsHandler
        {
            get
            {
                if (_iUnitySceneEventsHandler == null)
                    _iUnitySceneEventsHandler = _unitySceneEventsObj.GetComponent<IUnitySceneEventsHandler>();

                return _iUnitySceneEventsHandler; 
            }
        }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return; 
            }

            _instance = this;

            if (transform.parent != null)
            {
                transform.SetParent(null);
                transform.SetSiblingIndex(0);
            }

            DontDestroyOnLoad(gameObject);

            Subscribe();

            this.Ready = true; 
        }

        private void OnEnable()
        {
            if (IsFirstSceneLoaded) return; 

            if(UnitySceneStack.Count == 0)
            {
                var firstLoadedScene = SceneManager.GetActiveScene().name;

#if UNITY_EDITOR
                var FirstSceneFromSceneListPath = EditorBuildSettings.scenes[0].path;
                var FirstSceneFromSceneList = Path.GetFileNameWithoutExtension(FirstSceneFromSceneListPath);
                if (!firstLoadedScene.Equals(FirstSceneFromSceneList))
                    LoadAsyncUnitySceneByObj(FirstSceneFromSceneList);                
#else
                UnitySceneStack.Push(firstLoadedScene);
                UnitySceneStackSize++; 
#endif
                IsFirstSceneLoaded = true;
            }
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            IUnitySceneEventsHandler.OnLoadAsyncUnitySceneByObj += LoadAsyncUnitySceneByObj;
            IUnitySceneEventsHandler.OnLoadPreviousUnityScene += LoadPreviousUnityScene;
            IUnitySceneEventsHandler.OnGetCurrentSceneName += GetCurrentSceneName;
            IUnitySceneEventsHandler.OnGetPreviousUnitySceneName += GetPreviousUnitySceneName;
            IUnitySceneEventsHandler.OnGetUnitySceneStackSize += GetUnitySceneStackSize;
        }

        private void Unsubscribe()
        {
            IUnitySceneEventsHandler.OnLoadAsyncUnitySceneByObj -= LoadAsyncUnitySceneByObj;
            IUnitySceneEventsHandler.OnLoadPreviousUnityScene -= LoadPreviousUnityScene;
            IUnitySceneEventsHandler.OnGetCurrentSceneName -= GetCurrentSceneName;
            IUnitySceneEventsHandler.OnGetPreviousUnitySceneName -= GetPreviousUnitySceneName;
            IUnitySceneEventsHandler.OnGetUnitySceneStackSize -= GetUnitySceneStackSize;
        }

        private void LoadAsyncUnitySceneByObj(string unitySceneName, bool reloadIfSceneIsAlreadyLoaded = false)
        {
            if (!reloadIfSceneIsAlreadyLoaded)
                if (unitySceneName == CurrentUnitySceneName) return; 
            
            if(CurrentUnitySceneName != null)
            {
                UnitySceneStack.Push(CurrentUnitySceneName);
                UnitySceneStackSize++; 
            }
            CurrentUnitySceneName = unitySceneName;
            SceneManager.LoadSceneAsync(unitySceneName); 
        }

        private void LoadPreviousUnityScene()
        {
            var previousSceneObj = UnitySceneStack.Pop() as string;

            if (previousSceneObj == null)
            {
                Debug.Log("\t There's no more scene in stack.");
                return; 
            }

            UnitySceneStackSize--; 
            SceneManager.LoadSceneAsync(previousSceneObj);
        }

        private string GetCurrentSceneName() => CurrentUnitySceneName;

        private string GetPreviousUnitySceneName()
        {
            var previousUnitySceneObj = UnitySceneStack.Peek() as string;

            if (previousUnitySceneObj != null)
                return previousUnitySceneObj;

            return null;
        }

        private int GetUnitySceneStackSize() => UnitySceneStackSize;
    }
}
