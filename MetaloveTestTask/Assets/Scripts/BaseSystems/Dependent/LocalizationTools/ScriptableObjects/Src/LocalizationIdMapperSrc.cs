using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems.Localization
{
    [CreateAssetMenu(fileName = "LocalizationIdMapper" , menuName = "Scriptable Obj/Localization/Localization id mapper")]
    public class LocalizationIdMapperSrc : ScriptableObject, ILocalizationIdMapper
    {
        [SerializeField]
        private LocalizationStringValueUnit[] _localizationStringValues;

        [NonSerialized]
        private bool _dictionaryIsReady;

        private Dictionary<LocalizationId, string> _localizationStringDictionary;
        public Dictionary<LocalizationId, string> LocalizationStringDictionry
        {
            get
            {
                if (!_dictionaryIsReady)
                {
                    _localizationStringDictionary = new Dictionary<LocalizationId, string>();

                    foreach (var item in _localizationStringValues)
                        _localizationStringDictionary.Add(item._localizationId, item._stringValue);

                    _dictionaryIsReady = true;
                }

                return _localizationStringDictionary;
            }
        }

        [Serializable]
        private struct LocalizationStringValueUnit
        {
            [HideInInspector]
            public string _name;

            public LocalizationId _localizationId;

            public string _stringValue; 

            public LocalizationStringValueUnit(
                string name,
                LocalizationId localizationId,
                string value
                )
            {
                _name = name; 
                _localizationId = localizationId;
                _stringValue = value;
            }
        }

        private void OnValidate()
        {
            for (int i = 0; i < _localizationStringValues.Length; i++)
            {
                var name = _localizationStringValues[i]._localizationId.ToString();
                _localizationStringValues[i] = new LocalizationStringValueUnit(
                                                                                name,
                                                                                _localizationStringValues[i]._localizationId,
                                                                                _localizationStringValues[i]._stringValue
                                                                                ); 
            }
        }

        public string GetLocalizationIdStrValue(LocalizationId localizationId)
        {
            if (!LocalizationStringDictionry.ContainsKey(localizationId))
            {
                Debug.LogError("\t There is no string value for localization id: "+ localizationId);
                return "Missing value";
            }

            return LocalizationStringDictionry[localizationId]; 
        }
    }
}
