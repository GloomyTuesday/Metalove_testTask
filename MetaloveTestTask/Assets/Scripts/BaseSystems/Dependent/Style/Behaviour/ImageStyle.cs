using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.BaseSystems.Style
{
    [RequireComponent(typeof(Image))]
    public class ImageStyle : MonoBehaviour
    {
        [SerializeField]
        private bool _applyStretchModeAndOffsetFromStyle;
        [SerializeField]
        private bool _invertOffset;

        [SerializeField, Space(10)]
        private bool _useColorStyle=true;
        [SerializeField]
        private bool _useSpriteStyle = true;
        [SerializeField]
        private bool _useMaterialStyle = true;
        [SerializeField]
        private bool _useImageTypeStyle = true;
        [SerializeField]
        private bool _useRayCastTargetStyle = true; 

        //  Inspector visibility constraints can be changed in ImageExtendedEditor file
        [SerializeField]
        public ImageStyleId _imageStyleId;

        [SerializeField, Space(10), FilterByType(typeof(IImageStyle))]
        public UnityEngine.Object _imageStyleObj;

        private RectTransform _localRectTransform;
        private RectTransform LocalRectTransform
        {
            get
            {
                if(_localRectTransform==null)
                    _localRectTransform = GetComponent<RectTransform>();

                return _localRectTransform;
            }
        }

        private Image _image; 
        private Image Image
        {
            get
            {
                if(_image == null)
                    _image = GetComponent<Image>();

                return _image;
            }
        }

        private IImageStyle _iImageStyle;

        //  In case the name is changed it al so should be changed in ImageStyleDrawer
        public IImageStyle IImageStyle  
        {
            get
            {
                if(_iImageStyle==null)
                {
                    if (_imageStyleObj == null) return default;

                    _iImageStyle = _imageStyleObj as IImageStyle;
                }

                return _iImageStyle;
            }
        }

        private void OnValidate()
        {
            ApplyStyle();
        }

        private void OnEnable()
        {
            ApplyStyle();
        }

        public void ApplyStyle()
        {
            if (_imageStyleObj == null) return;

            try
            {
                if(_useColorStyle)
                    Image.color = IImageStyle.GetColor(_imageStyleId);

                if(_useSpriteStyle)
                    Image.sprite = IImageStyle.GetSprite(_imageStyleId);

                if(_useMaterialStyle)
                    Image.material = IImageStyle.GetMaterial(_imageStyleId);

                if(_useImageTypeStyle)
                    Image.type = IImageStyle.GetType(_imageStyleId);

                if(_useRayCastTargetStyle)
                    Image.raycastTarget = IImageStyle.GetRaycastTarget(_imageStyleId);

                if (_applyStretchModeAndOffsetFromStyle)
                {
                    var offsetData = IImageStyle.GetOffset(_imageStyleId);

                    LocalRectTransform.anchorMin = new Vector2(0, 0);
                    LocalRectTransform.anchorMax = new Vector2(1, 1);
                    LocalRectTransform.pivot = new Vector2(.5f, .5f);

                    LocalRectTransform.ForceUpdateRectTransforms();

                    var invertValue = _invertOffset == true ? 1 : -1;

                    LocalRectTransform.offsetMin = offsetData.Item1 * invertValue;
                    LocalRectTransform.offsetMax = -offsetData.Item2 * invertValue;
                }
                    
            }
            catch ( Exception e)
            {
                Debug.Log("\t "+gameObject.name+"\t requested Id: "+ _imageStyleId +"\t "+ e); 
            }
        }
    }

}
