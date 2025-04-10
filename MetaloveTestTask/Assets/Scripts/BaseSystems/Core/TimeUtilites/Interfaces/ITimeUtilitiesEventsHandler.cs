using System;

namespace Scripts.BaseSystems.Core
{
    public interface ITimeUtilitiesEventsHandler
    {
        /// <summary>
        ///     Time.timeScale and application pause is taken in account
        /// </summary>
        public event Func<float> OnGetCurrentFixedUpdateTime;
        public event Func<float> OnGetLastFixedUpdateDeltaTime;
        public event Func<DateTime> OnGetWwwCurrenDateTime;
    }
}
