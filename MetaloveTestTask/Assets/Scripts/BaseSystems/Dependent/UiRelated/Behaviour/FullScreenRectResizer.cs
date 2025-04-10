using UnityEngine;

namespace Scripts.BaseSystems
{
    public class FullScreenRectResizer : MonoBehaviour
    {
        [SerializeField]
        private bool _scale;

        [SerializeField,HideInInspector]
        private Canvas _canvas; 
        private Canvas Canvas
        {
            get
            {
                if(_canvas == null)
                    _canvas = GetComponentInParent<Canvas>();

                return _canvas; 
            }
        }

        private RectTransform _mainRect;
        private RectTransform MainRect
        {
            get
            {
                if (_mainRect == null)
                    _mainRect = GetComponent<RectTransform>();

                return _mainRect;
            }
        }

        private void OnValidate()
        {
            _canvas = GetComponentInParent<Canvas>();
            _mainRect = GetComponent<RectTransform>();

            if(_scale)
            {
                _scale = false;
                Scale(); 
            }
        }

        private void OnEnable()
        {
            Scale();
        }

        private void Scale()
        {
            var anchorMinBuff = MainRect.anchorMin;
            var anchorMaxBuff = MainRect.anchorMax;
            var pivotBuff = MainRect.pivot;

            MainRect.anchorMin = new Vector2(.5f, .5f);
            MainRect.anchorMax = MainRect.anchorMin;
            MainRect.pivot = MainRect.anchorMax;
            
            MainRect.sizeDelta = new Vector2(Screen.width, Screen.height) / Canvas.scaleFactor;

            MainRect.anchorMin = anchorMinBuff;
            MainRect.anchorMax = anchorMaxBuff;
            MainRect.pivot = pivotBuff;
        }
    }
}
