using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.BaseSystems.UiRelated
{
    public class CanvasResizeReactor : MonoBehaviour
    {
        [SerializeField]
        private bool _update;

        [SerializeField, Range(1,10)]
        private float _updateRate; 

        [SerializeField, Space(10)]
        private Canvas _canvas;

        [SerializeField]
        private Vector2 _initReferenceResolution;

        [SerializeField, Space(10)]
        private Vector2 _previousScreenSize=Vector2.zero; 

        private RectTransform _canvasRectTransform;
        private CanvasScaler _canvasScaler;  

        

        private void OnValidate()
        {
            var currentScreenSize = new Vector2(Screen.width, Screen.height);

            if (currentScreenSize != _previousScreenSize)
                _previousScreenSize = currentScreenSize;

            if (_update)
                _update = false;

            if (_canvas == null)
                _canvas = GetComponent<Canvas>();
        }

        private void Awake()
        {
            if (_canvas == null) return;

            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
            _canvasScaler = _canvas.GetComponent<CanvasScaler>();
            _initReferenceResolution = _canvasScaler.referenceResolution;
        }

        private void OnEnable()
        {
            StartCoroutine(Co_CanvasScaleController());
        }

        private void OnDisable()
        {
            StopAllCoroutines(); 
        }

        private IEnumerator Co_CanvasScaleController()
        {
            while(true)
            {
                //   Debug.Log("\t Screen size: "+Screen.width+" : "+Screen.height);
                //   Debug.Log("\t Canvas scale: " + _canvasRectTransform.localScale+ "\t _initScale: "+ _initScale);

                var currentScreenSize = new Vector2(Screen.width, Screen.height);

                if (currentScreenSize != _previousScreenSize)
                    _previousScreenSize = currentScreenSize;

                /*
                if (_initReferenceResolution.x <  Screen.width ||
                    _initReferenceResolution.y < Screen.height)
                {
                    _canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
                }
                else
                {
                    _canvasScaler.referenceResolution = _initReferenceResolution; 
                }
                */

                /*
                if (_canvasRectTransform.localScale != _initScale)
                {
                    _canvasRectTransform.localScale = _initScale;
                }
                */
                yield return new WaitForSeconds(1 / _updateRate);
            }
        }
    }
}
