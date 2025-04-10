using System;
using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    [CreateAssetMenu(fileName = "TimeUtilitiesEvents", menuName = "Scriptable Obj/Base systems/Core/Time utilites/Time utilities events")]
    public class TimeUtilitiesEventsSrc : 
        ScriptableObject,
        ITimeUtilitiesEventsHandler, 
        ITimeUtilitiesEventsInvoke,
        ITimeUtilitiesEventsCallbackInvoker,
        ITimeUtilitiesEventsCallbackHandler
    {
        private Func<float> _onGetLastFixedUpdateDeltaTime;
        public event Func<float> OnGetLastFixedUpdateDeltaTime
        {
            add => _onGetLastFixedUpdateDeltaTime += value;
            remove => _onGetLastFixedUpdateDeltaTime -= value;
        }

        /// <summary>
        ///     Retur 0 if value is NULL
        /// </summary>
        /// <returns></returns>
        public float GetLastFixedUpdateDeltaTime()
        {
            var result = _onGetLastFixedUpdateDeltaTime?.Invoke();

            if (result == null) return 0;

            return result.Value; 
        }


        private Func<DateTime> _onGetWwwCurrenDateTime;
        public event Func<DateTime> OnGetWwwCurrenDateTime
        {
            add => _onGetWwwCurrenDateTime += value;
            remove => _onGetWwwCurrenDateTime -= value;
        }
        public DateTime? GetWwwCurrenDateTime() => _onGetWwwCurrenDateTime?.Invoke();


        /// <summary>
        ///     Time.timeScale and application pause is taken in account, in cse of null method returns 0
        /// </summary>
        /// 
        private Func<float> _onGetCurrentFixedUpdateTime;
        public event Func<float> OnGetCurrentFixedUpdateTime
        {
            add => _onGetCurrentFixedUpdateTime += value;
            remove => _onGetCurrentFixedUpdateTime -= value;
        }
        public float GetCurrentFixedUpdateTime()
        {
            var result = _onGetCurrentFixedUpdateTime?.Invoke();

            if (result == null) return 0;

            return result.Value;
        }


        private Action _onUnityFixedUpdate;
        public event Action OnUnityFixedUpdate
        {
            add => _onUnityFixedUpdate += value;
            remove => _onUnityFixedUpdate -= value;
        }
        public void UnityFixedUpdate() => _onUnityFixedUpdate?.Invoke(); 
    }
}
