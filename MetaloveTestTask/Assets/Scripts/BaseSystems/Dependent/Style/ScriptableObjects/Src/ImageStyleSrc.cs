using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.BaseSystems.Style
{
    [CreateAssetMenu(fileName = "ImageStyle", menuName = "Scriptable Obj/Style/Image style")]
    public class ImageStyleSrc : ScriptableObject, IImageStyle
    {
        [SerializeField, Header(" item with id Non is mandatory")]
        private bool _ressetNonStyleUnit;

        [Space(10),SerializeField]
        private StyleUnit[] _styles;

        [NonSerialized]
        private bool _ready;

        [SerializeField]
        private ImageStyleId[] _availableStyleId;
        public ImageStyleId[] AvailableStyleId
        {
            get
            {
                if (!_ready)
                    Init();

                return _availableStyleId; 
            }
        }

        [SerializeField]
        private Dictionary<ImageStyleId,StyleUnit> _styleDictionary = new Dictionary<ImageStyleId, StyleUnit> ();
        private Dictionary<ImageStyleId, StyleUnit> StyleDictionary
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

            public ImageStyleId _id;
            public Color _color;
            public Sprite _sprite;
            public Material _material; 
            public Image.Type _type;
            public bool _raycastTarget;

            [Space(10), Header("Values for case when is used stretch anchor preset.")]
            public Vector2 _offsetMin;
            public Vector2 _offsetMax;

            public StyleUnit(
                string name,
                ImageStyleId id,
                Color color,
                Sprite sprite,
                Material material,
                Image.Type type,
                bool raycastTarget,
                Vector2 offsetMin,
                Vector2 offsetMax
                )
            {
                _name = name;
                _id = id;
                _color = color;
                _sprite = sprite;
                _material = material;
                _type = type;
                _raycastTarget = raycastTarget;
                _offsetMin = offsetMin; 
                _offsetMax = offsetMax;
            }

        }

        [Obsolete]
        private void OnValidate()
        {
            if (_ressetNonStyleUnit)
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
                _styles[i]._name = i+" "+_styles[i]._id.ToString().Replace("_", " ");

            var imageExtendedObjects = UnityEngine.Object.FindObjectsOfType<ImageStyle>();
            foreach (var item in imageExtendedObjects)
                item.ApplyStyle();

            _ready = false; 
        }
        
        private void Init()
        {
            _styleDictionary.Clear();
            _availableStyleId = new ImageStyleId[_styles.Length]; 

            for (int i = 0; i < _styles.Length; i++)
            {
                _availableStyleId[i] = _styles[i]._id;
                _styleDictionary.Add(_styles[i]._id, _styles[i]);
            }
            _ready = true;
        }

        private void TryToDeleteNonStyleUnit()
        {
            for (int i = 0; i < _styles.Length; i++)
                if (_styles[i]._id == ImageStyleId.Non)
                {
                    var bufList = new List<StyleUnit>(_styles);
                    bufList.RemoveAt(i);
                    _styles = bufList.ToArray();
                    return;
                }
        }

        private StyleUnit GenerateNonStyleUnit()
        {
            var nonStyle = new StyleUnit(
                                              ImageStyleId.Non.ToString(),
                                              ImageStyleId.Non,
                                              Color.magenta,
                                              null,
                                              null,
                                              Image.Type.Sliced,
                                              false,
                                              Vector2.zero,
                                              Vector2.zero
                                          );

            return nonStyle;
        }

        private void TryToPlaceNonStyleUnitAsFirstItem()
        {
            for (int i = 0; i < _styles.Length; i++)
                if (_styles[i]._id == ImageStyleId.Non)
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

        private StyleUnit GetStyleUnitById(ImageStyleId styleId)
        {
            if (Application.isPlaying)
            {
                if (StyleDictionary.ContainsKey(styleId))
                    return StyleDictionary[styleId];

                Debug.LogWarning("Text style obj: " + this.name + " is missing style with id: " + styleId);
                return StyleDictionary[ImageStyleId.Non];
            }

            for (int i = 0; i < _styles.Length; i++)
            {
                if (_styles[i]._id == styleId)
                    return _styles[i];
            }

            Debug.LogWarning("Text style obj: " + this.name + " is missing style with id: " + styleId);
            return StyleDictionary[ImageStyleId.Non];
        }

        public Sprite GetSprite(ImageStyleId styleId)
        {
            var sprite = GetStyleUnitById(styleId)._sprite;

            if(sprite == null)
            {
                Vector2Int spriteSize = new Vector2Int(4, 4);

                Texture2D texture = new Texture2D(spriteSize.x, spriteSize.y);
                Color[] pixels = new Color[spriteSize.x * spriteSize.y];
                for (int i = 0; i < pixels.Length; i++)
                    pixels[i] = Color.white;

                texture.SetPixels(pixels);
                texture.Apply();

                sprite = Sprite.Create(texture, new Rect(0, 0, spriteSize.x, spriteSize.y), Vector2.one * .5f);
            }

            return sprite; 
        }

        public Color GetColor(ImageStyleId styleId) => GetStyleUnitById(styleId)._color;

        public Material GetMaterial(ImageStyleId styleId) => GetStyleUnitById(styleId)._material;

        public Image.Type GetType(ImageStyleId styleId) => GetStyleUnitById(styleId)._type;

        public bool GetRaycastTarget(ImageStyleId styleId) => GetStyleUnitById(styleId)._raycastTarget; 

        /// <summary>
        ///     first item is offset min
        ///     second item is offset max
        /// </summary>
        /// <param name="styleId"></param>
        /// <returns></returns>
        public (Vector2, Vector2) GetOffset(ImageStyleId styleId)
        {
             var styleItem = GetStyleUnitById(styleId);

            return (styleItem._offsetMin, styleItem._offsetMax);
        }
    }
}
