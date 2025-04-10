using UnityEngine;
using System;
using TMPro;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Scripts.BaseSystems.Style
{
    [CreateAssetMenu(fileName = "TextStyle", menuName = "Scriptable Obj/Style/Text style")]
    public class TextStyleSrc : ScriptableObject, ITextStyle
    {
        [SerializeField, Header(" item with id Non is mandatory")]
        private bool _ressetNonStyleUnit; 

        [Space(10), SerializeField, Space(10)]
        private StyleUnit[] _styles;

        [NonSerialized]
        private bool _ready;

        [SerializeField]
        private Dictionary<TextStyleId, StyleUnit> _styleDictionary = new Dictionary<TextStyleId, StyleUnit>();
        private Dictionary<TextStyleId, StyleUnit> StyleDictionary
        {
            get
            {
                if (!_ready)
                    Init();

                return _styleDictionary;
            }
        }

        [Serializable]
        private struct StyleUnit
        {
            [HideInInspector]
            public string _name;

            public TextStyleId _id;
            public Color _color;
            public bool _raycastTarget;
            public TMPro.FontStyles _fontStyle;
            public float _fontSize;

            public TMP_FontAsset _fontAsset;

            public StyleUnit(
                string name,
                TextStyleId id,
                Color color,
                bool raycastTarget,
                TMPro.FontStyles fontStyle,
                float _fontSize,
                TMP_FontAsset fontAsset
                )
            {
                _name = name;
                _id = id;
                _color = color;
                _raycastTarget = raycastTarget;
                _fontStyle = fontStyle;
                this._fontSize = _fontSize;
                _fontAsset = fontAsset;
            }
        }

        //  This method contains calls that can be occured only in Unity editor
#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_ressetNonStyleUnit)
            {
                _ressetNonStyleUnit = false;
                TryToDeleteNonStyleUnit();


                var nonStyleUnit = GenerateNonStyleUnit();


                var bufList = new List<StyleUnit>(_styles);
                bufList.Insert(0, nonStyleUnit);
                _styles = bufList.ToArray(); 
            }

            TryToPlaceNonStyleUnitAsFirstItem();

            for (int i = 0; i < _styles.Length; i++)
            {
                var id = _styles[i]._id;
                var color = _styles[i]._color;
                var raycastTarget = _styles[i]._raycastTarget;
                var fontStyle = _styles[i]._fontStyle;
                var fontSize = _styles[i]._fontSize;
                var fontAsset = _styles[i]._fontAsset;

                _styles[i] = new StyleUnit(
                        id.ToString(),
                        id,
                        color,
                        raycastTarget,
                        fontStyle,
                        fontSize,
                        fontAsset
                    );
            }
        }
#endif

        private void TryToPlaceNonStyleUnitAsFirstItem()
        {
            for (int i = 0; i < _styles.Length; i++)
                if (_styles[i]._id == TextStyleId.Non)
                {
                    if (i == 0) return;

                    var bufList = new List<StyleUnit>(_styles);
                    var buffNonUnit = bufList[i];
                    bufList.RemoveAt(i);
                    bufList.Insert(0, buffNonUnit);
                    _styles = bufList.ToArray(); 
                    return;
                }
        }

        private void Init()
        {
            _styleDictionary.Clear();

            for (int i = 0; i < _styles.Length; i++)
            {
                _styleDictionary.Add(_styles[i]._id, _styles[i]);
            }
            _ready = true;
        }

        private void TryToDeleteNonStyleUnit()
        {
            for (int i = 0; i < _styles.Length; i++)
                if (_styles[i]._id == TextStyleId.Non)
                {
                    var bufList = new List<StyleUnit>(_styles);
                    bufList.RemoveAt(i);
                    _styles = bufList.ToArray();
                    return;
                }
        }

#if UNITY_EDITOR
        private StyleUnit GenerateNonStyleUnit()
        {
            var textAsset = AssetDatabase.FindAssets("t:TMP_FontAsset", new[] { "Assets" });
            TMP_FontAsset defaultTextAsset = default;

            Debug.Log("\t font asset amount: " + textAsset.Length);

            if (textAsset.Length != 0)
            {

                for (int i = 0; i < textAsset.Length; i++)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(textAsset[i]);
                    var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                    Debug.Log("\t\t [ " + i + " ] ");

                    if (obj != null)
                        Debug.Log("\t\t\t " + obj.name);

                    defaultTextAsset = obj as TMP_FontAsset;

                    if (defaultTextAsset != null) break;
                }

            }
            else
            {
                Debug.Log("\t font asset amount is 0");
            }


            var nonStyle = new StyleUnit(
                                            TextStyleId.Non.ToString(),
                                            TextStyleId.Non,
                                            Color.magenta,
                                            false,
                                            TMPro.FontStyles.Normal,
                                            20,
                                            defaultTextAsset
                                        );

            return nonStyle; 
        }
#endif

        private StyleUnit GetStyleUnitById(TextStyleId styleId)
        {
            if (Application.isPlaying)
            {
                if(StyleDictionary.ContainsKey(styleId))
                    return StyleDictionary[styleId];

                Debug.LogWarning("Text style obj: "+this.name + " is missing style with id: "+ styleId); 
                return StyleDictionary[TextStyleId.Non];
            }

            for (int i = 0; i < _styles.Length; i++)
            {
                if (_styles[i]._id == styleId)
                    return _styles[i];
            }

            Debug.LogWarning("Text style obj: " + this.name + " is missing style with id: " + styleId);
            return StyleDictionary[TextStyleId.Non];
        }

        public Color GetColor(TextStyleId styleId) => GetStyleUnitById(styleId)._color;

        public bool GetRaycastTarget(TextStyleId styleId) => GetStyleUnitById(styleId)._raycastTarget;

        public TMP_FontAsset GetTmpFontAsset(TextStyleId styleId) => GetStyleUnitById(styleId)._fontAsset;

        public float GetFontSize(TextStyleId styleId) => GetStyleUnitById(styleId)._fontSize;

        public FontStyles GetFontStyle(TextStyleId styleId) => GetStyleUnitById(styleId)._fontStyle;
    }
}
