using UnityEngine;
using TMPro;
using System;

namespace Scripts.BaseSystems.Style
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextMeshProStyle : MonoBehaviour
    {
        [SerializeField]
        private bool _update;

        [SerializeField, Space(10)]
        private TextStyleId _textStyleId = TextStyleId.Normal;

        [SerializeField, Space(10)]
        private bool _useFont = true;
        [SerializeField]
        private bool _useColor = true;
        [SerializeField]
        private bool _useMaterial = true;
        [SerializeField]
        private bool _useTextSize = true;
        [SerializeField]
        private bool _useRayCast = true;
        [SerializeField]
        private bool _fontStyle = true;

        [SerializeField, Space(10), FilterByType(typeof(ITextStyle))]
        public UnityEngine.Object _textStyleObj;

        private TextMeshProUGUI _textMeshProUGUI;
        private TextMeshProUGUI TextMeshProUGUI
        {
            get
            {
                if (_textMeshProUGUI == null)
                    _textMeshProUGUI = GetComponent<TextMeshProUGUI>();

                return _textMeshProUGUI;
            }
        }

        private ITextStyle _iTextStyle;
        public ITextStyle ITextStyle
        {
            get
            {
                if (_iTextStyle == null)
                    _iTextStyle = _textStyleObj as ITextStyle;

                return _iTextStyle;
            }
        }

        private void OnValidate()
        {
            if (_update)
            {
                _update = false;
            }

            ApplyStyle();
        }

        private void OnEnable()
        {
            ApplyStyle();
        }

        public void ApplyStyle()
        {
            if (_textStyleObj == null)
            {
                Debug.LogWarning("\t " + gameObject.name + "\t _textStyleObj is NULL");
                return;
            }

            if (TextMeshProUGUI == null)
            {
                Debug.LogWarning("\t " + gameObject.name + "\t TextMeshProUGUI is NULL");
                return;
            }

            if (ITextStyle == null)
            {
                Debug.LogWarning("\t " + gameObject.name + "\t ITextStyle is NULL");
                return;
            }

            if (_useColor)
                TextMeshProUGUI.color = ITextStyle.GetColor(_textStyleId);

            if (_useFont)
                TextMeshProUGUI.font = ITextStyle.GetTmpFontAsset(_textStyleId);

            if (_useRayCast)
                TextMeshProUGUI.raycastTarget = ITextStyle.GetRaycastTarget(_textStyleId);

            if (_useTextSize)
                TextMeshProUGUI.fontSize = ITextStyle.GetFontSize(_textStyleId);

            if (_fontStyle)
                TextMeshProUGUI.fontStyle = ITextStyle.GetFontStyle(_textStyleId);
        }
    }
}
