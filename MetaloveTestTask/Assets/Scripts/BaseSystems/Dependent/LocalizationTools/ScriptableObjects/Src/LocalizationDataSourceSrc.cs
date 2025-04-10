using Scripts.BaseSystems.FileIOAndBinary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Scripts.BaseSystems.Localization
{
    [CreateAssetMenu(fileName = "LocalizationDataSource", menuName = "Scriptable Obj/Localization/Localization data source")]
    public class LocalizationDataSourceSrc : ScriptableObject, ILocalizationDataSource
    {
        private char s_rowDelimeter = '\n';

        [SerializeField]
        private bool _update;

        [Space(10),Header("Keep in mind that if there is no localization value for a certain key ")]
        [Header("Localization source will return a first localized value whatevery localization it is.")]

        [Space(10), SerializeField]
        private string _addressableTextAssetName;
        //  private AssetReference _localizationSource;
        //  private UnityEngine.Object _localizationSource;

        [Space(10), SerializeField]
        private float _waitingToLoadTimeSeconds;

        [Space(10), SerializeField, FilterByType(typeof(ILocalizationIdSource))]
        private UnityEngine.Object _localizationIdSourceObj;
        [SerializeField, FilterByType(typeof(IEnumMapperTools))]
        private UnityEngine.Object _enumMapperToolsObj;

        [SerializeField, HideInInspector]
        private int _currentlocalizationSourceHashcode;

        [Space(10), SerializeField]
        private LocalizedUnit[] _localizedData;

        [Space(10), SerializeField]
        private string[] _keyValuesToIgnore;

        [SerializeField]
        private char _cellDelimeter = ',';

        [Space(10), SerializeField]
        private ReplacementInstruction[] _replacementInstruction;

        [NonSerialized]
        private bool _ready;

        [NonSerialized]
        private bool _subscribed;

        [NonSerialized]
        private bool _initializationInProgress;

        /// <summary>
        ///     First dictionary key is an string kay that has a localization value
        /// </summary>
        private Dictionary<string, Dictionary<LocalizationId, string>> _localizationDictionary = new Dictionary<string, Dictionary<LocalizationId, string>>();
        private Dictionary<string, Dictionary<LocalizationId, string>> LocalizationDictionary => _localizationDictionary;

        private Dictionary<string, string> _localizationDescriptionDictionary = new Dictionary<string, string>(); 
        private Dictionary<string, string> LocalizationDescriptionDictionary => _localizationDescriptionDictionary;

        private HashSet<string> KeyValuesToIgnore { get; set; } = new HashSet<string>();

        public bool Ready => _ready;

        private LocalizationId LocalizationId { get; set; } = LocalizationId.Non; 

        private ILocalizationIdSource _iLocalizationIdSource;
        private ILocalizationIdSource ILocalizationIdSource
        {
            get
            {
                if (_iLocalizationIdSource == null)
                    _iLocalizationIdSource = _localizationIdSourceObj.GetComponent<ILocalizationIdSource>();

                return _iLocalizationIdSource;
            }
        }

        private IEnumMapperTools _iEnumMapperTools; 
        private IEnumMapperTools IEnumMapperTools
        {
            get
            {
                if (_iEnumMapperTools == null)
                    _iEnumMapperTools = _enumMapperToolsObj.GetComponent<IEnumMapperTools>();

                return _iEnumMapperTools; 
            }
        }

        public event Action OnLocalizationIdUpdated;

        #region Structures
        [Serializable]
        private struct LocalizedUnit
        {
            [HideInInspector]
            public string _name;
            public string _key;
            public string _description;

            public string[] _values;

            public LocalizedUnit(
                string name,
                string key,
                string description,
                string[] values
                )
            {
                _name = name;
                _key = key;
                _description = description;

                _values = (string[])values.Clone();
            }
        }

        [Serializable]
        private struct ReplacementInstruction
        {
            [HideInInspector]
            public string _name;

            public char _oldSymbol;
            public char _newSymbol;

            public ReplacementInstruction(
                string name,
                char oldSymbol,
                char newSymbol
                )
            {
                _name = name;
                _oldSymbol = oldSymbol;
                _newSymbol = newSymbol;
            }
        }
        #endregion

        private void OnValidate()
        {
            if (_waitingToLoadTimeSeconds < 0)
                _waitingToLoadTimeSeconds = 0; 

            if (_update)
            {
                _update = false; 
            }

            FixReplacementInstructionNames(); 
        }

        public async Task InitAsync()
        {

            if (_initializationInProgress)
            {
                Debug.LogWarning("LocalizationDataSourceSrc Localization initialization is in progress ");

                while (!_ready)
                    await Task.Yield();

                Debug.LogWarning("LocalizationDataSourceSrc Localization initialization await finished \t localization is ready: " + _ready);
                return; 
            }

            _initializationInProgress = true; 

            var localizationAsset = await GetLocalizatationAddressableTextAssetAsync(_addressableTextAssetName).ConfigureAwait(true); 

            if(localizationAsset == null)
            {
                Debug.LogWarning("LocalizationDataSourceSrc Localization asset is null, initialization is interrupted");
                _ready = false;
                return; 
            }

            FillLocalizationDictionaryes(localizationAsset);

            if (!_subscribed)
                Subscribe();

            _initializationInProgress = false; 
            _ready = true; 
        }
        
        public void Init()
        {
            if (_keyValuesToIgnore != null || _keyValuesToIgnore.Length < 1)
            {
                for (int i = 0; i < _keyValuesToIgnore.Length; i++)
                    if (!KeyValuesToIgnore.Contains(_keyValuesToIgnore[i]))
                        KeyValuesToIgnore.Add(_keyValuesToIgnore[i]); 
            }

            var localizationAsset = GetLocalizatationAddressableTextAsset(_addressableTextAssetName);

            if (localizationAsset == null)
            {
                Debug.LogWarning("LocalizationDataSourceSrc Localization asset is null"); 
                _ready = false;
                return;
            }

            FillLocalizationDictionaryes(localizationAsset);

            if (!_subscribed)
                Subscribe();

            _ready = true;
        }

        private void Subscribe()
        {
            if (_subscribed) return;

            ILocalizationIdSource.OnLocalizationIdUpdate += OnLocalizationIdUpdate;

            _subscribed = true;
        }

        private void OnLocalizationIdUpdate(LocalizationId localizationSourceId)
        {
            LocalizationId = localizationSourceId;
            OnLocalizationIdUpdated?.Invoke();
        }

        private void FillLocalizationDictionaryes(TextAsset localizationAsset)
        {
            /*
              First row should consist from: 
                
                column:
                [ 0 ]   -   Can be empty ( is ignored by script ), this column should contain description for localization key
                [ 1 ]   -   Should be empty ( is ignored by script ), this column should contain localization key, but first row should be mepty
                [ 2 ]   -   LocalizationId
                [ 3 ]   -   LocalizationId
                ...     -   LocalizationId
                and so on
            */

            var columnWithKeyIndex = 1;
            var columnWithDescriptionIndex = 0;
            string[] rows = localizationAsset.text.Split(s_rowDelimeter);

            _localizationDictionary = new Dictionary<string, Dictionary<LocalizationId, string>>();

            var localizationFileRowItems = rows[0].Split(_cellDelimeter);

            var localizationIdList = GetLocalizationIdArray(localizationFileRowItems);

            var localizationIdIndexThatMissesLocalizationValue = new List<int>();
            var firstLocalizedValueBuffer = ""; // Is used to fill the localization empty cells

            //  Because first row was used to fill with values: localizationIdList 
            for (int i = 1; i < rows.Length; i++)
            {
                localizationFileRowItems = rows[i].Split(_cellDelimeter);

                if (localizationFileRowItems.Length < columnWithKeyIndex) continue; 
                if (KeyValuesToIgnore.Contains(localizationFileRowItems[columnWithKeyIndex])) continue;

                if (_localizationDescriptionDictionary.ContainsKey(localizationFileRowItems[columnWithKeyIndex]))
                {
                    Debug.LogWarning("LocalizationDataSourceSrc Localization key: " + localizationFileRowItems[columnWithKeyIndex] +"\t already added"); 
                    continue; 
                }

                //  First column contains description and the second one contains the they for localization value
                _localizationDescriptionDictionary.Add(localizationFileRowItems[columnWithKeyIndex], localizationFileRowItems[columnWithDescriptionIndex]);
                firstLocalizedValueBuffer = localizationFileRowItems[columnWithKeyIndex];

                _localizationDictionary.Add(localizationFileRowItems[columnWithKeyIndex], new Dictionary<LocalizationId, string>());

                for (int j = 0; j < localizationFileRowItems.Length - 1; j++)
                {
                    if (localizationIdList[j] == LocalizationId.Non) continue;

                    if (localizationFileRowItems[j] == null || localizationFileRowItems[j].Length < 1)
                    {
                        //  originalText.Replace(searchString, replacementString);
                        localizationFileRowItems[j] = ApplyReplacement(localizationFileRowItems[j]); 
                        _localizationDictionary[localizationFileRowItems[columnWithKeyIndex]].Add(
                                                                                localizationIdList[j ],
                                                                                localizationFileRowItems[columnWithKeyIndex]
                                                                                );

                        localizationIdIndexThatMissesLocalizationValue.Add(j);
                        continue;
                    }

                    _localizationDictionary[localizationFileRowItems[columnWithKeyIndex]].Add(
                                                                            localizationIdList[j],
                                                                            localizationFileRowItems[j]
                                                                            );

                    if (firstLocalizedValueBuffer == "")
                        firstLocalizedValueBuffer = localizationFileRowItems[j];
                }

                //  Fill unlocalized values with first localized values no matter what localizationId it was
                if (firstLocalizedValueBuffer == "") continue;

                for (int j = 0; j < localizationIdIndexThatMissesLocalizationValue.Count; j++)
                {
                    var index = localizationIdIndexThatMissesLocalizationValue[j];
                    _localizationDictionary[localizationFileRowItems[columnWithKeyIndex]][localizationIdList[index]] = firstLocalizedValueBuffer;
                }

                localizationIdIndexThatMissesLocalizationValue.Clear();
                firstLocalizedValueBuffer = "";
            }

#if UNITY_EDITOR
            FillLocalizedData();
#endif
        }

        private LocalizationId[] GetLocalizationIdArray(string[] strArray)
        {
            var localizationIdArray = new LocalizationId[strArray.Length];

            for (int i = 0; i < strArray.Length; i++)
            {
                var result = IEnumMapperTools.GetEnumValue<LocalizationId>(strArray[i]);
                localizationIdArray[i] = result == null ? LocalizationId.Non : (LocalizationId)result.Value;
            }

            return localizationIdArray; 
        }

        private string ApplyReplacement(string str)
        {
            if (_replacementInstruction == null) return str; 

            for (int i = 0; i < _replacementInstruction.Length; i++)
                str = str.Replace(_replacementInstruction[i]._oldSymbol, _replacementInstruction[i]._newSymbol);

            return str; 
        }

        private async Task<TextAsset> GetLocalizatationAddressableTextAssetAsync(string AddressableAssetName)
        {
            AsyncOperationHandle<TextAsset> localizationTextAssetOpHandle;
            try
            {
                localizationTextAssetOpHandle = Addressables.LoadAssetAsync<TextAsset>(AddressableAssetName);
            }
            catch ( Exception e)
            {
                Debug.LogError(" "+e.Message);
                return null; 
            }

            var ready = false;
            var endTime = Time.realtimeSinceStartup + _waitingToLoadTimeSeconds;
            TextAsset localizationTextAsset = null; 

            Action<AsyncOperationHandle<TextAsset>> OnCompleteAction = (handle) => 
            {
                localizationTextAsset = handle.Result;
                ready = true; 
            };

            localizationTextAssetOpHandle.Completed += OnCompleteAction;

            while (!ready || Time.realtimeSinceStartup < endTime)
                await Task.Yield();

            localizationTextAssetOpHandle.Completed -= OnCompleteAction;

            if (!ready) return null;

            return localizationTextAsset; 
        }

        private TextAsset GetLocalizatationAddressableTextAsset(string AddressableAssetName)
        {
            var localizationTextAssetOpHandle = Addressables.LoadAssetAsync<TextAsset>(AddressableAssetName);

            var localizationTextAsset = localizationTextAssetOpHandle.WaitForCompletion();

            return localizationTextAsset;
        }

        public string GetLocalizationValue(string key, string defaultValue = "")
        {
            if (LocalizationId == LocalizationId.Non)
                LocalizationId = ILocalizationIdSource.LocalizationId;

            return GetLocalizationValue(LocalizationId, key, defaultValue);
        }

        public string GetLocalizationValue(LocalizationId localizationId, string key, string defaultValue = "" )
        {
            if (defaultValue.Length < 1)
                defaultValue = key;

            if (!LocalizationDictionary.ContainsKey(key))
            {
                Debug.LogWarning("Missing localization value for key: " + key);
                return defaultValue;
            }

            if (!LocalizationDictionary[key].ContainsKey(localizationId))
            {
                Debug.LogWarning("No localization id: " + localizationId + "\t is missing from localization data source");
                return defaultValue;
            }

            return LocalizationDictionary[key][localizationId];
        }

        public async Task<string> GetLocalizationValueAsync(string key, string defaultValue = "")
        {
            if (!_ready)
                await InitAsync();

            if (LocalizationId == LocalizationId.Non)
                LocalizationId = ILocalizationIdSource.LocalizationId;

            return GetLocalizationValue(LocalizationId, key, defaultValue);
        }

        public async Task<string> GetLocalizationValueAsync(LocalizationId localizationId, string key, string defaultValue = "")
        {
            if (!_ready)
                await InitAsync();

            return GetLocalizationValue(localizationId, key, defaultValue);
        }

        private void FillLocalizedData()
        {
            int count = 0; 
            _localizedData = new LocalizedUnit[LocalizationDictionary.Count];

            foreach (var item in LocalizationDictionary)
            {
                var name = item.Key;
                var description = LocalizationDescriptionDictionary[item.Key];

                string[] values = new string[item.Value.Count];

                int valueCount = 0;

                foreach (var valueItem in item.Value)
                {
                    values[valueCount] = valueItem.Value;
                    valueCount++; 
                }

                _localizedData[count] = new LocalizedUnit(
                    name,
                    item.Key,
                    description,
                    values
                    );

                count++; 
            }
        }

        private void FixReplacementInstructionNames()
        {
            if (_replacementInstruction == null) return;

            for (int i = 0; i < _replacementInstruction.Length; i++)
            {
                var instruction = _replacementInstruction[i];
                string name = instruction._oldSymbol + "\t" + instruction._newSymbol;
                _replacementInstruction[i] = new ReplacementInstruction(name, instruction._oldSymbol,instruction._newSymbol); 
            }
        }
    }
}
