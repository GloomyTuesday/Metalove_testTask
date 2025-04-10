using System.Collections.Generic;

namespace Scripts.BaseSystems.Localization
{
    public interface ILocalizationIdMapper 
    {
        public Dictionary<LocalizationId, string> LocalizationStringDictionry { get; }
        public string GetLocalizationIdStrValue(LocalizationId localizationId);
    }
}
