using Scripts.BaseSystems.Core;
using UnityEngine;

namespace Scripts.BaseSystems.Localization
{
    public class LocalizationInitializer : MonoBehaviour, IReady
    {   
        [Header("Localization objects to initialize")]
        [SerializeField, FilterByType(typeof(ILocalizationDataSource))]
        private Object _localizationDataSourceObj;

        private bool _ready; 
        public bool Ready => _ready;

        private void OnEnable()
        {
            var localizationDataSource = _localizationDataSourceObj.GetComponent<ILocalizationDataSource>();

            if (localizationDataSource != null)
            {
                localizationDataSource.Init();
            }
            else
            {
                Debug.Log("\t LocalizationInitializer Localization data source obj is NULL  ");
            }
            _ready = true;
        }
    }
}
