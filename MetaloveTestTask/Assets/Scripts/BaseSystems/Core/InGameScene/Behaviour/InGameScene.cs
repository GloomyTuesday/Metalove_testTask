/*
 
    All objects that are not child or Roo, World or Canvas objcts are going to be destroyd
 
*/
using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    public class InGameScene : MonoBehaviour, IInGameScene
    {
        //  To make things related to game scene more cleare and easy to find
        #region Constants
        const string _canvasPrefix = "-- ";
        const string _canvasSuffix = " -- canvas";

        const string _spacePrefix = "-- ";
        const string _spaceSuffix = " -- world";

        const string _rootPrefix = "-- ";
        const string _rootSuffix = " -- root";
        #endregion

        [Header("Contains script that destroys all gameObjects that are not siblings of: root, world or canvas"),Space(10)]
        [SerializeField]
        private GameObject _inGameScenePrefab;
        [SerializeField, Space(20)]
        private Transform _canvasContentHolder;
        [SerializeField]
        private Transform _worldContentHolder;
        [SerializeField]
        private Transform _rootContentHolder;

        public GameObject InGameScenePrefab => _inGameScenePrefab;

        public Transform CanvasContentHolder => _canvasContentHolder;
        public Transform SpaceContentHolder => _worldContentHolder;
        public Transform RootContentHolder => _rootContentHolder;
        public string InGameSceneName { get; protected set; }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_inGameScenePrefab != null)
                InGameSceneName = _inGameScenePrefab.name;
            else
                InGameSceneName = gameObject.name;

            if (_canvasContentHolder != null)
                _canvasContentHolder.name = _canvasPrefix + InGameSceneName + _canvasSuffix;

            if (SpaceContentHolder != null)
                SpaceContentHolder.name = _spacePrefix + InGameSceneName + _spaceSuffix;

            if (RootContentHolder != null)
                RootContentHolder.name = _rootPrefix + InGameSceneName + _rootSuffix;
        }
#endif

        private void OnEnable()
        {
            DestoyUnnecessaryObj(); 
        }

        public void DistributeContent(Transform canvasContentHolder, Transform worldContentHolder, Transform rootContentHolder)
        {
            RectTransform rectTransform = _canvasContentHolder.GetComponent<RectTransform>();

            _canvasContentHolder.transform.SetParent(canvasContentHolder.transform);
            _worldContentHolder.transform.SetParent(worldContentHolder.transform);
            _rootContentHolder.transform.SetParent(rootContentHolder.transform);

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = new Vector2(.5f, .5f);
            rectTransform.localPosition = Vector3.zero;
            rectTransform.sizeDelta = Vector3.zero;
            rectTransform.localScale = Vector3.one;
        }

        public void DestroyScene()
        {
            Destroy(_canvasContentHolder.gameObject);
            Destroy(_worldContentHolder.gameObject);
            Destroy(_rootContentHolder.gameObject);
            Destroy(gameObject);
        }

        private void DestoyUnnecessaryObj()
        {
            var childCount = transform.childCount;
            for (int i = childCount-1; i > 0; i--)
            {
                if (transform.GetChild(i) == _rootContentHolder) continue;
                if (transform.GetChild(i) == _worldContentHolder) continue;
                if (transform.GetChild(i) == _canvasContentHolder.transform.parent.gameObject) continue;

                Destroy(transform.GetChild(i).gameObject); 
            }
        }
    }
}
