using TMPro;
using UnityEngine;

namespace Scripts.BaseSystems.Style
{
    public interface ITextStyle 
    {
        public Color GetColor(TextStyleId styleId);
        public TMP_FontAsset GetTmpFontAsset(TextStyleId styleId);
        public float GetFontSize(TextStyleId styleId); 
        public bool GetRaycastTarget(TextStyleId styleId);
        public FontStyles GetFontStyle(TextStyleId styleId);
    }
}
