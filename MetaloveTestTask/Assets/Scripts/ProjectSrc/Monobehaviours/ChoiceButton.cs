using Scripts.BaseSystems.UiRelated;
using Scripts.BaseSystems;
using TMPro;
using UnityEngine;

namespace Scripts.ProjectSrc
{
    public class ChoiceButton : MonoBehaviour, IChoiceButton
    {
        [SerializeField]
        private TextMeshProUGUI[] _textMeshPro; 

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IScenarioFrameEvents))]
        private Object _scenarioFrameEventsObj;

        [SerializeField]
        [FilterByType(typeof(ICollectionRegister<IRectTransformAligner>))]
        private Object _collectionRegisterObj;

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

        private string NextScenarioFrameId { get; set; }


        public void SetNextScenarioFrameToLoadId(string nextScenarioFrameToLoadId)
        {
            NextScenarioFrameId = nextScenarioFrameToLoadId;
        }

        public void SetText(string text)
        {
            if (_textMeshPro == null) return;
            if (string.IsNullOrEmpty(text)) return;

            for (int i = 0; i < _textMeshPro.Length; i++)
                _textMeshPro[i].text = text;
        }

        public void TryToLoadNextScenarioFrame()
        {
            IScenarioFrameEvents.LoadScenarioFrame(NextScenarioFrameId);
        }
    }
}
