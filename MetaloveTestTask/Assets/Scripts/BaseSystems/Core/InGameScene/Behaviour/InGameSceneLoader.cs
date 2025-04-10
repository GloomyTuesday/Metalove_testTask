using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    public class InGameSceneLoader : MonoBehaviour, IReady
    {
        [SerializeField]
        [FilterByType(typeof(IBankType<GameObject>))]
        private UnityEngine.Object _sceneBankObj;

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IInGameSceneEvents))]
        private UnityEngine.Object _inGameSceneEventsObj;

        private bool _ready;
        public bool Ready => _ready;

        private IBankType<GameObject> _iBank;
        private IBankType<GameObject> IBank
        {
            get
            {
                if (_iBank == null)
                    _iBank = _sceneBankObj.GetComponent<IBankType<GameObject>>();

                return _iBank;
            }
        }

        private IInGameSceneEvents _inGameSceneEvents;
        private IInGameSceneEvents InGameSceneEvents
        {
            get
            {
                if (_inGameSceneEvents == null)
                    _inGameSceneEvents = _inGameSceneEventsObj.GetComponent<IInGameSceneEvents>();

                return _inGameSceneEvents;
            }
        }

        private void OnEnable()
        {
            var sceneCollection = IBank.GetItemArray();

            if (sceneCollection == null || sceneCollection.Length < 1) return;
            if (sceneCollection[0] == null) return; 

            InGameSceneEvents.LoadInGameScene(sceneCollection[0]);
            _ready = true;
        }
    }
}
