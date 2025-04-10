using Scripts.BaseSystems;
using Scripts.BaseSystems.UiRelated;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.ProjectSrc
{
    public class ScenarioFrameChoice : MonoBehaviour
    {
        [SerializeField]
        private GameObject _scrollableBtnPrefab;

        [SerializeField]
        private RectTransform _scrollContent;

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IScenarioFrameEvents))]
        private UnityEngine.Object _scenarioFrameEventsObj;

        [SerializeField]
        [FilterByType(typeof(ICollectionRegister<IRectTransformAligner>))]
        private UnityEngine.Object _collectionRegisterObj;

        private ICollectionRegister<IRectTransformAligner> _icollectionRegisterObj;
        private ICollectionRegister<IRectTransformAligner> ICollectionRegisterObj
        {
            get
            {
                if (_icollectionRegisterObj == null)
                    _icollectionRegisterObj = _collectionRegisterObj.GetComponent<ICollectionRegister<IRectTransformAligner>>();

                return _icollectionRegisterObj;
            }
        }

        [SerializeField]
        [Uneditable]
        private List<GameObject> _createdObjects;
        private List<GameObject> CreatedObjects { get=> _createdObjects; set => _createdObjects = value; }

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

        private void OnEnable()
        {
            CreatedObjects = new List<GameObject>(); 
            Subscribe(); 
        }

        private void OnDisable()
        {
            Unsubscribe(); 
        }

        private void Subscribe()
        {
       //     IScenarioFrameEvents.OnLoadPreviousScenarioFrameId += LoadPreviousScenarioFrameId;
            IScenarioFrameEvents.OnApplyChoices += ApplyChoices;
        }
        
        private void Unsubscribe()
        {
      //      IScenarioFrameEvents.OnLoadPreviousScenarioFrameId -= LoadPreviousScenarioFrameId;
            IScenarioFrameEvents.OnApplyChoices -= ApplyChoices;
        }

        private void LoadScenarioFrame(string obj)
        {
            DestroyAllCeatedObjects();
        }

        private void ApplyChoices(string[] choicesText, string[] choicesNextFrameId )
        {
            DestroyAllCeatedObjects();

            for ( int i = 0; i < choicesText.Length; i++ )
            {
                AddItem(_scrollableBtnPrefab, choicesText[i], choicesNextFrameId[i]);
            }

            CallAliggnerWithDelay(); 

            LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollContent);
        }

        private void CallAliggnerWithDelay()
        {
            var alligners = ICollectionRegisterObj.GetRegistereItems();

            if (alligners == null) return;

            for (int i = 0; i < alligners.Length; i++)
                alligners[i].AlignWithDelay(2);
        }

        public void AddItem(GameObject prefab, string text, string choicesNextFrameId)
        {
            GameObject newItem = Instantiate(prefab, _scrollContent);

            var iChoiceButton = newItem.GetComponent<IChoiceButton>();
            if(iChoiceButton!=null)
            {
                iChoiceButton.SetText(text);
                iChoiceButton.SetNextScenarioFrameToLoadId(choicesNextFrameId);
            }

            CreatedObjects.Add(newItem); 
        }

        private void DestroyAllCeatedObjects()
        {
            for (int i = CreatedObjects.Count - 1; i >= 0 ; i--)
                Destroy(CreatedObjects[i]);

            CreatedObjects.Clear();
        }
    }
}
