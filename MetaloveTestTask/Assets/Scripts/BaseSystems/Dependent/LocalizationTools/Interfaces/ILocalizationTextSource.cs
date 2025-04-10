using System;

namespace Scripts.BaseSystems.Localization
{
    public interface ILocalizationTextSource
    {
        public string GetLocalizationString();
        public event Action<string> OnLocalizationStringUpdateed; 
    }
}
