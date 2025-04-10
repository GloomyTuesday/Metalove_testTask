using Scripts.BaseSystems.InternetTools;
using System;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    public class TimeUtilityObserver : MonoBehaviour, IReady
    {
        [SerializeField]
        [FilterByType(typeof(ITimeUtilitiesEventsHandler))]
        private UnityEngine.Object _timeUtilitiesEventsHandlerObj;

        [SerializeField]
        [FilterByType(typeof(ITimeUtilitiesEventsCallbackInvoker))]
        private UnityEngine.Object _timeUtilitiesEventsCallbackInvokerObj;

        [SerializeField]
        [FilterByType(typeof(IInternetTools))]
        private UnityEngine.Object _iInternetToolsObj;

        private float FixedUpdateTimehenasPaused { get; set; }
        private float FixedUpdatePauseDelay { get; set; }
        private float FixeUpdateTimeSinceStart { get; set; }
        private float LastFixedUpdateDeltaTime { get; set; }
        private bool IsApplicationPaused { get; set; }

        private WwwTimeSourceData[] InternetTimeDataSources =
        {
            new WwwTimeSourceData("time.nist.gov",13)
        };

        private DateTime WwwConnectsEstablishedDateTime { get; set; }
        private float SecondsFromAppStartConnectionEstablished { get; set; }

        private bool _ready;
        public bool Ready => _ready;

        private ITimeUtilitiesEventsHandler _iTimeUtilitiesEventsHandler; 
        private ITimeUtilitiesEventsHandler ITimeUtilitiesEventsHandler
        {
            get
            {
                if(_iTimeUtilitiesEventsHandler==null)
                    _iTimeUtilitiesEventsHandler = _timeUtilitiesEventsHandlerObj.GetComponent<ITimeUtilitiesEventsHandler>();

                return _iTimeUtilitiesEventsHandler;
            }
        }

        private ITimeUtilitiesEventsCallbackInvoker _iTimeUtilitiesEventsCallbackInvoker;
        private ITimeUtilitiesEventsCallbackInvoker ITimeUtilitiesEventsCallbackInvoker
        {
            get
            {
                if (_iTimeUtilitiesEventsCallbackInvoker == null)
                    _iTimeUtilitiesEventsCallbackInvoker = _timeUtilitiesEventsCallbackInvokerObj.GetComponent<ITimeUtilitiesEventsCallbackInvoker>();

                return _iTimeUtilitiesEventsCallbackInvoker;
            }
        }

        private IInternetTools _iInternetTools;
        private IInternetTools IInternetTools
        {
            get
            {
                if (_iInternetTools == null)
                    _iInternetTools = _iInternetToolsObj.GetComponent<IInternetTools>();

                return _iInternetTools;
            }
        }

        #region Internal models

        private struct WwwTimeSourceData
        {
            private string _url;
            public string URL => _url;

            private int _port;
            public int Port => _port;

            public WwwTimeSourceData(string url, int port)
            {
                _url = url;
                _port = port;
            }
        }

        #endregion

        private async void OnEnable()
        {
            await InitializeWwwDateTime();

            Subscrie();

            _ready = true; 
        }

        private void OnDisable()
        {
            Unsuscribe();
        }

        private void Subscrie()
        {
            ITimeUtilitiesEventsHandler.OnGetCurrentFixedUpdateTime += GetCurrentFixedUpdateTime;
            ITimeUtilitiesEventsHandler.OnGetLastFixedUpdateDeltaTime += GetLastFixedUpdateDeltaTime;
            ITimeUtilitiesEventsHandler.OnGetWwwCurrenDateTime += GetWwwCurrenDateTime;
        }

        private void Unsuscribe()
        {
            ITimeUtilitiesEventsHandler.OnGetCurrentFixedUpdateTime -= GetCurrentFixedUpdateTime;
            ITimeUtilitiesEventsHandler.OnGetLastFixedUpdateDeltaTime -= GetLastFixedUpdateDeltaTime;
            ITimeUtilitiesEventsHandler.OnGetWwwCurrenDateTime -= GetWwwCurrenDateTime;
        }

        private void Update()
        {
            var secondsDif = Time.realtimeSinceStartup - SecondsFromAppStartConnectionEstablished;
            WwwConnectsEstablishedDateTime = WwwConnectsEstablishedDateTime.AddSeconds((double)secondsDif);
        }

        public DateTime GetWwwCurrenDateTime() => WwwConnectsEstablishedDateTime;

        private void FixedUpdate()
        {
            float currentTime = Time.fixedTime - FixedUpdatePauseDelay;
            float deltaTime = (IsApplicationPaused ? 0f : Mathf.Max(0, currentTime - FixeUpdateTimeSinceStart)) * Time.timeScale;

            FixeUpdateTimeSinceStart += deltaTime;
            LastFixedUpdateDeltaTime = deltaTime;

            if(deltaTime!=0)
                ITimeUtilitiesEventsCallbackInvoker.UnityFixedUpdate(); 
        }

        private float GetCurrentFixedUpdateTime() => FixeUpdateTimeSinceStart;
        private float GetLastFixedUpdateDeltaTime() => LastFixedUpdateDeltaTime;

        private void OnApplicationPause(bool pause)
        {
            IsApplicationPaused = pause;

            if(pause)
            {
                FixedUpdateTimehenasPaused = Time.fixedTime;
            }
            else
            {
                FixedUpdatePauseDelay += Time.fixedTime - FixedUpdateTimehenasPaused;
                FixedUpdateTimehenasPaused = 0; 
            }
        }

        private async Task InitializeWwwDateTime()
        {
            for (int i = 0; i < InternetTimeDataSources.Length; i++)
            {
                var result = await IInternetTools.TryToGetRequestAnswerTcp(
                                                InternetTimeDataSources[i].URL,
                                                InternetTimeDataSources[i].Port
                                                );

                var wwwDateTime = GetDateTimeFromInternetAnswer(result);

                if (wwwDateTime != null)
                {
                    WwwConnectsEstablishedDateTime = wwwDateTime.Value;
                    SecondsFromAppStartConnectionEstablished = Time.realtimeSinceStartup;
                    break;
                }
            }
        }

        private DateTime? GetDateTimeFromInternetAnswer(string str)
        {
            DateTime wwwDateTime;

            try
            {
                var utcDateTimeString = str.Substring(7, 17);
                wwwDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                return wwwDateTime;
            }
            catch (Exception exception)
            {
                Debug.LogWarning("\t Couldn't not transform string: " + str + "\t in to the DateTime object: " + exception);
            }

            return null;
        }
    }
}
