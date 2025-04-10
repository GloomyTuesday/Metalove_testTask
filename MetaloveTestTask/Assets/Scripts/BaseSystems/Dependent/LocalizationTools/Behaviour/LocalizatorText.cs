/*

    Inspector of this monobehaviour is controlled by: LocalizatorTextEditor

*/

using UnityEngine;
using TMPro;
using System;
using Scripts.BaseSystems.Core;

namespace Scripts.BaseSystems.Localization
{
    public class LocalizatorText : MonoBehaviour, ILocalizatorText, IReady
    {
        [SerializeField]
        private bool _update;

        [Space(10), SerializeField]
        private string _localizationKey;

        [Space(10), SerializeField, TextArea]
        private string _initValue;

        [SerializeField]
        private string _localizedValue; 

        //  Export source fields
        //  Keep in mind that inspector output is controlled by TextLocalizatorEditor
        [Space(10),SerializeField]
        private ExportLocalizationSourceId _exportSourceId = ExportLocalizationSourceId.TextMeshProUGUI;

        [SerializeField]
        private TextMeshProUGUI _textMeshPro;

        [SerializeField]
        private TextMeshProUGUI[] _textMeshProCollection;
        [SerializeField]
        private TMP_InputField _tmpInputField;
        [SerializeField]
        private TMP_InputField[] _tmpInputFieldCollection;

        //  Import source fields
        //  Keep in mind that inspector output is controlled by TextLocalizatorEditor
        [SerializeField, Space(10)]
        private ImportLocalizationSourceId _importSourceId = ImportLocalizationSourceId.FilteredByType_ITextLocalizator;

        [SerializeField]
        private ScriptableObject _importSourceScriptableObject;
        [SerializeField, FilterByType(typeof(ILocalizationDataSource))]
        private UnityEngine.Object _localizationDataSourceObj;


        private ILocalizationDataSource _iLocalizationDataSource; 
        private ILocalizationDataSource ILocalizationDataSource
        {
            get
            {
                if (_iLocalizationDataSource == null)
                    _iLocalizationDataSource = _localizationDataSourceObj.GetComponent<ILocalizationDataSource>();

                return _iLocalizationDataSource; 
            }
        }

        private string LocalizedValue {
            get
            {
                return _localizedValue; 
            }

            set
            {
                _localizedValue = value; 
            }
        }

        private bool _ready; 
        public bool Ready => _ready;

        public event Action<string> OnLocalizedValueUpdated;

        private void OnValidate()
        {
            if (_update)
            {
                Init();
                _update = false;
            }
        }

        private void OnEnable()
        {
            LocalizedValue = _initValue;

            Init();

            if(ILocalizationDataSource!=null)
                Subscribe();
        }

        private void OnDisable()
        {
            if (ILocalizationDataSource != null)
                Unsubscribe();
        }

        private void Subscribe()
        {
            ILocalizationDataSource.OnLocalizationIdUpdated += LocalizationStringUpdateed;
        }

        private void Unsubscribe()
        {
            ILocalizationDataSource.OnLocalizationIdUpdated -= LocalizationStringUpdateed;
        }

        private void LocalizationStringUpdateed()
        {
            var newString = ILocalizationDataSource.GetLocalizationValue(_localizationKey);

            if (!IsValid(newString)) return;

            ApplyString(newString);
            OnLocalizedValueUpdated?.Invoke(newString); 
        }

        private void Init()
        {
            if (ILocalizationDataSource == null) return;

            LocalizedValue = ILocalizationDataSource.GetLocalizationValue(_localizationKey, _initValue);

            if (!IsValid(LocalizedValue)) 
            {
                Debug.LogWarning(gameObject.name + " LocalizedValue value is not valid");
                LocalizedValue = _initValue;
                return; 
            }

            ApplyString(LocalizedValue);
            _ready = true; 
        }

        private bool IsValid(string localizedString)
        {
            if (_exportSourceId == ExportLocalizationSourceId.Non) return false;
            if (localizedString == null) return false;

            return true;
        }

        private void ApplyString(string localizedString)
        {
            switch (_exportSourceId)
            {
                case ExportLocalizationSourceId.Non:
                    break;
                case ExportLocalizationSourceId.TextMeshProUGUI:
                    if (_textMeshPro == null) return;
                    _textMeshPro.text = localizedString;
                    break;
                case ExportLocalizationSourceId.TextMeshProUGUI_collection:
                    foreach (var item in _textMeshProCollection)
                    {
                        if (item == null) continue;

                        item.text = localizedString;
                    }
                    break;
                case ExportLocalizationSourceId.TMP_InputField:
                    if (_tmpInputField == null) return;

                    _tmpInputField.SetTextWithoutNotify(localizedString);
                    _tmpInputField.ForceLabelUpdate();
                    break;
                case ExportLocalizationSourceId.TMP_InputField_collection:
                    foreach (var item in _tmpInputFieldCollection)
                    {
                        if (item == null) continue;

                        item.SetTextWithoutNotify(localizedString);
                    }
                    break;
                case ExportLocalizationSourceId.String:
                    //  There's nothing to do here right now, as the localization result is applyed to _localizedValue
                    break; 
                default:
                    break;
            }
        }

        public string GetLocalizedValue()
        {
            Debug.Log("\t Localization get key: "+_localizationKey+"\t value: "+ LocalizedValue);
            if (!_ready)
                Init();

            if (ILocalizationDataSource == null)
            {
                Debug.LogWarning( gameObject.name+" ILocalizationDataSource is NULL, localizator init value is used");
                
                if(_initValue.Length>0)
                    return _initValue;

                Debug.LogWarning(gameObject.name + " Init value is empty.");
                return _localizationKey; 
            }

            return LocalizedValue; 
        }
    }
}
