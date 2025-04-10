using System;

namespace Scripts.BaseSystems.Localization

{
    public interface ILocalizationIdSource 
    {
        public LocalizationId LocalizationId { get; }

        public event Action<LocalizationId> OnLocalizationIdUpdate;
        public void SetLocalization(LocalizationId localizationId);
    }
}
