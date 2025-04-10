using System;

namespace Scripts.BaseSystems.Localization
{
    public interface ILocalizatorText
    {
        public string GetLocalizedValue();
        public event Action<string> OnLocalizedValueUpdated;
    }
}
