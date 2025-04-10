using Scripts.BaseSystems.Core;
using System;
using System.Threading.Tasks;

namespace Scripts.BaseSystems.Localization
{
    public interface ILocalizationDataSource: IReady
    {
        //  public Task InitAsync();
        public void Init();

        /// <summary>
        ///     Default value is used in case when localizator could not find value for the key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetLocalizationValue(string key, string defaultValue = "");

        /// <summary>
        ///     Default value is used in case when localizator could not find value for the key
        /// </summary>
        /// <param name="localizationId"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetLocalizationValue(LocalizationId localizationId, string key, string defaultValue = "");

        /// <summary>
        ///     Default value is used in case when localizator could not find value for the key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        //  public Task<string> GetLocalizationValueAsync(string key, string defaultValue = "");

        /// <summary>
        ///     Default value is used in case when localizator could not find value for the key
        /// </summary>
        /// <param name="localizationId"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        //  public Task<string> GetLocalizationValueAsync(LocalizationId localizationId, string key, string defaultValue = "");

        public event Action OnLocalizationIdUpdated;

    }
}
