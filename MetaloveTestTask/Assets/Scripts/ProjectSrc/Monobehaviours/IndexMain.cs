using Scripts.BaseSystems;
using Scripts.BaseSystems.Core;
using UnityEngine;

namespace Scripts.ProjectSrc
{
    public class IndexMain : MonoBehaviour
    {

        [SerializeField]
        [Uneditable]
        private string _unitySceneToLoadName = "";

        [SerializeField]
        private UnityEngine.Object _unitySceneToLoad;

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IUnitySceneEventsInvoker))]
        private UnityEngine.Object _unitySceneEventsInvokerObj;

        private IUnitySceneEventsInvoker _iUnitySceneEventsInvoker;
        private IUnitySceneEventsInvoker IUnitySceneEventsInvoker
        {
            get
            {
                if (_iUnitySceneEventsInvoker == null)
                    _iUnitySceneEventsInvoker = _unitySceneEventsInvokerObj.GetComponent<IUnitySceneEventsInvoker>();

                return _iUnitySceneEventsInvoker; 
            }
        }

        private void OnValidate()
        {
            if (_unitySceneToLoad != null)
                _unitySceneToLoadName = _unitySceneToLoad.name; 
        }

        private void OnEnable()
        {
            if(_unitySceneToLoadName!=null && _unitySceneToLoadName.Length > 0 )
                IUnitySceneEventsInvoker.LoadAsyncUnitySceneByObj(_unitySceneToLoadName);
        }
    }
}
