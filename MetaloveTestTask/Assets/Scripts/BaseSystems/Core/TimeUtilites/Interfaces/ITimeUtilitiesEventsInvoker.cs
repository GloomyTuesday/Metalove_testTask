using System;

namespace Scripts.BaseSystems.Core
{
    public interface ITimeUtilitiesEventsInvoke
    {
        /// <summary>
        ///     Retur 0 if value is NULL
        /// </summary>
        /// <returns></returns>
        public float GetLastFixedUpdateDeltaTime();

        /// <summary>
        ///     Time.timeScale and application pause is taken in account
        /// </summary>
        /// 
        public float GetCurrentFixedUpdateTime();

        public DateTime? GetWwwCurrenDateTime();
    }
}
