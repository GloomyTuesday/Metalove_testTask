using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems.Localization
{
    [CreateAssetMenu(fileName = "LocalizationValue", menuName = "Scriptable Obj/Localization/Localization value")]
    public class LocalizationTextValueSrc : ScriptableObject, ILocalizationTextSource
    {
        [SerializeField, FilterByType(typeof(ILocalizationIdSource))]
        private UnityEngine.Object _localizationIdSourceObj;

        [SerializeField,Space(10)]
        private Data[] _data;

        [NonSerialized]
        private bool _subscribed;

        [NonSerialized]
        private bool _dictionaryIsReady;

        public event Action<string> OnLocalizationStringUpdateed;

        private Dictionary<LocalizationId, string> _localizationStringDictionary = new Dictionary<LocalizationId, string>(); 
        private Dictionary<LocalizationId, string> LocalizationStringDictionry
        {
            get
            {
                if (!_dictionaryIsReady)
                    Init();

                return _localizationStringDictionary;
            }
        }


        private ILocalizationIdSource _localizationIdSource;
        private ILocalizationIdSource ILocalizationIdSource
        {
            get
            {
                if(_localizationIdSource==null)
                    _localizationIdSource = _localizationIdSourceObj.GetComponent<ILocalizationIdSource>();

                return _localizationIdSource;
            }
        }
        [Serializable]
        private struct Data
        {
           // [HideInInspector]
            public string _localizationIdName;

            public LocalizationId _localizationId;

            [TextArea]
            public string _localizationText;

            public Data( string localizationIdName, LocalizationId localizationId, string localizationText)
            {
                _localizationIdName = localizationIdName;
                _localizationId = localizationId;
                _localizationText = localizationText;
            }
            public void SetLocalizationIdName(string name) => _localizationIdName = name;
        }

        private void OnValidate()
        {
            HashSet<LocalizationId> _usedLocalizationId = new HashSet<LocalizationId>();

            for(int i = 0; i < _data.Length; i++ )
            {
                if ((int)_data[i]._localizationId <= 0) continue;

                var name = _data[i]._localizationId.ToString();
                _data[i] = new Data(name, _data[i]._localizationId, _data[i]._localizationText);

                if(_usedLocalizationId.Contains(_data[i]._localizationId))
                {
                    Debug.LogError(" The Id: "+ _data[i]._localizationId+"\t is already in use."); 
                }
                else
                {
                    _usedLocalizationId.Add(_data[i]._localizationId);
                }
            }
        }

        private void Init()
        {
            _localizationStringDictionary.Clear();

            foreach (var item in _data)
                _localizationStringDictionary.Add(item._localizationId, item._localizationText);

            Subscribe(); 

            _dictionaryIsReady = true; 
        }

        private void Subscribe()
        {
            if(_subscribed) return;

            ILocalizationIdSource.OnLocalizationIdUpdate += LocalizationIdUpdate;
            _subscribed = true; 
        }


        private string GetLocalizationStringEditorMod(LocalizationId localizationId)
        {
            foreach (var item in _data)
            {
                if(item._localizationId == localizationId)
                    return item._localizationText;
            }

            return ""; 
        }

        public string GetLocalizationString()
        {
            if(Application.isPlaying)
            {
                if (LocalizationStringDictionry.TryGetValue(ILocalizationIdSource.LocalizationId, out var result)) return result;

                Debug.LogError("\t Missing text for localization id : "+ ILocalizationIdSource.LocalizationId); 
                return "";
            }

            return GetLocalizationStringEditorMod(ILocalizationIdSource.LocalizationId);
        }

        private void LocalizationIdUpdate(LocalizationId localizationId)
        {
            OnLocalizationStringUpdateed?.Invoke( GetLocalizationString() ); 
        }
    }
}
