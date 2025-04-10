using System;
using UnityEngine;

namespace Scripts.BaseSystems.Localization
{
    [CreateAssetMenu(fileName = "ApplicationLocalizationIdSource", menuName = "Scriptable Obj/Localization/Application localization id source")]
    public class ApplicationLocalizationIdSourceSrc : ScriptableObject, ILocalizationIdSource
    {
        [SerializeField]
        private LocalizationId _localization = LocalizationId.English;
        private LocalizationId _previousLocalizationId; 

        public LocalizationId LocalizationId => _localization;

        public event Action<LocalizationId> OnLocalizationIdUpdate;

        private void OnValidate()
        {
            if(_previousLocalizationId!= _localization)
            {
                _previousLocalizationId = _localization;
                OnLocalizationIdUpdate?.Invoke(_localization);
            }
        }

        public void SetLocalization(LocalizationId localizationId)
        {
            if (_localization == localizationId) return;

            _localization = localizationId;
            OnLocalizationIdUpdate?.Invoke(_localization); 
        }
    }
}
