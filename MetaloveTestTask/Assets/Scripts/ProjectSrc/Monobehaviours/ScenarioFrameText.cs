using Scripts.BaseSystems;
using Scripts.BaseSystems.UiRelated;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.ProjectSrc
{
    public class ScenarioFrameText : MonoBehaviour
    {
        [Space(15)]
        [SerializeField]
        private bool _treatAsScreenSizePercentage; 

        [SerializeField]
        private Vector3 _rectPositionOffset;

        [Space(10)]
        [SerializeField]
        private Vector2Int _rectMinSizeOffset;

        [SerializeField]
        private Vector2Int _rectMaxSizeOffset;

        [SerializeField] 
        private TextMeshProUGUI[] _text;

        [SerializeField] 
        private RectTransform[] _rectTransform;

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IScenarioFrameEvents))]
        private UnityEngine.Object _scenarioFrameEventsObj;

        [SerializeField]
        [FilterByType(typeof(ICollectionRegister<IRectTransformAligner>))]
        private Object _collectionRegisterObj;

        private Vector3 PositionOffsetAdapted;

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
            if (_treatAsScreenSizePercentage)
            {
                PositionOffsetAdapted = new Vector3(
                    Screen.width * _rectPositionOffset.x/100,
                    Screen.height * _rectPositionOffset.y/100,
                    0
                    ); 
            }
            else
            {
                PositionOffsetAdapted = _rectPositionOffset;
            }

            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe(); 
        }

        private void Subscribe()
        {
            IScenarioFrameEvents.OnApplyText += ApplyText;
        }

        private void Unsubscribe()
        {
            IScenarioFrameEvents.OnApplyText -= ApplyText;
        }

        private void CallAliggnerWithDelay()
        {
            var alligners = ICollectionRegisterObj.GetRegistereItems();

            if (alligners == null) return;

            for (int i = 0; i < alligners.Length; i++)
                alligners[i].AlignNextFrame();
        }

        private void ApplyText(string textString)
        {
            if (_text == null || _text.Length < 1) return;

            for (int i = 0; i < _text.Length; i ++ )
                _text[i].text = textString;

            Vector2 preferredValues = _rectMinSizeOffset;

            preferredValues = _text[0].GetPreferredValues(textString, Screen.width - _rectMaxSizeOffset.x, float.PositiveInfinity - _rectMaxSizeOffset.y) + _rectMinSizeOffset;

            for (int i = 0; i < _rectTransform.Length; i++)
            {
                _rectTransform[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredValues.x);
                _rectTransform[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredValues.y);

           //     _rectTransform[i].localPosition = _rectTransform[i].localPosition + PositionOffsetAdapted;

           //     LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform[i]);
            }

            CallAliggnerWithDelay(); 
        }

        public void TryToLaunchNextScenarioFrame()
        {
            var nextScenarioFrame = IScenarioFrameEvents.GetNextScenarioFrameId();

            if (string.IsNullOrEmpty(nextScenarioFrame)) return; 

            IScenarioFrameEvents.LoadScenarioFrame(nextScenarioFrame); 
        }
    }
}
