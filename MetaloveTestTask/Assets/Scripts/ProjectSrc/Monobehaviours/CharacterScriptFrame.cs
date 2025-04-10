using Scripts.BaseSystems;
using System;
using UnityEngine;

namespace Scripts.ProjectSrc
{
    public class CharacterScriptFrame : MonoBehaviour
    {
        [SerializeField]
        private CharacterAlignmentData[] _alignmentData;

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IBankTypeId<string, GameObject>))]
        private UnityEngine.Object _characterBankObj;

        private IBankTypeId<string, GameObject> _characterBank;
        private IBankTypeId<string, GameObject> CharacterBank
        {
            get
            {
                if(_characterBank == null)
                    _characterBank = _characterBankObj.GetComponent<IBankTypeId<string, GameObject>>();

                return _characterBank;
            }
        }

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IScenarioFrameEvents))]
        private UnityEngine.Object _scenarioFrameEventsObj;

        private GameObject _characterGameObj; 

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

        private void OnValidate()
        {
            if(_alignmentData!=null)
            {
                foreach (var item in _alignmentData)
                    item._name = item._alignmentId.ToString(); 
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
            IScenarioFrameEvents.OnApplyCharacter += ApplyCharacter;
        }

        private void Unsubscribe()
        {
            IScenarioFrameEvents.OnApplyCharacter -= ApplyCharacter;
        }

        private void ApplyCharacter(string characterId, AlignmentId alignmentId)
        {
            if (_characterGameObj != null)
                Destroy(_characterGameObj);

            if (string.IsNullOrEmpty(characterId)) return; 

            var characterPrefab = CharacterBank.GetItem(characterId);

            if (characterPrefab == null) return;

            Transform transform = default;

            if(_alignmentData!=null && _alignmentData.Length > 0)
            {
                transform = _alignmentData[0]._transform; 

                for (int i = 1; i < _alignmentData.Length; i++)
                {
                    if (_alignmentData[i]._alignmentId == alignmentId)
                    {
                        transform = _alignmentData[i]._transform;
                        break;
                    }
                }
            }

            _characterGameObj = Instantiate(characterPrefab); 

            if(transform!=null)
                _characterGameObj.transform.SetParent(transform);

            _characterGameObj.transform.localPosition = Vector3.zero;
            _characterGameObj.transform.localRotation = Quaternion.identity;
            _characterGameObj.transform.localScale = Vector3.one;

        }

        [Serializable]
        private class CharacterAlignmentData
        {
            [HideInInspector]
            public string _name; 

            public AlignmentId _alignmentId;
            public Transform _transform; 
        }
    }
}
