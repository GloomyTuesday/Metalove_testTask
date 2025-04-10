using System;

namespace Scripts.BaseSystems
{
    public interface ITimeUtilitiesEventsCallbackHandler
    {
        public event Action OnUnityFixedUpdate;
    }
}
