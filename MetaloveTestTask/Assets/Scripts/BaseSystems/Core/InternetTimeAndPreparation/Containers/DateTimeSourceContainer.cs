using System;

namespace Scripts.BaseSystems.Core
{
    [Serializable]
    public struct DateTimeSourceContainer 
    {
        public DateTimeSourceId _dateTimeSourceId;
        public SocketContainer _socket; 
    }
}
