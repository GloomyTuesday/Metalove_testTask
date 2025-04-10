using System.Collections.Generic;

namespace Scripts.BaseSystems.NetworkBase
{ 
    public interface INetworkPlayerBs
    {
        public string PlayerName { get; }
        public string PlayerGlobalId { get; } //  This information is secret and should not be accesible to other players
        public int GameSessionNetworkId { get; }
        public Dictionary<object, object> CustomProperty { get; }
        public T GetProperty<T>(object key);
    }
}
