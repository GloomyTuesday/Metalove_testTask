using System;
using System.Collections.Generic;

namespace Scripts.BaseSystems.NetworkBase
{
    [Serializable]
    //  Network Player Base Systems
    public class NetworkPlayerBs : INetworkPlayerBs
    {
        public string _playerName;
        public string _playerGlobalId;    //  This information is secret and should not be accesible to other players
        public Dictionary<object, object> _customProperty = new Dictionary<object, object>();
        public int _gameSessionNetworkId;

        public string PlayerName => _playerName;
        public string PlayerGlobalId => _playerGlobalId;
        public Dictionary<object, object> CustomProperty => _customProperty;
        public int GameSessionNetworkId => _gameSessionNetworkId;

        public NetworkPlayerBs(string playerName, int gameSessionNetworkId, Dictionary<object, object> customProperty = null)
        {
            _playerName = playerName;
            _playerGlobalId = null;
            _gameSessionNetworkId = gameSessionNetworkId;

            if(customProperty!=null)
                _customProperty = customProperty; 
        }

        public NetworkPlayerBs(INetworkPlayerBs playerData)
        {
            _playerName = playerData.PlayerName;
            _playerGlobalId = playerData.PlayerGlobalId;
            _gameSessionNetworkId = playerData.GameSessionNetworkId;

            foreach (var item in playerData.CustomProperty)
                _customProperty.Add(item.Key, item.Value);
        }

        public NetworkPlayerBs(string playerName, string playerGlobalId, int gameSessionNetworkId, Dictionary<object, object> customProperty = null)
        {
            _playerName = playerName;
            _playerGlobalId = playerGlobalId;
            _gameSessionNetworkId = gameSessionNetworkId;

            if (customProperty != null)
                _customProperty = new Dictionary<object, object>(customProperty);
        }

        public void CopyDataFrom(INetworkPlayerBs playerData)
        {
            _customProperty = new Dictionary<object, object>(playerData.CustomProperty);

            _playerName = playerData.PlayerName;
            _playerGlobalId = playerData.PlayerGlobalId;
            _gameSessionNetworkId = playerData.GameSessionNetworkId;
        }

        public T GetProperty<T>(object key)
        {
            if (!_customProperty.ContainsKey(key)) return default;
            if (!(_customProperty[key] is not T)) return default;
            return (T)_customProperty[key];
        }
    }
}
