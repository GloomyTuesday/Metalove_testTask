using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Scripts.BaseSystems.Core
{
    public class InGameSceneObserver : MonoBehaviour, IReady
    {
        private const int MAX_QUEUE_SIZE = 2;

        [SerializeField]
        [Header("Game object from canvas that will hold all canvas elements from in game scene")]
        [Uneditable]
        private Transform _canvasContentHolder;
        private Transform CanvasContentHolder
        {
            get => _canvasContentHolder; 
            set
            {
                _canvasContentHolder = value;
                UpdateIntegrityStatus();
            }
        }

        [SerializeField]
        [Uneditable]
        [Header("Game object from world space that will hold all the world space objs from in game scene")]
        private Transform _worldContentHolder;
        private Transform WorldContentHolder
        {
            get => _worldContentHolder;
            set
            {
                _worldContentHolder = value;
                UpdateIntegrityStatus(); 
            }
        }

        [SerializeField]
        [Uneditable]
        [Header("Game object for all root objects that are inside the in game scene")]
        private Transform _rootContentHolder;
        private Transform RootContentHolder
        {
            get => _rootContentHolder;
            set
            {
                _rootContentHolder = value;
                UpdateIntegrityStatus();
            }
        }

        [SerializeField]
        [Uneditable]
        [Header("Game object for all root objects that are inside the in game scene")]
        private Transform _instanceContentHolder;
        private Transform InstanceContentHolder
        {
            get => _instanceContentHolder;
            set
            {
                _instanceContentHolder = value;
                UpdateIntegrityStatus();
            }
        }

        [Space(15)]
        [SerializeField, FilterByType(typeof(IInGameSceneContentHolderBuffer))]
        private Object _inGameSceneContentHolderBufferObj;
        [SerializeField, FilterByType(typeof(IInGameSceneEventsHandler))]
        private Object _inGameSceneEventsHandlerObj;

        private GameObject CurrentScenePrefab { get; set; } 

        private IInGameScene CurrentSceneInterface { get; set; }
        private Stack GameSceneStack { get; set; } = new Stack();
        private int StackSize { get; set; }
        private Queue<GameObject> SceneToLoadQueue { get; set; } = new Queue<GameObject>();
        private bool LoadingInProgress { get; set; }

        private IInGameSceneContentHolderBuffer _iInGameSceneContentHolderBuffer;
        private IInGameSceneContentHolderBuffer IInGameSceneContentHolderBuffer
        {
            get
            {
                if (_iInGameSceneContentHolderBuffer == null)
                    _iInGameSceneContentHolderBuffer = _inGameSceneContentHolderBufferObj.GetComponent<IInGameSceneContentHolderBuffer>();

                return _iInGameSceneContentHolderBuffer;
            }
        }

        private IInGameSceneEventsHandler _iInGameSceneEventsHandler;
        private IInGameSceneEventsHandler InGameSceneEventsHandler
        {
            get
            {
                if (_iInGameSceneEventsHandler == null) 
                    _iInGameSceneEventsHandler = _inGameSceneEventsHandlerObj.GetComponent<IInGameSceneEventsHandler>();

                return _iInGameSceneEventsHandler; 
            }
        }

        public bool Ready { get; private set; }

        private void OnEnable()
        {
            CanvasContentHolder = IInGameSceneContentHolderBuffer.CanvasContentHolder;
            WorldContentHolder = IInGameSceneContentHolderBuffer.WorldContentHolder;
            RootContentHolder = IInGameSceneContentHolderBuffer.RootContentHolder;
            InstanceContentHolder = IInGameSceneContentHolderBuffer.InstanceHolderTransform;
            Subscribe();
        }

        private void OnDisable()
        {
            StopAllCoroutines(); 
            Unsubscribe();
        }

        private void Subscribe()
        {
            IInGameSceneContentHolderBuffer.OnCanvasContentHolderUpdated += CanvasContentHolderUpdated;
            IInGameSceneContentHolderBuffer.OnWorldContentHolderUpdated += WorldContentHolderUpdated;
            IInGameSceneContentHolderBuffer.OnRootContentHolderUpdated += RootContentHolderUpdated;
            IInGameSceneContentHolderBuffer.OnInstanceHolderUpdated += InstanceHolderUpdated;

            InGameSceneEventsHandler.OnLoadInGameScene += LoadScene;
            InGameSceneEventsHandler.OnLoadPreviousInGameScene += LoadPreviousInGameScene;
        }

        private void Unsubscribe()
        {
            IInGameSceneContentHolderBuffer.OnCanvasContentHolderUpdated -= CanvasContentHolderUpdated;
            IInGameSceneContentHolderBuffer.OnWorldContentHolderUpdated -= WorldContentHolderUpdated;
            IInGameSceneContentHolderBuffer.OnRootContentHolderUpdated -= RootContentHolderUpdated;
            IInGameSceneContentHolderBuffer.OnInstanceHolderUpdated -= InstanceHolderUpdated;

            InGameSceneEventsHandler.OnLoadInGameScene -= LoadScene;
            InGameSceneEventsHandler.OnLoadPreviousInGameScene -= LoadPreviousInGameScene;
        }

        private void CanvasContentHolderUpdated(Transform transform) => CanvasContentHolder = transform; 
        private void WorldContentHolderUpdated(Transform transform) => WorldContentHolder = transform;
        private void RootContentHolderUpdated(Transform transform) => RootContentHolder = transform;
        private void InstanceHolderUpdated(Transform transform) => InstanceContentHolder = transform;

        private void UpdateIntegrityStatus()
        {
            if(_canvasContentHolder == null)
            {
                Ready = false;
                return;
            }

            if (_worldContentHolder == null)
            {
                Ready = false;
                return;
            }

            if (_rootContentHolder == null)
            {
                Ready = false;
                return;
            }

            if (_instanceContentHolder == null)
            {
                Ready = false;
                return;
            }

            Ready = true;
        }

        private void LoadScene(GameObject scenePrefab)
        {
            Debug.Log("\t - InGameSceneObserver \t LoadScene() \t"+ scenePrefab.name);
            if (SceneToLoadQueue.Count < MAX_QUEUE_SIZE)
                SceneToLoadQueue.Enqueue(scenePrefab);

            if (!LoadingInProgress)
                StartCoroutine(InitializeInGameScene_Co());
        }

        private IEnumerator InitializeInGameScene_Co()
        {
            LoadingInProgress = true;

            while (SceneToLoadQueue.Count > 0)
            {
                if (!Ready)
                {
                    yield return null;
                    continue; 
                }

                if (SceneToLoadQueue.Count < 1) break;

                var scenePrefab = SceneToLoadQueue.Dequeue();

                if (CurrentSceneInterface != null)
                    CurrentSceneInterface.DestroyScene();

                if (CurrentScenePrefab != null)
                    GameSceneStack.Push(CurrentScenePrefab);

                var inGameSceneGameObject = Instantiate(scenePrefab, IInGameSceneContentHolderBuffer.InstanceHolderTransform);
                
                CurrentSceneInterface = inGameSceneGameObject.GetComponent<IInGameScene>();
                CurrentScenePrefab = scenePrefab;

                StackSize++;

                CurrentSceneInterface.DistributeContent(_canvasContentHolder, _worldContentHolder, _rootContentHolder);
                yield return null; 
            }

            LoadingInProgress = false;
        }

        private void LoadPreviousInGameScene()
        {
            Debug.Log("\t - InGameSceneObserver \t LoadPreviousInGameScene() ");
            if (StackSize < 1) return;

            StackSize--;
            var inGameScenePrefab = (GameObject)GameSceneStack.Pop();
            LoadScene(inGameScenePrefab);
        }


    }
}
