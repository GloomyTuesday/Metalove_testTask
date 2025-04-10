using Scripts.BaseSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.ProjectSrc
{
    public class BtnNextScenarioFrame:MonoBehaviour
    {
        [SerializeField]
        private Vector3 _pressedLocalScale; 


        [Space(15)]
        [SerializeField]
        private Image _raycastTargetObj; 

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(ICollectionRegister<RectTransform>))]
        private Object _rectTransformCollectionObj;

        [SerializeField]
        [FilterByType(typeof(IScenarioFrameEvents))]
        private Object _scenarioFrameEventsObj;

        private Vector3 PositionOffsetAdapted;
        private string NextScenarioFrameToLoadId { get; set; }

        private IScenarioFrameEvents _iScenarioFrameEvents;
        private IScenarioFrameEvents IScenarioFrameEvents
        {
            get
            {
                if (_iScenarioFrameEvents == null)
                    _iScenarioFrameEvents = _scenarioFrameEventsObj.GetComponent<IScenarioFrameEvents>();

                return _iScenarioFrameEvents;
            }
        }

        private ICollectionRegister<RectTransform> _iCollectionRegister;
        private ICollectionRegister<RectTransform> ICollectionRegister
        {
            get
            {
                if(_iCollectionRegister == null)
                    _iCollectionRegister = _rectTransformCollectionObj.GetComponent<ICollectionRegister<RectTransform>>();

                return _iCollectionRegister;
            }
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            IScenarioFrameEvents.OnApplyNextScenarioFrameToLoad += ApplyNextScenarioFrameToLoad;
        }

        private void Unsubscribe()
        {
            IScenarioFrameEvents.OnApplyNextScenarioFrameToLoad -= ApplyNextScenarioFrameToLoad;
        }

        private void ApplyNextScenarioFrameToLoad(string nextScenarioFrameToLoadId)
        {
            _raycastTargetObj.raycastTarget = true;

            if (string.IsNullOrEmpty(nextScenarioFrameToLoadId))
                _raycastTargetObj.raycastTarget = false;                 

            NextScenarioFrameToLoadId = nextScenarioFrameToLoadId;
        }

        public void TryToLaunchNextFrame()
        {
            if (string.IsNullOrEmpty(NextScenarioFrameToLoadId)) return;

            IScenarioFrameEvents.LoadScenarioFrame(NextScenarioFrameToLoadId); 
        }

        private Dictionary<int, Vector3> _initScaleDictionary; 

        public void Down()
        {
            _initScaleDictionary = new Dictionary<int, Vector3>(); 
            var dictionaryCollection = ICollectionRegister.InstanceIdItemIndexDictionary;

            foreach (var item in dictionaryCollection)
            {
                Debug.Log(""+item.Value.name);

                _initScaleDictionary.Add(item.Key, item.Value.localScale);
                item.Value.localScale = new Vector3(
                    item.Value.localScale.x * _pressedLocalScale.x ,
                    item.Value.localScale.y * _pressedLocalScale.y ,
                    item.Value.localScale.z * _pressedLocalScale.z
                    );
            }

        }

        public void Up()
        {
            if (_initScaleDictionary == null)
            {
                Debug.LogWarning("Down method was not called.");
                return;
            }

            var dictionaryCollection = ICollectionRegister.InstanceIdItemIndexDictionary;

            foreach (var item in dictionaryCollection)
            {
                if (!_initScaleDictionary.ContainsKey(item.Key)) continue;

                item.Value.localScale = _initScaleDictionary[item.Key];
            }

        }
    }
}
