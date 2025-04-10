using System;

namespace Scripts.BaseSystems
{
    public interface IEventDataUpdated
    {
        public event Action OnDataUpdated;
    }
}
